using Binary.Properties;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Prompt
{
    public partial class Combo : Form
    {
        public int Value { get; private set; } = 0;

        public Combo()
        {
            this.InitializeComponent();
            this.ToggleTheme();
        }

        public Combo(string[] options, string desc, bool prompt) : this()
        {
            this.ComboButtonCancel.Enabled = !prompt;
            this.ControlBox = !prompt;
            this.DescriptionLabel.Text = desc ?? String.Empty;
            if (options == null)
            {
                return;
            }

            this.ComboBoxSelection.Items.AddRange(options);
            this.ComboBoxSelection.SelectedIndex = 0;
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.ComboButtonOK.BackColor = theme.Colors.ButtonBackColor;
            this.ComboButtonOK.ForeColor = theme.Colors.ButtonForeColor;
            this.ComboButtonOK.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.ComboButtonCancel.BackColor = theme.Colors.ButtonBackColor;
            this.ComboButtonCancel.ForeColor = theme.Colors.ButtonForeColor;
            this.ComboButtonCancel.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.ComboBoxSelection.BackColor = theme.Colors.TextBoxBackColor;
            this.ComboBoxSelection.ForeColor = theme.Colors.TextBoxForeColor;
            this.DescriptionLabel.ForeColor = theme.Colors.LabelTextColor;
        }

        private void ComboButtonOK_Click(object sender, EventArgs e)
        {
            this.Value = this.ComboBoxSelection.SelectedIndex;
            this.Close();
        }

        private void ComboButtonCancel_Click(object sender, EventArgs e) => this.Close();
    }
}
