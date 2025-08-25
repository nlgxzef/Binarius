using Binary.Properties;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Prompt
{
    public partial class Info : Form
    {
        public Info()
        {
            this.InitializeComponent();
            this.ToggleTheme();
        }

        public Info(string desc) : this()
        {
            this.InfoLabel.Text = desc ?? String.Empty;
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.InfoButtonOK.BackColor = theme.Colors.ButtonBackColor;
            this.InfoButtonOK.ForeColor = theme.Colors.ButtonForeColor;
            this.InfoButtonOK.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.InfoLabel.ForeColor = theme.Colors.LabelTextColor;
        }

        private void InfoButtonOK_Click(object sender, EventArgs e) => this.Close();
    }
}
