using Binary.Properties;

using CoreExtensions.Native;
using CoreExtensions.Text;

using Nikki.Core;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Tools
{
    public partial class Raider : Form
    {
        public Raider()
        {
            this.InitializeComponent();
            this.ToggleTheme();
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.ChooseSearchMode.BackColor = theme.Colors.TextBoxBackColor;
            this.ChooseSearchMode.ForeColor = theme.Colors.TextBoxForeColor;
            this.BinHashInput.BackColor = theme.Colors.TextBoxBackColor;
            this.BinHashInput.ForeColor = theme.Colors.TextBoxForeColor;
            this.BinFileInput.BackColor = theme.Colors.TextBoxBackColor;
            this.BinFileInput.ForeColor = theme.Colors.TextBoxForeColor;
            this.StringGuessed.BackColor = theme.Colors.TextBoxBackColor;
            this.StringGuessed.ForeColor = theme.Colors.TextBoxForeColor;
            this.CopyBinHash.BackColor = theme.Colors.ButtonBackColor;
            this.CopyBinHash.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyBinHash.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CopyBinFile.BackColor = theme.Colors.ButtonBackColor;
            this.CopyBinFile.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyBinFile.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CopyString.BackColor = theme.Colors.ButtonBackColor;
            this.CopyString.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyString.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.label1.ForeColor = theme.Colors.LabelTextColor;
            this.label2.ForeColor = theme.Colors.LabelTextColor;
            this.label3.ForeColor = theme.Colors.LabelTextColor;
            this.label4.ForeColor = theme.Colors.LabelTextColor;
        }

        private void ChooseSearchMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ChooseSearchMode.SelectedIndex == 0)
            {
                this.BinHashInput.ReadOnly = false;
                this.BinFileInput.ReadOnly = true;
            }
            else
            {
                this.BinHashInput.ReadOnly = true;
                this.BinFileInput.ReadOnly = false;
            }
        }

        private void BinHashInput_TextChanged(object sender, EventArgs e)
        {
            string value = this.BinHashInput.Text.StartsWith("0x")
                ? this.BinHashInput.Text
                : $"0x{this.BinHashInput.Text}";

            if (!this.BinHashInput.ReadOnly && value.IsHexString())
            {

                if (value.Length > 10) { this.StringGuessed.Text = "N/A"; return; }

                uint key = Convert.ToUInt32(this.BinHashInput.Text, 16);
                this.BinFileInput.Text = $"0x{key.Reverse():X8}";

                this.StringGuessed.Text = Map.BinKeys.TryGetValue(key, out string result)
                    ? result
                    : "N/A";

            }
        }

        private void BinFileInput_TextChanged(object sender, EventArgs e)
        {
            string value = this.BinFileInput.Text.StartsWith("0x")
                ? this.BinFileInput.Text
                : $"0x{this.BinFileInput.Text}";

            if (!this.BinFileInput.ReadOnly && value.IsHexString())
            {

                if (value.Length > 10) { this.StringGuessed.Text = "N/A"; return; }

                uint key = Convert.ToUInt32(this.BinFileInput.Text, 16);
                key = key.Reverse();
                this.BinHashInput.Text = $"0x{key:X8}";

                this.StringGuessed.Text = Map.BinKeys.TryGetValue(key, out string result)
                    ? result
                    : "N/A";
            }
        }

        private void CopyBinHash_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.BinHashInput.Text))
            {

                Clipboard.SetText(this.BinHashInput.Text);

            }
        }

        private void CopyBinFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.BinFileInput.Text))
            {

                Clipboard.SetText(this.BinFileInput.Text);

            }
        }

        private void CopyString_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.StringGuessed.Text))
            {

                Clipboard.SetText(this.StringGuessed.Text);

            }
        }
    }
}
