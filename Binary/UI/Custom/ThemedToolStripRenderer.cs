using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    /// Professional renderer that draws disabled items in a color made from the theme's own colors,
    /// instead of the fixed Windows gray that can match or vanish into a theme.
    /// </summary>
    internal class ThemedToolStripRenderer : ToolStripProfessionalRenderer
    {
        public ThemedToolStripRenderer() : base() { }

        public ThemedToolStripRenderer(ProfessionalColorTable table) : base(table) { }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            // The base renderer always draws disabled text in the Windows gray, whatever color is given to it
            if (!e.Item.Enabled && e.TextDirection == ToolStripTextDirection.Horizontal && !String.IsNullOrEmpty(e.Text))
            {

                var back = e.Item.IsOnDropDown || e.ToolStrip is null ? e.Item.BackColor : e.ToolStrip.BackColor;
                TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont, e.TextRectangle, DisabledColor(e.Item.ForeColor, back), e.TextFormat);
                return;

            }

            base.OnRenderItemText(e);
        }

        // A menu separator in the colors of its menu items: the base renderer leaves the default background behind it
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if (e.Vertical || !e.Item.IsOnDropDown)
            {

                base.OnRenderSeparator(e);
                return;

            }

            var size = e.Item.Size;
            var margin = ImageMarginWidth(e.ToolStrip);

            // The whole row, as menu items paint their color over the column of images too
            using (var back = new SolidBrush(e.Item.BackColor))
            {

                e.Graphics.FillRectangle(back, 0, 0, size.Width, size.Height);

            }

            using var pen = new Pen(DisabledColor(e.Item.ForeColor, e.Item.BackColor));
            var y = size.Height / 2;
            e.Graphics.DrawLine(pen, margin + 4, y, size.Width - 4, y);
        }

        private static readonly System.Reflection.PropertyInfo s_imageMargin = typeof(ToolStripDropDownMenu)
            .GetProperty("ImageMargin", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        // Width of the column of images and checks on the left of a menu, which the renderer paints itself
        private static int ImageMarginWidth(ToolStrip strip)
        {
            if (strip is ToolStripDropDownMenu menu && (menu.ShowImageMargin || menu.ShowCheckMargin) &&
                s_imageMargin?.GetValue(menu) is Rectangle margin) return margin.Right;

            return 0;
        }

        /// <summary>
        /// The text color moved part of the way toward the background: clearly dimmer than enabled text,
        /// still readable on it.
        /// </summary>
        public static Color DisabledColor(Color fore, Color back)
        {
            const float toward = 0.45f;

            static int Mix(int f, int b) => (int)Math.Round(f + (b - f) * toward);

            return Color.FromArgb(Mix(fore.R, back.R), Mix(fore.G, back.G), Mix(fore.B, back.B));
        }
    }
}
