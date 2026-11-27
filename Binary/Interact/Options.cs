using Binary.Properties;

using Endscript.Core;
using Endscript.Enums;

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
            Binary.UI.DialogLayout.Attach(this); // re-flow for the current DPI before showing
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
            this.OptionsCheckSafeSave.ForeColor = theme.Colors.MainForeColor;
            this.OptionsLabelTemp.ForeColor = theme.Colors.MainForeColor;
            this.OptionsCheckKeepTemp.ForeColor = theme.Colors.MainForeColor;
            this.OptionsComboTemp.BackColor = theme.Colors.TextBoxBackColor;
            this.OptionsComboTemp.ForeColor = theme.Colors.TextBoxForeColor;
            this.OptionsTextTempPath.BackColor = theme.Colors.TextBoxBackColor;
            this.OptionsTextTempPath.ForeColor = theme.Colors.TextBoxForeColor;
            this.OptionsButtonTempBrowse.BackColor = theme.Colors.ButtonBackColor;
            this.OptionsButtonTempBrowse.ForeColor = theme.Colors.ButtonForeColor;
            this.OptionsButtonTempBrowse.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.OptionsCheckDisableAdminWarning.ForeColor = theme.Colors.MainForeColor;
            this.OptionsCheckHideEmptyManagers.ForeColor = theme.Colors.MainForeColor;

        }

        private void LoadOptions()
        {
            this.OptionsTextWatermark.Text = Configurations.Default.Watermark;

            this.OptionsCheckAutoBackups.Checked = Configurations.Default.AutoBackups;
            this.OptionsCheckStartMaximized.Checked = Configurations.Default.StartMaximized;
            this.OptionsCheckSafeSave.Checked = Configurations.Default.SafeSave;
            this.OptionsTextTempPath.Text = Configurations.Default.TempCustomPath;
            this.OptionsCheckKeepTemp.Checked = Configurations.Default.KeepTempFiles;
            this.OptionsComboTemp.SelectedIndex =
                Enum.TryParse(Configurations.Default.TempLocation, out eTempLocation location) && Enum.IsDefined(location)
                    ? (int)location
                    : (int)eTempLocation.Game;
            this.UpdateTempControls();
            this.OptionsCheckDisableAdminWarning.Checked = Configurations.Default.DisableAdminWarning;
            this.OptionsCheckHideEmptyManagers.Checked = Configurations.Default.HideEmptyManagers;

        }

        private void SaveOptions()
        {
            Configurations.Default.Watermark = this.OptionsTextWatermark.Text;

            Configurations.Default.AutoBackups = this.OptionsCheckAutoBackups.Checked;
            Configurations.Default.StartMaximized = this.OptionsCheckStartMaximized.Checked;
            Configurations.Default.SafeSave = this.OptionsCheckSafeSave.Checked;
            Configurations.Default.TempLocation = this.SelectedTempLocation.ToString();
            Configurations.Default.TempCustomPath = this.OptionsTextTempPath.Text.Trim();
            Configurations.Default.KeepTempFiles = this.OptionsCheckKeepTemp.Checked;
            Configurations.Default.DisableAdminWarning = this.OptionsCheckDisableAdminWarning.Checked;
            Configurations.Default.HideEmptyManagers = this.OptionsCheckHideEmptyManagers.Checked;

            Configurations.Default.Save();
            Utils.ApplySaveSettings();
        }

        private eTempLocation SelectedTempLocation => (eTempLocation)Math.Max(0, this.OptionsComboTemp.SelectedIndex);

        private void UpdateTempControls()
        {
            bool enabled = this.OptionsCheckSafeSave.Checked;
            bool custom = enabled && this.SelectedTempLocation == eTempLocation.Custom;

            this.OptionsComboTemp.Enabled = enabled;
            this.OptionsCheckKeepTemp.Enabled = enabled;
            this.OptionsTextTempPath.Enabled = custom;
            this.OptionsButtonTempBrowse.Enabled = custom;
        }

        private void OptionsTempSetting_Changed(object sender, EventArgs e) => this.UpdateTempControls();

        private void OptionsButtonTempBrowse_Click(object sender, EventArgs e)
        {
            using var browser = new FolderBrowserDialog()
            {
                Description = "Select the folder for temporary files.",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true,
            };

            var current = this.OptionsTextTempPath.Text.Trim();
            if (Directory.Exists(current)) browser.SelectedPath = current;

            if (browser.ShowDialog(this) == DialogResult.OK) this.OptionsTextTempPath.Text = browser.SelectedPath;
        }

        // Better to find out about a bad folder here than on the next save
        private string CheckTempFolder()
        {
            if (!this.OptionsCheckSafeSave.Checked || this.SelectedTempLocation != eTempLocation.Custom) return null;

            var path = this.OptionsTextTempPath.Text.Trim();

            if (path.Length == 0 || !Path.IsPathFullyQualified(path))
                return "Enter the full path of the custom temporary folder, or choose it with Browse.";

            try
            {
                Directory.CreateDirectory(path);
                var test = Path.Combine(path, $"binarius-{Guid.NewGuid():N}.test");
                File.Create(test, 1, FileOptions.DeleteOnClose).Dispose();
                return null;
            }
            catch (Exception ex)
            {
                return $"The custom temporary folder cannot be written to: {ex.Message}";
            }
        }

        private void OptionsButtonOK_Click(object sender, EventArgs e)
        {
            var problem = this.CheckTempFolder();

            if (problem != null)
            {
                MessageBox.Show(problem, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
