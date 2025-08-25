using Binary.Properties;

using Nikki.Reflection.Abstract;

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;



namespace Binary.Tools
{
    public partial class Swatcher : Form
    {
        public Swatcher()
        {
            this.InitializeComponent();
            this.ToggleTheme();
            this.ColorPreview.BackColor = Color.Black;
        }

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;

            this.CopyBrightnessValue.BackColor = theme.Colors.ButtonBackColor;
            this.CopyBrightnessValue.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyBrightnessValue.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CopySaturationValue.BackColor = theme.Colors.ButtonBackColor;
            this.CopySaturationValue.ForeColor = theme.Colors.ButtonForeColor;
            this.CopySaturationValue.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CopyPaintSwatchValue.BackColor = theme.Colors.ButtonBackColor;
            this.CopyPaintSwatchValue.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyPaintSwatchValue.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.TextBoxBrightness.BackColor = theme.Colors.TextBoxBackColor;
            this.TextBoxBrightness.ForeColor = theme.Colors.TextBoxForeColor;
            this.TextBoxSaturation.BackColor = theme.Colors.TextBoxBackColor;
            this.TextBoxSaturation.ForeColor = theme.Colors.TextBoxForeColor;
            this.TextBoxPaintSwatch.BackColor = theme.Colors.TextBoxBackColor;
            this.TextBoxPaintSwatch.ForeColor = theme.Colors.TextBoxForeColor;
            this.GroupBoxRGB.ForeColor = theme.Colors.LabelTextColor;
            this.GroupBoxSwatch.ForeColor = theme.Colors.LabelTextColor;
        }

        #endregion

        private void RGBtoHSV(float red, float green, float blue)
        {
            float hue = 0; // paintswatch

            float max = red > green ? red : green;
            max = (max > blue) ? max : blue;
            float min = red < green ? red : green;
            min = (min < blue) ? min : blue;
            float brt = max;
            float dif = max - min;
            float sat = max == 0 ? 0 : dif / max;

            if (max == min)
            {
                hue = 0;
            }
            else if (max == red)
            {
                hue = ((60 * ((green - blue) / dif)) + 360) % 360;
            }
            else if (max == green)
            {
                hue = ((60 * ((blue - red) / dif)) + 120) % 360;
            }
            else if (max == blue)
            {
                hue = ((60 * ((red - green) / dif)) + 240) % 360;
            }

            hue = 90 - (hue / 4);

            this.TextBoxPaintSwatch.Text = ((int)hue).ToString();
            this.TextBoxSaturation.Text = sat.ToString();
            this.TextBoxBrightness.Text = brt.ToString();
            this.ColorPreview.BackColor = Color.FromArgb(this.TrackBar_Red.Value, this.TrackBar_Green.Value, this.TrackBar_Blue.Value);
        }

        private void TrackBar_Red_Scroll(object sender, EventArgs e)
        {
            float red = Convert.ToSingle(this.TrackBar_Red.Value) / 255;
            float green = Convert.ToSingle(this.TrackBar_Green.Value) / 255;
            float blue = Convert.ToSingle(this.TrackBar_Blue.Value) / 255;
            this.RGBtoHSV(red, green, blue);
        }

        private void TrackBar_Green_Scroll(object sender, EventArgs e)
        {
            float red = Convert.ToSingle(this.TrackBar_Red.Value) / 255;
            float green = Convert.ToSingle(this.TrackBar_Green.Value) / 255;
            float blue = Convert.ToSingle(this.TrackBar_Blue.Value) / 255;
            this.RGBtoHSV(red, green, blue);
        }

        private void TrackBar_Blue_Scroll(object sender, EventArgs e)
        {
            float red = Convert.ToSingle(this.TrackBar_Red.Value) / 255;
            float green = Convert.ToSingle(this.TrackBar_Green.Value) / 255;
            float blue = Convert.ToSingle(this.TrackBar_Blue.Value) / 255;
            this.RGBtoHSV(red, green, blue);
        }

        private void OpenWindowsColorForm_Click(object sender, EventArgs e)
        {
            if (this.SwatchDialog.ShowDialog() == DialogResult.OK)
            {

                this.TrackBar_Red.Value = this.SwatchDialog.Color.R;
                this.TrackBar_Green.Value = this.SwatchDialog.Color.G;
                this.TrackBar_Blue.Value = this.SwatchDialog.Color.B;

                float red = Convert.ToSingle(this.TrackBar_Red.Value) / 255;
                float green = Convert.ToSingle(this.TrackBar_Green.Value) / 255;
                float blue = Convert.ToSingle(this.TrackBar_Blue.Value) / 255;

                this.RGBtoHSV(red, green, blue);
            }
        }

        private void CopyPaintSwatchValue_Click(object sender, EventArgs e) => Clipboard.SetText(this.TextBoxPaintSwatch.Text);

        private void CopySaturationValue_Click(object sender, EventArgs e) => Clipboard.SetText(this.TextBoxSaturation.Text);

        private void CopyBrightnessValue_Click(object sender, EventArgs e) => Clipboard.SetText(this.TextBoxBrightness.Text);

        private void GroupBoxRGB_Paint(object sender, PaintEventArgs e)
        {
            var box = sender as GroupBox;

            Theme.Deserialize(Path.Combine("Themes", Configurations.Default.ThemeFile), out var theme);
            this.DrawGroupBox(box, e.Graphics, theme.Colors.LabelTextColor, theme.Colors.LabelTextColor);
        }

        private void GroupBoxSwatch_Paint(object sender, PaintEventArgs e)
        {
            var box = sender as GroupBox; 
            
            Theme.Deserialize(Path.Combine("Themes", Configurations.Default.ThemeFile), out var theme);
            this.DrawGroupBox(box, e.Graphics, theme.Colors.LabelTextColor, theme.Colors.LabelTextColor);
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color fore, Color border)
        {
            if (box is null)
            {
                return;
            }

            var fore_b = new SolidBrush(fore);
            var border_b = new SolidBrush(border);
            var border_p = new Pen(border_b);

            var size = g.MeasureString(box.Text, box.Font);

            int x = box.ClientRectangle.X;
            int y = box.ClientRectangle.Y + (int)(size.Height / 2);
            int width = box.ClientRectangle.Width - 1;
            int height = box.ClientRectangle.Height - (int)(size.Height / 2) - 1;

            var rect = new Rectangle(x, y, width, height);

            g.Clear(this.BackColor);
            g.DrawString(box.Text, box.Font, fore_b, box.Padding.Left, 0);

            g.DrawLine(border_p, rect.Location, new Point(rect.X, rect.Y + rect.Height));
            g.DrawLine(border_p, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            g.DrawLine(border_p, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
            g.DrawLine(border_p, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
            g.DrawLine(border_p, new Point(rect.X + box.Padding.Left + (int)size.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y));
        }
    }
}
