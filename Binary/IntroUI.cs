using Binary.Interact;
using Binary.Prompt;
using Binary.Properties;
using Binary.Tools;
using Binary.UI;

using CoreExtensions.Management;

using Endscript.Commands;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;

using Nikki.Core;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using static System.Windows.Forms.AxHost;



namespace Binary
{
    public partial class IntroUI : Form
    {
        public IntroUI()
        {
            this.InitializeComponent();

            this.IntroToolTip.SetToolTip(this.IntroPictureUser, "Launch Binarius for Users");
            this.IntroToolTip.SetToolTip(this.IntroPictureModder, "Launch Binarius for Modders");
            this.IntroToolTip.SetToolTip(this.PictureBoxUpdates, "Check updates for Binarius");
            this.IntroToolTip.SetToolTip(this.PictureBoxTools, "Tools");
            this.IntroToolTip.SetToolTip(this.PictureBoxTheme, "Change theme");

            this.ToggleTheme();
            this.Text = $"Binarius - v{this.ProductVersion}";
        }

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.IntroPanelModder.BackColor = theme.Colors.ButtonBackColor;
            this.IntroPanelUser.BackColor = theme.Colors.ButtonBackColor;
            this.LabelBinary.ForeColor = theme.Colors.LabelTextColor;

            this.LabelUserMode.ForeColor = theme.Colors.MainForeColor;
            this.LabelUserModeDesc.ForeColor = theme.Colors.LabelTextColor;
            this.LabelModderMode.ForeColor = theme.Colors.MainForeColor;
            this.LabelModderModeDesc.ForeColor = theme.Colors.LabelTextColor;

            this.ToolsMenu.BackColor = theme.Colors.MainBackColor;
            this.ToolsMenu.ForeColor = theme.Colors.LabelTextColor;
            this.newLauncherToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.newLauncherToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.hasherToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.hasherToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.raiderToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.raiderToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.swatcherToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.swatcherToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.settingsToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.settingsToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;

            // Set images according to theme
            this.PictureBoxTheme.Image = theme.DarkTheme
                ? Resources.DarkTheme : Resources.LightTheme;

            this.IntroPictureUser.Image = theme.DarkTheme
                ? Resources.DarkUser : Resources.LightUser;

            this.IntroPictureModder.Image = theme.DarkTheme
                ? Resources.DarkModder : Resources.LightModder;

            this.PictureBoxTools.Image = theme.DarkTheme
                ? Resources.DarkTools : Resources.LightTools;

            this.PictureBoxUpdates.Image = Resources.Update;
        }

        #endregion

        private void IntroPictureUser_Click(object sender, EventArgs e)
        {
#if !DEBUG
			try
			{
#endif

            this.UserInteract();
            ForcedX.GCCollect();

#if !DEBUG
			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}
#endif
        }

        private void IntroPictureModder_Click(object sender, EventArgs e)
        {
            // If password check was not done yet (deprecated)
            /*
			if (!Configurations.Default.PassPassed)
			{
				using var form = new ModderPass();
				
				if (form.ShowDialog() != DialogResult.OK)
				{
			
					return;
			
				}
			
			}
			*/

            this.ModderInteract();
            ForcedX.GCCollect();
        }

        private void UserInteract()
        {
            using var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "Binary/ius End Launcher Files|*.end;*.endlauncher|Binary End Launcher Files|*.end|Binarius End Launcher Files|*.endlauncher|All Files|*.*",
                Multiselect = false,
                Title = "Select End Launcher",
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            Launch.Deserialize(dialog.FileName, out Launch launch);

            if (launch.UsageID != eUsage.User)
            {

                throw new Exception($"Usage type of the endscript is stated to be {launch.Usage}, while should be User");

            }

            if (launch.GameID == GameINT.None)
            {

                throw new Exception($"Invalid stated game type named {launch.Game}");

            }

            using var browser = new FolderBrowserDialog()
            {
                Description = $"Select Need for Speed: {launch.Game} directory to modify.",
                RootFolder = Environment.SpecialFolder.MyComputer,
                UseDescriptionForTitle = true,
                AutoUpgradeEnabled = false,
            };

            if (browser.ShowDialog() != DialogResult.OK) return;

            launch.Directory = browser.SelectedPath;
            launch.ThisDir = Path.GetDirectoryName(dialog.FileName);
            launch.CheckEndscript();
            launch.CheckFiles();
            launch.LoadLinks();

            var endscript = Path.Combine(launch.ThisDir, launch.Endscript);
            var parser = new EndScriptParser(endscript);
            BaseCommand[] commands;

            try
            {

                commands = parser.Read();

            }
            catch (Exception ex)
            {

                var error = $"Error has occured -> File: {parser.CurrentFile}, Line: {parser.CurrentIndex}" +
                    Environment.NewLine + $"Command: [{parser.CurrentLine}]" + Environment.NewLine +
                    $"Error: {ex.GetLowestMessage()}";

                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            var profile = BaseProfile.NewProfile(launch.GameID, launch.Directory);
            var exceptions = profile.Load(launch);

            if (exceptions.Length > 0)
            {

                foreach (var exception in exceptions)
                {

                    MessageBox.Show(exception, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                MessageBox.Show($"Unable to execute endscript because of the errors.", "Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            this.EnsureBackups(profile);

            var manager = new EndScriptManager(profile, commands, endscript);

            try
            {

                manager.CommandChase();

                while (!manager.ProcessScript())
                {

                    var command = manager.CurrentCommand;

                    if (command is InfoboxCommand infobox)
                    {

                        using var input = new Info(infobox.Description);
                        input.ShowDialog();

#if DEBUG
                        Console.WriteLine($"Infobox pending");
#endif

                    }
                    else if (command is CheckboxCommand checkbox)
                    {

                        using var input = new Check(checkbox.Description, true);
                        input.ShowDialog();
                        checkbox.Choice = input.Value ? 1 : 0;

#if DEBUG
                        Console.WriteLine($"Checkbox pending : option {input.Value} was chosen by user");
#endif

                    }
                    else if (command is ComboboxCommand combobox)
                    {

                        var options = new string[combobox.Options.Length];
                        for (int i = 0; i < options.Length; ++i) options[i] = combobox.Options[i].Name;
                        using var input = new Combo(options, combobox.Description, true);
                        input.ShowDialog();
                        combobox.Choice = input.Value < 0 ? 0 : input.Value;

#if DEBUG
                        Console.WriteLine($"Checkbox pending : option {input.Value} was chosen by user");
#endif

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Execution has been interrupted", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            var script = Path.GetFileName(dialog.FileName);

            if (manager.Errors.Any())
            {

                Utils.WriteErrorsToLog(manager.Errors, dialog.FileName);
                MessageBox.Show($"Script {script} has been applied, however, {manager.Errors.Count()} errors " +
                    $"has been detected. Check EndError.log for more information", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                MessageBox.Show($"Script {script} has been successfully applied",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            var save = MessageBox.Show("Would you like to save files?", "Prompt",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (save == DialogResult.Yes)
            {

                var errors = profile.Save();

                if (errors.Length > 0)
                {

                    foreach (var error in errors)
                    {

                        MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
                else
                {

                    this.AskForGameRun(profile);

                }

            }
        }

        private void ModderInteract()
        {
            this.Hide();
            var start = FormStartPosition.CenterScreen;
            var state = Configurations.Default.StartMaximized
                ? FormWindowState.Maximized
                : FormWindowState.Normal;

            using (var editor = new Editor() { StartPosition = start, WindowState = state })
            {

                editor.ShowDialog();

            }

            this.Show();
            this.ToggleTheme();
        }

        private void EnsureBackups(BaseProfile profile)
        {
            foreach (var sdb in profile)
            {

                var orig = sdb.FullPath;
                var back = $"{orig}.bacc";
                if (!File.Exists(back)) File.Copy(orig, back, true);

            }
        }

        private void AskForGameRun(BaseProfile profile)
        {
            var result = MessageBox.Show("Do you wish to run the game?", "Prompt",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                try
                {

                    var path = profile.Directory;
                    var game = profile.GameINT;
                    var exe = path;

                    exe += game switch
                    {
                        GameINT.Carbon => @"\NFSC.EXE",
                        GameINT.MostWanted => @"\SPEED.EXE",
                        GameINT.Prostreet => @"\NFS.EXE",
                        GameINT.Undercover => @"\NFS.EXE",
                        GameINT.Underground1 => @"\SPEED.EXE",
                        GameINT.Underground2 => @"\SPEED2.EXE",
                        _ => throw new Exception($"Game {game} is an invalid game type")
                    };

                    Process.Start(new ProcessStartInfo(exe) { WorkingDirectory = path });

                }
                catch (Exception e)
                {

                    MessageBox.Show(e.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }

        private void PictureBoxTools_Click(object sender, EventArgs e)
        {
            ToolsMenu.Show(PictureBoxTools.Left + this.Left, PictureBoxTools.Top + this.Top); // offset from the edge of your button
        }

        private void PictureBoxUpdates_Click(object sender, EventArgs e)
        {
            Utils.OpenBrowser("https://github.com/nlgxzef/Binarius/releases");
        }

        private void PictureBoxAutoBackups_Click(object sender, EventArgs e)
        {
            Configurations.Default.AutoBackups = !Configurations.Default.AutoBackups;
            Configurations.Default.Save();
            this.ToggleTheme();
        }

        private void PictureBoxMaximized_Click(object sender, EventArgs e)
        {
            Configurations.Default.StartMaximized = !Configurations.Default.StartMaximized;
            Configurations.Default.Save();
            this.ToggleTheme();
        }

        private void PictureBoxTheme_Click(object sender, EventArgs e)
        {
            var themes = new ThemeSelector() { StartPosition = FormStartPosition.CenterScreen };
            themes.ShowDialog();

            if (themes.DialogResult == DialogResult.OK)
            {
                this.ToggleTheme();
            }
        }

        private void LabelBinary_Click(object sender, EventArgs e)
        {
            var about = new About() { StartPosition = FormStartPosition.CenterScreen };
            about.Show();
        }

        private void newLauncherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new LanMaker() { StartPosition = FormStartPosition.CenterScreen };
            form.Show();
        }

        private void hasherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var hasher = new Hasher() { StartPosition = FormStartPosition.CenterScreen };
            hasher.Show();
        }

        private void raiderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var raider = new Raider() { StartPosition = FormStartPosition.CenterScreen };
            raider.Show();
        }

        private void swatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var swatcher = new Swatcher() { StartPosition = FormStartPosition.CenterScreen };
            swatcher.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = new Interact.Options() { StartPosition = FormStartPosition.CenterScreen };
            settings.Show();
        }
    }
}
