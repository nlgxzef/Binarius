using Binary.Properties;

using CoreExtensions.Native;
using CoreExtensions.Text;

using Nikki.Support.Shared.Parts.STRParts;
using Nikki.Utils;

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;



namespace Binary.Interact
{
    public partial class StringCreator : Form
    {
        public string Key { get; private set; }
        public string Label { get; private set; }
        public string Value { get; private set; }

        public StringCreator()
        {
            this.InitializeComponent();
            this.ToggleTheme();
            this.StringTextBoxKey.Text = "0x00000000";
        }

        public StringCreator(StringRecord record) : this()
        {
            this.StringTextBoxKey.Text = $"0x{record.Key:X8}";
            this.StringTextBoxLabel.Text = record.Label;
            this.StringTextBoxText.Text = record.Text;

            if (record.Key != record.Label.BinHash())
            {

                this.StringCheckBoxCustom.Checked = true;

            }
        }

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.StringButtonOK.BackColor = theme.Colors.ButtonBackColor;
            this.StringButtonOK.ForeColor = theme.Colors.ButtonForeColor;
            this.StringButtonOK.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.StringButtonCancel.BackColor = theme.Colors.ButtonBackColor;
            this.StringButtonCancel.ForeColor = theme.Colors.ButtonForeColor;
            this.StringButtonCancel.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.StringTextBoxKey.BackColor = theme.Colors.TextBoxBackColor;
            this.StringTextBoxKey.ForeColor = theme.Colors.TextBoxForeColor;
            this.StringTextBoxLabel.BackColor = theme.Colors.TextBoxBackColor;
            this.StringTextBoxLabel.ForeColor = theme.Colors.TextBoxForeColor;
            this.StringTextBoxText.BackColor = theme.Colors.TextBoxBackColor;
            this.StringTextBoxText.ForeColor = theme.Colors.TextBoxForeColor;
            this.StringCheckBoxCustom.ForeColor = theme.Colors.LabelTextColor;
            this.StringCheckBoxReversed.ForeColor = theme.Colors.LabelTextColor;
            this.label1.ForeColor = theme.Colors.LabelTextColor;
            this.label2.ForeColor = theme.Colors.LabelTextColor;
        }

        #endregion

        private void StringButtonOK_Click(object sender, EventArgs e)
        {
            if (this.StringCheckBoxReversed.Checked)
            {

                var key = Convert.ToUInt32(this.StringTextBoxKey.Text, 16);
                this.Key = $"0x{key.Reverse():X8}";

            }
            else
            {

                this.Key = this.StringTextBoxKey.Text;

            }

            this.Label = this.StringTextBoxLabel.Text ?? String.Empty;
            this.Value = this.StringTextBoxText.Text ?? String.Empty;
            this.Close();
        }

        private void StringButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StringTextBoxLabel_TextChanged(object sender, EventArgs e)
        {
            if (!this.StringCheckBoxCustom.Checked)
            {

                var key = this.StringTextBoxLabel.Text.BinHash();
                this.StringTextBoxKey.Text = this.StringCheckBoxReversed.Checked
                    ? $"0x{key.Reverse():X8}"
                    : $"0x{key:X8}";

            }
        }

        private void StringCheckBoxCustom_CheckedChanged(object sender, EventArgs e)
        {
            this.StringTextBoxKey.Enabled = this.StringCheckBoxCustom.Checked;

            if (!this.StringTextBoxKey.Enabled)
            {

                this.StringTextBoxKey.Text = $"0x{this.StringTextBoxLabel.Text.BinHash():X8}";

            }
        }

        private void StringCheckBoxReversed_CheckedChanged(object sender, EventArgs e)
        {
            var key = Convert.ToUInt32(this.StringTextBoxKey.Text, 16);
            this.StringTextBoxKey.Text = $"0x{key.Reverse():X8}";
        }

        private void StringTextBoxKey_Validating(object sender, CancelEventArgs e)
        {
            if (!this.StringTextBoxKey.Text.IsHexString())
            {

                MessageBox.Show("Key entered should be a hexadecimal hash that starts with 0x", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;

            }
        }
    }
}
