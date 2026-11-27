using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace Binary.UI
{
    /// <summary>
    /// Repaints the borders of text boxes and combo boxes with the theme's <see cref="Theme.ColorSet.TextBoxBorderColor"/>,
    /// replacing the white/system-colored borders Windows draws. Works for every open form and dialog once installed.
    /// </summary>
    public static class ThemedBorders
    {
        private static readonly ConditionalWeakTable<Control, object> s_hooked = new();
        private static bool s_installed;

        /// <summary>
        /// Hooks all current and future forms. Call once before the first form is shown.
        /// </summary>
        public static void Install()
        {
            if (s_installed) return;
            s_installed = true;
            Application.Idle += (s, e) =>
            {
                foreach (Form form in Application.OpenForms) Hook(form);
            };
        }

        /// <summary>
        /// Hooks a control tree right away (forms are also picked up automatically while idle).
        /// </summary>
        public static void Hook(Control control)
        {
            if (control is null || control.IsDisposed || s_hooked.TryGetValue(control, out _)) return;
            s_hooked.Add(control, null);

            switch (control)
            {
                case TextBox text when text.BorderStyle != BorderStyle.None:
                    _ = new BorderWindow(text, nonClient: true);
                    break;

                case ComboBox combo:
                    // Standard combos are drawn by visual styles (with animations that repaint outside WM_PAINT);
                    // flat ones are painted by WinForms, so their border and button can be themed reliably.
                    if (combo.FlatStyle == FlatStyle.Standard && !BorderColor.IsEmpty) combo.FlatStyle = FlatStyle.Flat;
                    _ = new BorderWindow(combo, nonClient: false);
                    break;
            }

            control.ControlAdded += (s, e) => Hook(e.Control);
            foreach (Control child in control.Controls) Hook(child);
        }

        /// <summary>
        /// Current border color, or <see cref="Color.Empty"/> to keep the native border.
        /// </summary>
        internal static Color BorderColor => CurrentTheme()?.Colors.TextBoxBorderColor ?? Color.Empty;

        private static Theme CurrentTheme()
        {
            try
            {
                Theme.Deserialize(Theme.GetThemeFile(), out var theme);
                return theme;
            }
            catch
            {
                return null;
            }
        }

        private sealed class BorderWindow : NativeWindow
        {
            private const int WM_PAINT = 0x000F;
            private const int WM_NCPAINT = 0x0085;
            private const int WM_PRINTCLIENT = 0x0318;
            private const int PRF_CLIENT = 0x0004;
            private const int PRF_ERASEBKGND = 0x0008;
            private const int WM_SETFOCUS = 0x0007;
            private const int WM_KILLFOCUS = 0x0008;
            private const int WM_ENABLE = 0x000A;
            private const int WM_MOUSEMOVE = 0x0200;
            private const int WM_MOUSELEAVE = 0x02A3;
            private const int WM_NCMOUSEMOVE = 0x00A0;
            private const int WM_NCMOUSELEAVE = 0x02A2;

            private readonly Control _control;
            private readonly bool _nonClient;

            public BorderWindow(Control control, bool nonClient)
            {
                this._control = control;
                this._nonClient = nonClient;

                if (control.IsHandleCreated) this.Attach();
                control.HandleCreated += (s, e) => this.Attach();
                control.HandleDestroyed += (s, e) => this.ReleaseHandle();
            }

            private void Attach()
            {
                if (this.Handle != IntPtr.Zero) this.ReleaseHandle();
                this.AssignHandle(this._control.Handle);

                // Multi-line text boxes with scroll bars and no frame: add a real 1 px frame (WS_BORDER) so the scroll
                // bars sit inside it. Windows then never paints the outer pixel that carries our border.
                if (this._nonClient && !BorderColor.IsEmpty && HasScrollBars(this._control) && this.Insets().Left == 0)
                {
                    var style = GetWindowLong(this.Handle, GWL_STYLE);
                    SetWindowLong(this.Handle, GWL_STYLE, style | WS_BORDER);
                    SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);
                }

                // Repaint now so already visible controls lose the native border
                RedrawWindow(this._control.Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_INVALIDATE | RDW_UPDATENOW);
            }

            protected override void WndProc(ref Message m)
            {
                var theme = CurrentTheme();
                var themed = theme != null && !theme.Colors.TextBoxBorderColor.IsEmpty && !this._control.IsDisposed;

                // Text boxes either have a Windows frame around the text area, or (FixedSingle with visual styles)
                // no frame and a 1 px border that WinForms paints inside the text area. The latter may still have
                // scroll bars outside the text area. Combo boxes paint everything in the client area.
                var insets = this.Insets();
                var hasFrame = this._nonClient && insets.Left > 0;
                var scroll = this._nonClient && HasScrollBars(this._control);

                if (themed && (!hasFrame || scroll) && m.Msg == WM_PAINT && m.WParam == IntPtr.Zero)
                {
                    // Paint off-screen (native drawing + our border/button), then copy in one step, so the
                    // screen never shows the native border for a moment (hover flicker)
                    this.PaintBuffered(ref m, theme, insets);
                    return;
                }

                if (themed && hasFrame && m.Msg == WM_NCPAINT)
                {
                    if (scroll)
                    {
                        // Let Windows' default handler draw only the scroll bars: clip away the frame ring and bypass
                        // the WinForms handler (which draws its own line around the text area)
                        GetWindowRect(this.Handle, out var w);
                        var ring = Math.Max(1, insets.Left);
                        var region = CreateRectRgn(w.Left + ring, w.Top + ring, w.Right - ring, w.Bottom - ring);
                        DefWindowProc(this.Handle, WM_NCPAINT, region, IntPtr.Zero);
                        DeleteObject(region);
                    }

                    // The frame itself is drawn only by us
                    this.DrawFrame(theme);
                    m.Result = IntPtr.Zero;
                    return;
                }

                base.WndProc(ref m);

                if (!themed) return;

                if (hasFrame && !scroll && m.Msg is WM_SETFOCUS or WM_KILLFOCUS or WM_MOUSEMOVE or WM_MOUSELEAVE or WM_NCMOUSEMOVE or WM_NCMOUSELEAVE or WM_ENABLE)
                {
                    // Visual styles may repaint a frame outside WM_NCPAINT; draw ours on top again
                    this.DrawFrame(theme);
                }
                else if ((!hasFrame || scroll) && m.Msg == WM_PRINTCLIENT)
                {
                    this.DrawClientBorder(theme, insets);
                }
            }

            /// <summary>
            /// Width of the non-client area on each side (frame and/or scroll bars).
            /// </summary>
            private Padding Insets()
            {
                if (!this._control.IsHandleCreated) return Padding.Empty;

                GetWindowRect(this.Handle, out var window);
                var origin = new POINT();
                ClientToScreen(this.Handle, ref origin);
                var client = this._control.ClientSize;

                var left = origin.X - window.Left;
                var top = origin.Y - window.Top;
                return new Padding(left, top, window.Right - window.Left - left - client.Width, window.Bottom - window.Top - top - client.Height);
            }

            private static bool HasScrollBars(Control c)
            {
                var style = GetWindowLong(c.Handle, GWL_STYLE);
                return (style & (WS_HSCROLL | WS_VSCROLL)) != 0;
            }

            private void PaintBuffered(ref Message m, Theme theme, Padding insets)
            {
                var ps = new PAINTSTRUCT();
                var hdc = BeginPaint(this.Handle, ref ps);
                var size = this._control.ClientSize;

                if (hdc == IntPtr.Zero || size.Width <= 0 || size.Height <= 0)
                {
                    if (hdc != IntPtr.Zero) EndPaint(this.Handle, ref ps);
                    return;
                }

                var mem = CreateCompatibleDC(hdc);
                var bmp = CreateCompatibleBitmap(hdc, size.Width, size.Height);
                var old = SelectObject(mem, bmp);

                try
                {
                    // Background first: the new bitmap is black, and many controls paint their background in
                    // WM_ERASEBKGND rather than WM_PAINT
                    using (var g = Graphics.FromHdc(mem))
                    using (var back = new SolidBrush(this._control.BackColor))
                    {
                        g.FillRectangle(back, 0, 0, size.Width, size.Height);
                    }

                    if (this._control is ComboBox combo)
                    {
                        // Closed combo box: draw it ourselves (native rendering into a bitmap ignores the themed
                        // back color). Editable combos have a child edit box that draws its own text on top.
                        if (combo.DropDownStyle == ComboBoxStyle.DropDownList)
                        {
                            using var g = Graphics.FromHdc(mem);
                            var arrow = SystemInformation.GetHorizontalScrollBarArrowWidthForDpi(combo.DeviceDpi);
                            var textRect = new Rectangle(3, 1, Math.Max(0, size.Width - arrow - 6), size.Height - 2);
                            var color = combo.Enabled ? combo.ForeColor : SystemColors.GrayText;
                            TextRenderer.DrawText(g, combo.Text, combo.Font, textRect, color,
                                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                        }
                    }
                    else
                    {
                        // Let the control render its client area into the memory DC (WM_PRINTCLIENT is the message
                        // meant for this)
                        var print = Message.Create(this.Handle, WM_PRINTCLIENT, mem, (IntPtr)(PRF_CLIENT | PRF_ERASEBKGND));
                        base.WndProc(ref print);
                    }

                    // Same for the memory DC after WM_PRINTCLIENT, or our border lines would be clipped away
                    SelectClipRgn(mem, IntPtr.Zero);
                    using (var g = Graphics.FromHdc(mem)) this.DrawClientOverlay(g, size, theme, insets);

                    // WinForms excludes the border from the clip region of the DCs it paints with (here: the screen DC
                    // during WM_ERASEBKGND in BeginPaint); clear it so the finished image reaches the border pixels too
                    SelectClipRgn(hdc, IntPtr.Zero);
                    BitBlt(hdc, 0, 0, size.Width, size.Height, mem, 0, 0, SRCCOPY);
                }
                finally
                {
                    SelectObject(mem, old);
                    DeleteObject(bmp);
                    DeleteDC(mem);
                    EndPaint(this.Handle, ref ps);
                }

                m.Result = IntPtr.Zero;
            }

            private void DrawFrame(Theme theme)
            {
                var hdc = GetWindowDC(this.Handle);
                if (hdc == IntPtr.Zero) return;

                try
                {
                    using var g = Graphics.FromHdc(hdc);
                    var insets = this.Insets();

                    // Frame: cover all of it (1 px FixedSingle, 2 px Fixed3D); scroll bars only: 1 px outer border
                    var thickness = Math.Max(1, insets.Left);
                    this.DrawOverlay(g, this._control.Size, thickness, theme);
                }
                finally
                {
                    ReleaseDC(this.Handle, hdc);
                }
            }

            private void DrawClientBorder(Theme theme, Padding insets)
            {
                var hdc = GetDC(this.Handle);
                if (hdc == IntPtr.Zero) return;

                try
                {
                    using var g = Graphics.FromHdc(hdc);
                    this.DrawClientOverlay(g, this._control.ClientSize, theme, insets);
                }
                finally
                {
                    ReleaseDC(this.Handle, hdc);
                }
            }

            /// <summary>
            /// Border inside the client area: sides that are also the control's outer edge get the theme border;
            /// sides next to a scroll bar get the back color, hiding the line WinForms draws there.
            /// </summary>
            private void DrawClientOverlay(Graphics g, Size size, Theme theme, Padding insets)
            {
                if (this._control is not TextBox || insets == Padding.Empty)
                {
                    this.DrawOverlay(g, size, 1, theme);
                    return;
                }

                // Sides that are the outer edge get the theme border; sides next to a frame or scroll bar get the
                // back color, hiding the line WinForms draws around the text area
                using var border = new Pen(theme.Colors.TextBoxBorderColor);
                using var back = new Pen(this._control.BackColor);
                int r = size.Width - 1, b = size.Height - 1;

                g.DrawLine(insets.Top > 0 ? back : border, 0, 0, r, 0);
                g.DrawLine(insets.Left > 0 ? back : border, 0, 0, 0, b);
                g.DrawLine(insets.Right > 0 ? back : border, r, 0, r, b);
                g.DrawLine(insets.Bottom > 0 ? back : border, 0, b, r, b);
            }

            private void DrawOverlay(Graphics g, Size size, int thickness, Theme theme)
            {
                // Flat combo boxes: replace the white drop-down button with a themed one
                if (this._control is ComboBox combo && combo.FlatStyle == FlatStyle.Flat && combo.DropDownStyle != ComboBoxStyle.Simple)
                {
                    var width = SystemInformation.GetHorizontalScrollBarArrowWidthForDpi(this._control.DeviceDpi);
                    var button = new Rectangle(size.Width - width - 1, 1, width, size.Height - 2);

                    using (var back = new SolidBrush(theme.Colors.ButtonBackColor)) g.FillRectangle(back, button);

                    var scale = this._control.DeviceDpi / 96f;
                    var cx = button.Left + button.Width / 2f;
                    var cy = button.Top + button.Height / 2f;
                    var w = 3.5f * scale;
                    var h = 2f * scale;

                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var arrow = new SolidBrush(theme.Colors.ButtonForeColor))
                        g.FillPolygon(arrow, new[] { new PointF(cx - w, cy - h), new PointF(cx + w, cy - h), new PointF(cx, cy + h) });
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                }

                using var outer = new Pen(theme.Colors.TextBoxBorderColor);
                g.DrawRectangle(outer, 0, 0, size.Width - 1, size.Height - 1);

                if (thickness > 1)
                {
                    using var inner = new Pen(this._control.BackColor);
                    for (int i = 1; i < thickness; ++i) g.DrawRectangle(inner, i, i, size.Width - 1 - 2 * i, size.Height - 1 - 2 * i);
                }
            }
        }

        private const uint RDW_INVALIDATE = 0x0001;
        private const uint RDW_UPDATENOW = 0x0100;
        private const uint RDW_FRAME = 0x0400;

        [DllImport("user32.dll")] private static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")] private static extern bool RedrawWindow(IntPtr hWnd, IntPtr rect, IntPtr region, uint flags);
        [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr hWnd, int index);
        [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int index, int value);
        [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hWnd, IntPtr after, int x, int y, int cx, int cy, uint flags);
        [DllImport("user32.dll", EntryPoint = "DefWindowProcW")] private static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("gdi32.dll")] private static extern IntPtr CreateRectRgn(int left, int top, int right, int bottom);
        [DllImport("gdi32.dll")] private static extern int SelectClipRgn(IntPtr hdc, IntPtr region);

        private const int WS_BORDER = 0x00800000;
        private const uint SWP_NOSIZE = 0x0001, SWP_NOMOVE = 0x0002, SWP_NOZORDER = 0x0004, SWP_NOACTIVATE = 0x0010, SWP_FRAMECHANGED = 0x0020;
        [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);
        [DllImport("user32.dll")] private static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [StructLayout(LayoutKind.Sequential)] private struct RECT { public int Left, Top, Right, Bottom; }
        [StructLayout(LayoutKind.Sequential)] private struct POINT { public int X, Y; }
        [DllImport("user32.dll")] private static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);
        [DllImport("user32.dll")] private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);
        [DllImport("gdi32.dll")] private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")] private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int height);
        [DllImport("gdi32.dll")] private static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);
        [DllImport("gdi32.dll")] private static extern bool DeleteObject(IntPtr obj);
        [DllImport("gdi32.dll")] private static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")] private static extern bool BitBlt(IntPtr dest, int x, int y, int w, int h, IntPtr src, int sx, int sy, uint rop);

        private const int GWL_STYLE = -16;
        private const int WS_HSCROLL = 0x00100000;
        private const int WS_VSCROLL = 0x00200000;
        private const uint SRCCOPY = 0x00CC0020;

        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public int left, top, right, bottom;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
        }
    }
}
