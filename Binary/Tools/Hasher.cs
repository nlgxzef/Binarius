using Binary.Properties;

using CoreExtensions.Native;

using Nikki.Utils;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Tools
{
    public partial class Hasher : Form
    {
        public Hasher()
        {
            this.InitializeComponent();
            this.ToggleTheme();
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.StringTextbox.BackColor = theme.Colors.TextBoxBackColor;
            this.StringTextbox.ForeColor = theme.Colors.TextBoxForeColor;
            this.BinHashTextbox.BackColor = theme.Colors.TextBoxBackColor;
            this.BinHashTextbox.ForeColor = theme.Colors.TextBoxForeColor;
            this.BinFileTextbox.BackColor = theme.Colors.TextBoxBackColor;
            this.BinFileTextbox.ForeColor = theme.Colors.TextBoxForeColor;
            this.VltHashTextbox.BackColor = theme.Colors.TextBoxBackColor;
            this.VltHashTextbox.ForeColor = theme.Colors.TextBoxForeColor;
            this.VltFileTextbox.BackColor = theme.Colors.TextBoxBackColor;
            this.VltFileTextbox.ForeColor = theme.Colors.TextBoxForeColor;
            this.CopyString.BackColor = theme.Colors.ButtonBackColor;
            this.CopyString.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyString.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CopyBinHash.BackColor = theme.Colors.ButtonBackColor;
            this.CopyBinHash.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyBinHash.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CopyBinFile.BackColor = theme.Colors.ButtonBackColor;
            this.CopyBinFile.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyBinFile.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CopyVltHash.BackColor = theme.Colors.ButtonBackColor;
            this.CopyVltHash.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyVltHash.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.CopyVltFile.BackColor = theme.Colors.ButtonBackColor;
            this.CopyVltFile.ForeColor = theme.Colors.ButtonForeColor;
            this.CopyVltFile.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.label1.ForeColor = theme.Colors.LabelTextColor;
            this.label2.ForeColor = theme.Colors.LabelTextColor;
            this.label3.ForeColor = theme.Colors.LabelTextColor;
            this.label4.ForeColor = theme.Colors.LabelTextColor;
            this.label5.ForeColor = theme.Colors.LabelTextColor;
        }

        private void StringTextbox_TextChanged(object sender, EventArgs e)
        {
            string str = this.StringTextbox.Text;
            string _0x = "0x";
            bool state = Hashing.PauseHashSave;
            Hashing.PauseHashSave = true;

            // Bin memory hash
            uint result = str.BinHash();
            this.BinHashTextbox.Text = $"{_0x}{result:X8}";

            // Bin file hash
            result = result.Reverse();
            this.BinFileTextbox.Text = $"{_0x}{result:X8}";

            // Vlt memory hash
            result = str.VltHash();
            this.VltHashTextbox.Text = $"{_0x}{result:X8}";

            // Vlt file hash
            result = result.Reverse();
            this.VltFileTextbox.Text = $"{_0x}{result:X8}";

            Hashing.PauseHashSave = state;
        }

        private void CopyString_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.StringTextbox.Text))
            {

                Clipboard.SetText(this.StringTextbox.Text);

            }
        }

        private void CopyBinHash_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.BinHashTextbox.Text))
            {

                Clipboard.SetText(this.BinHashTextbox.Text);

            }
        }

        private void CopyBinFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.BinFileTextbox.Text))
            {

                Clipboard.SetText(this.BinFileTextbox.Text);

            }
        }

        private void CopyVltHash_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.VltHashTextbox.Text))
            {

                Clipboard.SetText(this.VltHashTextbox.Text);

            }
        }

        private void CopyVltFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.VltFileTextbox.Text))
            {

                Clipboard.SetText(this.VltFileTextbox.Text);

            }
        }
    }
}
