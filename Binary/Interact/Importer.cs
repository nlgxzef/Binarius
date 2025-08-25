using Binary.Properties;

using System;
using System.IO;
using System.Windows.Forms;



namespace Binary.Interact
{
	public partial class Importer : Form
	{
		public int SerializationIndex { get; private set; } = 0;

		public Importer()
		{
			this.InitializeComponent();
			this.ToggleTheme();
			this.ImporterType.SelectedIndex = 0;

			string tip = "Serialazed import type of the collection. See Readme/Tutorials for more details.";
			this.ImporterToolTip.SetToolTip(this.ImporterLabel, tip);
			this.ImporterToolTip.SetToolTip(this.ImporterType, tip);
		}

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
			this.ForeColor = theme.Colors.MainForeColor;
			this.ImporterButton.BackColor = theme.Colors.ButtonBackColor;
			this.ImporterButton.ForeColor = theme.Colors.ButtonForeColor;
			this.ImporterButton.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
			this.ImporterLabel.BackColor = theme.Colors.MainBackColor;
			this.ImporterLabel.ForeColor = theme.Colors.LabelTextColor;
			this.ImporterType.BackColor = theme.Colors.TextBoxBackColor;
			this.ImporterType.ForeColor = theme.Colors.TextBoxForeColor;
		}

		private void ImporterButton_Click(object sender, EventArgs e)
		{
			this.SerializationIndex = this.ImporterType.SelectedIndex;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
