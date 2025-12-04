using Binary.Properties;

using Endscript.Core;

using Nikki.Core;
using Nikki.Reflection.Abstract;

using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Binary.Interact
{
    public partial class Options : Form
    {
        public Options()
        {
            this.InitializeComponent();
            this.ToggleTheme();
            this.LoadOptions();
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.OptionsLabelWatermark.ForeColor = theme.Colors.MainForeColor;
            this.OptionsTextWatermark.BackColor = theme.Colors.TextBoxBackColor;
            this.OptionsTextWatermark.ForeColor = theme.Colors.TextBoxForeColor;
            this.OptionsButtonOK.BackColor = theme.Colors.ButtonBackColor;
            this.OptionsButtonOK.ForeColor = theme.Colors.ButtonForeColor;
            this.OptionsButtonOK.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.OptionsButtonCancel.BackColor = theme.Colors.ButtonBackColor;
            this.OptionsButtonCancel.ForeColor = theme.Colors.ButtonForeColor;
            this.OptionsButtonCancel.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.OptionsCheckAutoBackups.ForeColor = theme.Colors.MainForeColor;
            this.OptionsCheckStartMaximized.ForeColor = theme.Colors.MainForeColor;
            this.OptionsCheckSoonFeature.ForeColor = theme.Colors.MainForeColor;
            this.OptionsCheckDisableAdminWarning.ForeColor = theme.Colors.MainForeColor;
            this.OptionsCheckHideEmptyManagers.ForeColor = theme.Colors.MainForeColor;

        }

        private void LoadOptions()
        {
            this.OptionsTextWatermark.Text = Configurations.Default.Watermark;

            this.OptionsCheckAutoBackups.Checked = Configurations.Default.AutoBackups;
            this.OptionsCheckStartMaximized.Checked = Configurations.Default.StartMaximized;
            this.OptionsCheckSoonFeature.Checked = Configurations.Default.SoonFeature;
            this.OptionsCheckDisableAdminWarning.Checked = Configurations.Default.DisableAdminWarning;
            this.OptionsCheckHideEmptyManagers.Checked = Configurations.Default.HideEmptyManagers;

        }

        private void SaveOptions()
        {
            Configurations.Default.Watermark = this.OptionsTextWatermark.Text;

            Configurations.Default.AutoBackups = this.OptionsCheckAutoBackups.Checked;
            Configurations.Default.StartMaximized = this.OptionsCheckStartMaximized.Checked;
            Configurations.Default.SoonFeature = this.OptionsCheckSoonFeature.Checked;
            Configurations.Default.DisableAdminWarning = this.OptionsCheckDisableAdminWarning.Checked;
            Configurations.Default.HideEmptyManagers = this.OptionsCheckHideEmptyManagers.Checked;

            Configurations.Default.Save();
        }

        private void OptionsButtonOK_Click(object sender, EventArgs e)
        {
            SaveOptions();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OptionsButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
