using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace Binary.UI
{
    /// <summary>
    /// Makes fixed-position dialogs work at any DPI scaling. WinForms scales control positions by the DPI factor,
    /// but text does not grow exactly in proportion, so labels run into the fields next to them and buttons crop
    /// their text. When a dialog loads (after scaling), this:
    /// <list type="number">
    /// <item>grows buttons, check boxes and fixed-size labels to fit their text,</item>
    /// <item>re-flows each container horizontally and vertically so nothing overlaps, keeping the original gaps
    /// and keeping controls that started in the same column (or row) aligned,</item>
    /// <item>grows containers and the dialog itself to fit, keeping the original margins.</item>
    /// </list>
    /// Nothing moves when nothing collides.
    /// </summary>
    public static class DialogLayout
    {
        /// <summary>
        /// Lays out <paramref name="form"/> when it loads.
        /// </summary>
        public static void Attach(Form form) => form.Load += (s, e) => Apply(form);

        /// <summary>
        /// Lays out <paramref name="form"/> now.
        /// </summary>
        public static void Apply(Form form)
        {
            form.SuspendLayout();

            try
            {
                var anchors = new Dictionary<Control, AnchorStyles>();
                Freeze(form, anchors);
                FitContainer(form, form.Font.Height);
                foreach (var pair in anchors) pair.Key.Anchor = pair.Value;
            }
            finally
            {
                form.ResumeLayout(true);
            }

            // Keep the dialog on screen
            var area = Screen.FromControl(form).WorkingArea;
            var x = Math.Max(area.Left, Math.Min(form.Left, area.Right - form.Width));
            var y = Math.Max(area.Top, Math.Min(form.Top, area.Bottom - form.Height));
            if (form.StartPosition == FormStartPosition.CenterScreen || form.StartPosition == FormStartPosition.CenterParent)
            {
                x = Math.Max(area.Left, area.Left + (area.Width - form.Width) / 2);
                y = Math.Max(area.Top, area.Top + (area.Height - form.Height) / 2);
            }
            form.Location = new Point(x, y);
        }

        // Anchors would move controls a second time when containers grow; pin everything to top-left meanwhile.
        private static void Freeze(Control control, Dictionary<Control, AnchorStyles> anchors)
        {
            foreach (Control child in control.Controls)
            {
                if (child.Dock == DockStyle.None)
                {
                    anchors[child] = child.Anchor;
                    child.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                }

                Freeze(child, anchors);
            }
        }

        private static bool IsLayoutContainer(Control c) =>
            (c is Panel || c is GroupBox || c is TabPage || c is UserControl) && c.Controls.Count > 0;

        /// <summary>
        /// Lays out the children of <paramref name="container"/> and grows it to fit them.
        /// </summary>
        private static void FitContainer(Control container, int fontHeight)
        {
            var children = container.Controls.Cast<Control>().Where(c => c.Visible && c.Dock == DockStyle.None && c is not ToolStrip).ToList();
            if (children.Count == 0) return;
            if (container.Controls.Cast<Control>().Any(c => c.Visible && c.Dock != DockStyle.None && c is not ToolStrip && c is not StatusStrip)) return;

            var original = children.ToDictionary(c => c, c => c.Bounds);
            var client = container.ClientSize;
            var rightMargin = Math.Max(0, client.Width - children.Max(c => c.Right));
            var bottomMargin = Math.Max(0, client.Height - children.Max(c => c.Bottom));

            // 1. Inner containers first, then grow controls to fit their text
            foreach (var c in children)
            {
                if (IsLayoutContainer(c)) FitContainer(c, fontHeight);
                GrowToText(c);
            }

            var minGap = Math.Max(2, fontHeight / 5);

            // 2. Horizontal: process columns (same original Left) from left to right
            foreach (var column in children.GroupBy(c => original[c].Left / 3).OrderBy(g => g.Key))
            {
                var newLeft = column.Max(c =>
                {
                    var left = original[c].Left;

                    foreach (var a in children)
                    {
                        if (!IsLeftOf(original[a], original[c])) continue;

                        // Keep the designed gap; if scaling already made them touch/overlap, use a minimum gap
                        var gap = Math.Max(minGap, original[c].Left - original[a].Right);
                        left = Math.Max(left, a.Right + gap);
                    }

                    return left;
                });

                foreach (var c in column) c.Left = newLeft;
            }

            // 3. Vertical: rows (controls overlapping vertically, e.g. label + field + button) move as a unit,
            //    from top to bottom, by the largest shift any of their members needs
            foreach (var row in Rows(children, original))
            {
                var shift = row.Max(c =>
                {
                    var top = original[c].Top;

                    foreach (var a in children)
                    {
                        if (row.Contains(a) || !IsAbove(original[a], original[c]) || !SameColumn(a.Bounds, c.Bounds)) continue;

                        var gap = Math.Max(0, original[c].Top - original[a].Bottom);
                        top = Math.Max(top, a.Bottom + gap);
                    }

                    return top - original[c].Top;
                });

                foreach (var c in row) c.Top = original[c].Top + shift;
            }

            // 4. Grow the container to fit, keeping the original margins
            var needWidth = children.Max(c => c.Right) + rightMargin;
            var needHeight = children.Max(c => c.Bottom) + bottomMargin;

            if (needWidth > client.Width || needHeight > client.Height)
            {
                var size = new Size(Math.Max(client.Width, needWidth), Math.Max(client.Height, needHeight));

                if (container is Form form) form.ClientSize = size;
                else container.Size += size - client;
            }
        }

        // Groups controls into rows: a control joins a row when it overlaps one of its members vertically
        // (by more than a third of the smaller height). Rows are returned top to bottom.
        private static List<List<Control>> Rows(List<Control> children, Dictionary<Control, Rectangle> original)
        {
            var rows = new List<List<Control>>();

            foreach (var c in children.OrderBy(c => original[c].Top))
            {
                // Join a row only if it overlaps a member and no member is stacked above/below it (e.g. a title and
                // its description next to a tall picture stay separate rows)
                var row = rows.FirstOrDefault(r =>
                    r.Any(m => SameRow(original[m], original[c]) && !Encloses(original[m], original[c]) && !Encloses(original[c], original[m])) &&
                    r.All(m => !(IsAbove(original[m], original[c]) && SameColumn(original[m], original[c])) &&
                               !(IsAbove(original[c], original[m]) && SameColumn(original[m], original[c]))));
                if (row is null) rows.Add(row = new List<Control>());
                row.Add(c);
            }

            return rows.OrderBy(r => r.Min(c => original[c].Top)).ToList();
        }

        // a is a control earlier in the same row: starts left of b, its middle is left of b's start
        // (so a label that scaling pushed into its field still counts), and it is not a backdrop enclosing b.
        private static bool IsLeftOf(Rectangle a, Rectangle b) =>
            a != b && a.Left < b.Left && a.Left + a.Width / 2 < b.Left && SameRow(a, b) && !Encloses(a, b);

        // a is above b in the same column (its middle is above b's top) and is not a backdrop enclosing b.
        private static bool IsAbove(Rectangle a, Rectangle b) =>
            a != b && a.Top < b.Top && a.Top + a.Height / 2 < b.Top && !Encloses(a, b);

        private static bool Encloses(Rectangle a, Rectangle b) => a.Contains(b.Left + b.Width / 2, b.Top + b.Height / 2);

        private static bool SameRow(Rectangle a, Rectangle b)
        {
            var overlap = Math.Min(a.Bottom, b.Bottom) - Math.Max(a.Top, b.Top);
            return overlap > Math.Min(a.Height, b.Height) / 3;
        }

        private static bool SameColumn(Rectangle a, Rectangle b)
        {
            var overlap = Math.Min(a.Right, b.Right) - Math.Max(a.Left, b.Left);
            return overlap > 2;
        }

        /// <summary>
        /// Grows buttons, check boxes, radio buttons and fixed-size labels so their text is not cropped.
        /// </summary>
        private static void GrowToText(Control c)
        {
            if (String.IsNullOrEmpty(c.Text) || c.AutoSize) return;

            switch (c)
            {
                case ButtonBase button:
                {
                    var need = button.GetPreferredSize(Size.Empty);
                    if (need.Width > button.Width) button.Width = need.Width;
                    if (need.Height > button.Height) button.Height = need.Height;
                    break;
                }

                case Label label:
                {
                    var need = TextRenderer.MeasureText(label.Text, label.Font, new Size(Math.Max(1, label.ClientSize.Width), Int32.MaxValue),
                        TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
                    if (need.Height > label.ClientSize.Height) label.Height += need.Height - label.ClientSize.Height;
                    break;
                }
            }
        }
    }
}
