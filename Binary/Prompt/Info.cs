using System;
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
            this.BackColor = Theme.MainBackColor;
            this.ForeColor = Theme.MainForeColor;
            this.InfoButtonOK.BackColor = Theme.ButtonBackColor;
            this.InfoButtonOK.ForeColor = Theme.ButtonForeColor;
            this.InfoButtonOK.FlatAppearance.BorderColor = Theme.ButtonFlatColor;
            this.InfoLabel.ForeColor = Theme.LabelTextColor;
        }

        private void InfoButtonOK_Click(object sender, EventArgs e) => this.Close();
    }
}
