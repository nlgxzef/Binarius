using System;
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
            this.BackColor = Theme.MainBackColor;
            this.ForeColor = Theme.MainForeColor;
            this.CheckButtonOK.BackColor = Theme.ButtonBackColor;
            this.CheckButtonOK.ForeColor = Theme.ButtonForeColor;
            this.CheckButtonOK.FlatAppearance.BorderColor = Theme.ButtonFlatColor;
            this.CheckButtonCancel.BackColor = Theme.ButtonBackColor;
            this.CheckButtonCancel.ForeColor = Theme.ButtonForeColor;
            this.CheckButtonCancel.FlatAppearance.BorderColor = Theme.ButtonFlatColor;
            this.CheckBoxSelection.ForeColor = Theme.LabelTextColor;
        }

        private void CheckButtonOK_Click(object sender, EventArgs e)
        {
            this.Value = this.CheckBoxSelection.Checked;
            this.Close();
        }

        private void CheckButtonCancel_Click(object sender, EventArgs e) => this.Close();
    }
}
