using Binary.Properties;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Interact
{
    public partial class Exporter : Form
    {
        public bool Serialized { get; private set; } = true;

        public Exporter() : this(true) { }

        public Exporter(bool allow_not_serialized)
        {
            this.InitializeComponent();
            this.ToggleTheme();

            string tip =
                "If enabled, collection is exported with full amount of information " +
                "about it, compressed and protected from user changing it. If disabled, collection " +
                "is exported plainly as it is used in the game.";
            this.ExporterToolTip.SetToolTip(this.ExportSerialized, tip);

            if (!allow_not_serialized)
            {
                this.ExportSerialized.Enabled = false;
            }
            else
            {
                this.Serialized = false;
            }
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.ExportSerialized.BackColor = theme.Colors.MainBackColor;
            this.ExportSerialized.ForeColor = theme.Colors.LabelTextColor;
            this.ExporterButton.BackColor = theme.Colors.ButtonBackColor;
            this.ExporterButton.ForeColor = theme.Colors.ButtonForeColor;
            this.ExporterButton.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
        }

        private void ExporterButton_Click(object sender, EventArgs e)
        {
            this.Serialized = this.ExportSerialized.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
