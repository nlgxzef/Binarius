using Binary.Properties;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Prompt
{
    public partial class Input : Form
    {
        private const string invalid = "Invalid input";
        private const string input = "Input value";
        private readonly Predicate<string> _input_check;
        private readonly string _error_message;
        public string Value { get; private set; } = String.Empty;

        public Input() : this(input, null, invalid, null) { }

        public Input(string text) : this(text, null, invalid, null) { }

        public Input(string text, Predicate<string> inputcheck) : this(text, inputcheck, invalid, null) { }

        public Input(string text, Predicate<string> inputcheck, string error) : this(text, inputcheck, error, null) { }

        public Input(string text, Predicate<string> inputcheck, string error, string initial)
        {
            this.InitializeComponent();
            this.ToggleTheme();
            this.InputLabel.Text = text;
            this._input_check = inputcheck;
            this._error_message = error;
            this.InputTextBox.Text = initial ?? String.Empty;
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.InputButtonOK.BackColor = theme.Colors.ButtonBackColor;
            this.InputButtonOK.ForeColor = theme.Colors.ButtonForeColor;
            this.InputButtonOK.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.InputButtonCancel.BackColor = theme.Colors.ButtonBackColor;
            this.InputButtonCancel.ForeColor = theme.Colors.ButtonForeColor;
            this.InputButtonCancel.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.InputTextBox.BackColor = theme.Colors.TextBoxBackColor;
            this.InputTextBox.ForeColor = theme.Colors.TextBoxForeColor;
            this.InputLabel.ForeColor = theme.Colors.LabelTextColor;
        }

        private void InputButtonOK_Click(object sender, EventArgs e)
        {
            if (!this._input_check?.Invoke(this.InputTextBox.Text) ?? false)
            {

                _ = MessageBox.Show(this._error_message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            this.Value = this.InputTextBox.Text;
            this.Close();
        }

        private void InputButtonCancel_Click(object sender, EventArgs e) => this.Close();
    }
}
