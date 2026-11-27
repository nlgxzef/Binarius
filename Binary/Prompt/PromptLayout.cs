using System;
using System.Drawing;
using System.Windows.Forms;



namespace Binary.Prompt
{
    /// <summary>
    /// Lays out prompt dialogs at runtime so their text always fits: content controls are
    /// stacked top-down, text is word-wrapped and measured, the dialog grows to fit it, and
    /// buttons are docked to the lower right corner.
    /// </summary>
    internal static class PromptLayout
    {
        private const int Margin = 12;
        private const int Gap = 8;
        private const int ButtonGap = 6;
        private const int MinContentWidth = 344;
        private const int MaxContentWidth = 480;
        private const int CheckGlyphWidth = 24;
        private const int TextSafety = 8; // labels draw with a small inset; measure narrower so text never wraps unexpectedly

        /// <summary>
        /// Arranges <paramref name="content"/> top-down and <paramref name="buttons"/> in the
        /// lower right corner, then resizes and recenters <paramref name="form"/>.
        /// Call from the form's OnLoad, after DPI scaling has been applied.
        /// </summary>
        /// <param name="form">Dialog to arrange.</param>
        /// <param name="icon">Optional icon shown to the left of the content; may be null.</param>
        /// <param name="content">Labels, check boxes, text boxes and combo boxes, top to bottom.</param>
        /// <param name="buttons">Buttons, left to right.</param>
        public static void Arrange(Form form, Control icon, Control[] content, Button[] buttons)
        {
            int L(int value) => form.LogicalToDeviceUnits(value);

            var margin = L(Margin);
            var gap = L(Gap);
            var area = Screen.FromControl(form).WorkingArea;

            var left = margin;
            if (icon != null) left += icon.Width + L(Gap * 2);

            // Width available for content: grow with the text, but never wider than the screen allows
            var maxWidth = Math.Min(L(MaxContentWidth), (int)(area.Width * 0.6) - left - margin);
            maxWidth = Math.Max(maxWidth, L(MinContentWidth));

            // Buttons must fit their text at any DPI
            foreach (var button in buttons)
            {
                if (button.AutoSize) continue;
                var need = button.GetPreferredSize(Size.Empty);
                button.Size = new Size(Math.Max(button.Width, need.Width), Math.Max(button.Height, need.Height));
            }

            var buttonsWidth = 0;
            foreach (var button in buttons) buttonsWidth += button.Width;
            buttonsWidth += Math.Max(0, buttons.Length - 1) * L(ButtonGap);

            // Widest single-line need among text controls decides the dialog width
            var width = Math.Max(L(MinContentWidth), buttonsWidth - (left - margin));

            foreach (var control in content)
            {
                var extra = control is CheckBox ? L(CheckGlyphWidth) : 0;
                if (!IsTextControl(control)) continue;
                var single = TextRenderer.MeasureText(control.Text, control.Font).Width;
                if (control is Label label) single = Math.Max(single, label.GetPreferredSize(Size.Empty).Width); // honours GDI+ rendering too
                width = Math.Max(width, Math.Min(maxWidth, single + extra + L(TextSafety)));
            }

            // Resize the form first: anchored controls would otherwise be shifted by the resize
            foreach (var control in content) control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            foreach (var button in buttons) button.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            if (icon != null) icon.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // Stack content top-down
            var y = margin;

            foreach (var control in content)
            {
                if (IsTextControl(control))
                {
                    var extra = control is CheckBox ? L(CheckGlyphWidth) : 0;
                    var measured = TextRenderer.MeasureText(control.Text, control.Font,
                        new Size(width - extra - L(TextSafety), Int32.MaxValue),
                        TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);

                    control.AutoSize = false;
                    control.MaximumSize = Size.Empty;
                    control.MinimumSize = Size.Empty;

                    var height = measured.Height + L(4);

                    // Ask the label itself as well: it knows its real rendering mode (GDI or GDI+)
                    if (control is Label textLabel)
                        height = Math.Max(height, textLabel.GetPreferredSize(new Size(width - L(TextSafety), 0)).Height + L(4));

                    if (control is CheckBox check)
                    {
                        height = Math.Max(height, L(20));
                        check.CheckAlign = ContentAlignment.TopLeft;
                        check.TextAlign = ContentAlignment.TopLeft;
                    }

                    control.SetBounds(left, y, width, height);
                }
                else
                {
                    control.SetBounds(left, y, width, control.Height);
                }

                y += control.Height + gap;
            }

            var contentBottom = y - gap;

            if (icon != null)
            {
                icon.Location = new Point(margin, margin);
                contentBottom = Math.Max(contentBottom, icon.Bottom);
            }

            // Dock buttons to the lower right, in left-to-right order
            var buttonHeight = 0;
            foreach (var button in buttons) buttonHeight = Math.Max(buttonHeight, button.Height);

            var buttonTop = contentBottom + L(Margin);
            var right = left + width;

            for (int i = buttons.Length - 1; i >= 0; --i)
            {
                var button = buttons[i];
                button.Location = new Point(right - button.Width, buttonTop);
                right -= button.Width + L(ButtonGap);
            }

            form.ClientSize = new Size(left + width + margin, buttonTop + buttonHeight + margin);

            // Anchor only after the final size is set, so later resizes keep buttons in the lower right
            foreach (var control in content) control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            foreach (var button in buttons) button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Keep the dialog on screen and centered
            var bounds = form.Bounds;
            var x = area.Left + (area.Width - bounds.Width) / 2;
            var top = area.Top + (area.Height - bounds.Height) / 2;
            form.Location = new Point(Math.Max(area.Left, x), Math.Max(area.Top, top));
        }

        private static bool IsTextControl(Control control) => control is Label || control is CheckBox;
    }
}
