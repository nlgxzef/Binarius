using Binary.Properties;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Prompt
{
    public partial class CheckError : Form
    {
        public bool Value { get; private set; } = false;

        public CheckError()
        {
            this.InitializeComponent();
            this.ToggleTheme();
        }

        public CheckError(string desc, bool prompt) : this(desc, prompt, false)
        {
        }

        public CheckError(string desc, bool prompt, bool initiallyChecked) : this()
        {
            this.CheckButtonCancel.Enabled = !prompt;
            this.ControlBox = !prompt;
            this.CheckBoxSelection.Text = desc ?? String.Empty;
            this.CheckBoxSelection.Checked = initiallyChecked;
        }

        public CheckError(string desc, string promptName, bool prompt, bool initiallyChecked) : this()
        {
            this.CheckButtonCancel.Enabled = !prompt;
            this.ControlBox = !prompt;
            this.CheckBoxSelection.Text = promptName ?? String.Empty;
            this.CheckBoxSelection.Checked = initiallyChecked;
            this.LabelDescription.Text = desc;
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.CheckButtonOK.BackColor = theme.Colors.ButtonBackColor;
            this.CheckButtonOK.ForeColor = theme.Colors.ButtonForeColor;
            this.CheckButtonOK.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CheckButtonCancel.BackColor = theme.Colors.ButtonBackColor;
            this.CheckButtonCancel.ForeColor = theme.Colors.ButtonForeColor;
            this.CheckButtonCancel.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CheckBoxSelection.ForeColor = theme.Colors.LabelTextColor;
        }

        private void CheckButtonOK_Click(object sender, EventArgs e)
        {
            this.Value = this.CheckBoxSelection.Checked;
            this.Close();
        }

        private void CheckButtonCancel_Click(object sender, EventArgs e) => this.Close();
    }
}
