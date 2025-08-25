using Binary.Properties;

using Endscript.Core;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Interact
{
	public partial class LanMaker : Form
	{
		private bool IsValidDirectoryChosen => Directory.Exists(this.LanMakerTextBoxDir.Text);
		public bool WasCreated { get; private set; }
		public string NewLanPath { get; private set; }

		public LanMaker()
		{
			this.InitializeComponent();
			this.ToggleTheme();
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
			this.LanMakerButtonDir.BackColor = theme.Colors.ButtonBackColor;
			this.LanMakerButtonDir.ForeColor = theme.Colors.ButtonForeColor;
			this.LanMakerButtonDir.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
			this.LanMakerButtonSave.BackColor = theme.Colors.ButtonBackColor;
			this.LanMakerButtonSave.ForeColor = theme.Colors.ButtonForeColor;
			this.LanMakerButtonSave.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
			this.LanMakerButtonHelp.BackColor = theme.Colors.ButtonBackColor;
			this.LanMakerButtonHelp.ForeColor = theme.Colors.ButtonForeColor;
			this.LanMakerButtonHelp.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
		}

		private void ChangeSaveButtonEnabledState()
		{
			this.LanMakerButtonSave.Enabled = this.IsValidDirectoryChosen &&
				!String.IsNullOrEmpty(this.LanMakerGame.Text) &&
				!String.IsNullOrEmpty(this.LanMakerUsage.Text);
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
				DefaultExt = ".end",
				Filter = "Endscript Files|*.end",
				OverwritePrompt = true,
				SupportMultiDottedExtensions = true,
				Title = "Select directory and filename of the endscript to be saved",
			};

			if (dialog.ShowDialog() == DialogResult.OK)
			{

				var directory = this.LanMakerTextBoxDir.Text;
				var game = this.LanMakerGame.Text;
				var usage = this.LanMakerUsage.Text;
				var launch = Utils.GenerateSample(directory, game, usage);
				Launch.Serialize(dialog.FileName, launch);
				MessageBox.Show($"File {dialog.FileName} has been saved.", "Success",
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				this.WasCreated = true;
				this.NewLanPath = dialog.FileName;

			}
		}

		private void LanMakerUsage_SelectionChangeCommitted(object sender, EventArgs e)
		{
			this.ChangeSaveButtonEnabledState();
		}

		private void LanMakerGame_SelectionChangeCommitted(object sender, EventArgs e)
		{
			this.ChangeSaveButtonEnabledState();
		}

		private void LanMakerButtonHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Launcher is a Version 1 .end file that is used for loading game files, " +
				"linked files, main endscript and game selection.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void LanMakerTextBoxDir_TextChanged(object sender, EventArgs e)
		{
			this.ChangeSaveButtonEnabledState();
		}
	}
}
