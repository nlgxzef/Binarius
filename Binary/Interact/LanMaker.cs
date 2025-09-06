using Binary.Properties;

using Endscript.Core;

using Nikki.Reflection.Abstract;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using static System.Net.Mime.MediaTypeNames;



namespace Binary.Interact
{
    public partial class LanMaker : Form
    {
        private bool IsValidDirectoryChosen => Directory.Exists(this.LanMakerTextBoxDir.Text);
        public bool WasCreated { get; private set; }
        public string NewLanPath { get; private set; }
        public Launch NewLan { get; private set; }

        public LanMaker()
        {
            this.InitializeComponent();
            this.ToggleTheme();

            this.NewLan = Utils.GenerateSample();
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.LanMakerGame.BackColor = theme.Colors.TextBoxBackColor;
            this.LanMakerGame.ForeColor = theme.Colors.TextBoxForeColor;
            this.LanMakerUsage.BackColor = theme.Colors.TextBoxBackColor;
            this.LanMakerUsage.ForeColor = theme.Colors.TextBoxForeColor;
            this.LanMakerTextBoxDir.BackColor = theme.Colors.TextBoxBackColor;
            this.LanMakerTextBoxDir.ForeColor = theme.Colors.TextBoxForeColor;
            this.LanMakerTextBoxEndScriptFile.BackColor = theme.Colors.TextBoxBackColor;
            this.LanMakerTextBoxEndScriptFile.ForeColor = theme.Colors.TextBoxForeColor;
            this.LanMakerButtonDir.BackColor = theme.Colors.ButtonBackColor;
            this.LanMakerButtonDir.ForeColor = theme.Colors.ButtonForeColor;
            this.LanMakerButtonDir.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.LanMakerButtonSave.BackColor = theme.Colors.ButtonBackColor;
            this.LanMakerButtonSave.ForeColor = theme.Colors.ButtonForeColor;
            this.LanMakerButtonSave.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.LanMakerButtonHelp.BackColor = theme.Colors.ButtonBackColor;
            this.LanMakerButtonHelp.ForeColor = theme.Colors.ButtonForeColor;
            this.LanMakerButtonHelp.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.LanMakerButtonSelectFiles.BackColor = theme.Colors.ButtonBackColor;
            this.LanMakerButtonSelectFiles.ForeColor = theme.Colors.ButtonForeColor;
            this.LanMakerButtonSelectFiles.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.LanMakerButtonSelectLauncher.BackColor = theme.Colors.ButtonBackColor;
            this.LanMakerButtonSelectLauncher.ForeColor = theme.Colors.ButtonForeColor;
            this.LanMakerButtonSelectLauncher.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
        }

        private void ChangeSaveButtonEnabledState()
        {
            this.LanMakerButtonSave.Enabled = this.IsValidDirectoryChosen &&
                !String.IsNullOrEmpty(this.LanMakerGame.Text) &&
                !String.IsNullOrEmpty(this.LanMakerUsage.Text);
            this.LanMakerButtonSelectFiles.Enabled = this.IsValidDirectoryChosen;

        }

        private void LanMakerButtonDir_Click(object sender, EventArgs e)
        {
            using var browser = new FolderBrowserDialog()
            {
                AutoUpgradeEnabled = false,
                Description = "Select any Need for Speed game directory",
                RootFolder = Environment.SpecialFolder.MyComputer,
            };

            if (browser.ShowDialog() == DialogResult.OK)
            {

                this.LanMakerTextBoxDir.Text = browser.SelectedPath;
                this.LanMakerButtonSave.Enabled = true;
                this.LanMakerButtonSelectFiles.Enabled = true;
            }

            this.ChangeSaveButtonEnabledState();
        }

        private void LanMakerButtonSave_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                DefaultExt = ".endlauncher",
                Filter = "Binary End Launcher Files|*.end|Binarius End Launcher Files|*.endlauncher",
                OverwritePrompt = true,
                SupportMultiDottedExtensions = true,
                Title = "Save End Launcher",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                this.NewLan.Directory = this.LanMakerTextBoxDir.Text;
                this.NewLan.Game = this.LanMakerGame.Text;
                this.NewLan.Usage = this.LanMakerUsage.Text;

                if (this.LanMakerUsage.Text == "User")

                {
                    this.NewLan.Endscript = this.LanMakerTextBoxEndScriptFile.Text;
                    using var sw = new StreamWriter(File.Open(Path.Combine(Path.GetDirectoryName(dialog.FileName), this.NewLan.Endscript), FileMode.Create));

                    var ext = Path.GetExtension(this.NewLan.Endscript).ToLower();

                    if (ext != ".endscript") // .endscript is not required to have versioning
                    {
                        sw.WriteLine("[VERSN2]");
                    }
                    sw.WriteLine();
                }
                else
                {
                    this.NewLan.Endscript = String.Empty;
                }

                Launch.Serialize(dialog.FileName, this.NewLan);
                MessageBox.Show($"File {dialog.FileName} has been saved.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.WasCreated = true;
                this.NewLanPath = dialog.FileName;

            }
        }

        private void LanMakerUsage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ChangeSaveButtonEnabledState();

            if (this.LanMakerUsage.Text == "User")
            {
                this.LanMakerTextBoxEndScriptFile.Text = "install.endscript";
                this.LanMakerTextBoxEndScriptFile.Enabled = true;
            }
            else
            {
                this.LanMakerTextBoxEndScriptFile.Text = "";
                this.LanMakerTextBoxEndScriptFile.Enabled = false;
            }
        }

        private void LanMakerGame_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ChangeSaveButtonEnabledState();
        }

        private void LanMakerButtonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Launcher is a Version 1 .end(launcher) file that is used for loading game files, " +
                "linked files, main endscript and game selection.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LanMakerTextBoxDir_TextChanged(object sender, EventArgs e)
        {
            this.ChangeSaveButtonEnabledState();
        }

        private void LanMakerButtonSelectFiles_Click(object sender, EventArgs e)
        {
            var sel = new LanMakerFileSelector(this.LanMakerTextBoxDir.Text) { StartPosition = FormStartPosition.CenterScreen };
            sel.Files = NewLan.Files;
            sel.ShowDialog();

            if (sel.DialogResult == DialogResult.OK)
            {
                NewLan.Files = sel.Files;
            }
        }

        private void LanMakerButtonSelectLauncher_Click(object sender, EventArgs e)
        {
#if !DEBUG
			try
			{
#endif
            using var dialog = new OpenFileDialog()
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                DefaultExt = ".endlauncher",
                Filter = "Binary End Launcher Files|*.end|Binarius End Launcher Files|*.endlauncher",
                SupportMultiDottedExtensions = true,
                Title = "Open End Launcher",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Launch.Deserialize(dialog.FileName, out var launch);
                this.NewLan = launch;

                this.LanMakerTextBoxDir.Text = launch.Directory;

                int index = this.LanMakerGame.FindString(launch.Game);
                this.LanMakerGame.SelectedIndex = index != -1 ? index : 0;

                index = this.LanMakerUsage.FindString(launch.Usage);
                this.LanMakerUsage.SelectedIndex = index != -1 ? index : 0;

                if (this.LanMakerUsage.SelectedIndex == 0) // User
                {
                    this.LanMakerTextBoxEndScriptFile.Text = launch.Endscript;
                }
                else
                {
                    this.LanMakerTextBoxEndScriptFile.Text = string.Empty;
                }

                this.LanMakerButtonSave.Enabled = true;
                this.LanMakerButtonSelectFiles.Enabled = true;
            }

#if !DEBUG
            }
			catch (Exception ex)
			{
			
				MessageBox.Show("Unable to load End Launcher file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			
				this.ChangeSaveButtonEnabledState();
			
			}
#endif
        }
    }
}
