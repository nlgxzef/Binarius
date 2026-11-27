using Binary.Properties;

using System;
using System.Drawing;
using System.Windows.Forms;



namespace Binary.Prompt
{
    /// <summary>
    /// Themed question dialog with a message, an optional checkbox and any number of buttons.
    /// <see cref="Value"/> is the index of the clicked button, or -1 if the dialog was closed.
    /// </summary>
    public class Choice : Form
    {
        private readonly Label _label;
        private readonly CheckBox _check;
        private readonly Button[] _buttons;

        public int Value { get; private set; } = -1;
        public bool Checked => this._check?.Checked ?? false;

        /// <param name="title">Window title.</param>
        /// <param name="text">Message shown to the user.</param>
        /// <param name="buttons">Button captions, left to right.</param>
        /// <param name="defaultIndex">Button activated by Enter.</param>
        /// <param name="cancelIndex">Button activated by Esc / closing the window; -1 for none.</param>
        /// <param name="checkText">Optional checkbox caption; null for no checkbox.</param>
        /// <param name="isChecked">Initial checkbox state.</param>
        public Choice(string title, string text, string[] buttons, int defaultIndex = 0, int cancelIndex = -1,
            string checkText = null, bool isChecked = false)
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = title;
            this.Icon = (Icon)new System.ComponentModel.ComponentResourceManager(typeof(Info)).GetObject("$this.Icon");
            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;

            this._label = new Label() { Text = text ?? String.Empty, ForeColor = theme.Colors.LabelTextColor };
            this.Controls.Add(this._label);

            if (checkText != null)
            {
                this._check = new CheckBox()
                {
                    Text = checkText,
                    Checked = isChecked,
                    ForeColor = theme.Colors.LabelTextColor,
                };
                this.Controls.Add(this._check);
            }

            this._buttons = new Button[buttons.Length];

            for (int i = 0; i < buttons.Length; ++i)
            {
                var index = i;
                var button = new Button()
                {
                    Text = buttons[i],
                    AutoSize = true,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = theme.Colors.ButtonBackColor,
                    ForeColor = theme.Colors.ButtonForeColor,
                    MinimumSize = new Size(this.LogicalToDeviceUnits(94), this.LogicalToDeviceUnits(27)),
                    UseVisualStyleBackColor = false,
                };
                button.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
                button.Click += (s, e) =>
                {
                    this.Value = index;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
                this._buttons[i] = button;
                this.Controls.Add(button);
            }

            if (defaultIndex >= 0 && defaultIndex < this._buttons.Length) this.AcceptButton = this._buttons[defaultIndex];
            if (cancelIndex >= 0 && cancelIndex < this._buttons.Length) this.CancelButton = this._buttons[cancelIndex];
            this.ResumeLayout(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            var content = this._check is null
                ? new Control[] { this._label }
                : new Control[] { this._label, this._check };

            PromptLayout.Arrange(this, null, content, this._buttons);
            base.OnLoad(e);

            var focus = this.AcceptButton as Button;
            focus?.Select();
        }
    }
}
