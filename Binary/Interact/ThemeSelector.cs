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
    public partial class ThemeSelector : Form
    {
        Theme CurrentTheme;
        public ThemeSelector()
        {
            this.InitializeComponent();
            this.GetThemes();
            this.ToggleTheme();
        }

        private void GetThemes()
        {
            this.ThemeListComboBox.BeginUpdate();

            this.ThemeListComboBox.Items.Clear();
            this.ThemeListComboBox.Items.Add("System");
            this.ThemeListComboBox.Items.Add("Automatic");

            if (!Directory.Exists("Themes"))
            {
                Directory.CreateDirectory("Themes");
            }

            // Get themes from dir
            var themes = Directory.GetFiles("Themes", "*.json", SearchOption.TopDirectoryOnly);
            foreach (var theme in themes) this.ThemeListComboBox.Items.Add(Path.GetFileName(theme));

            // Set combo box
            int index = this.ThemeListComboBox.FindString(Configurations.Default.ThemeFile);
            this.ThemeListComboBox.SelectedIndex = index != -1 ? index : 0;

            this.ThemeListComboBox.EndUpdate();
        }

        private void ToggleTheme()
        {
            this.ToggleTheme(Theme.GetThemeFile());
        }

        private void ToggleTheme(string filename)
        {
            Theme.Deserialize(filename, out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.ThemeButtonOK.BackColor = theme.Colors.ButtonBackColor;
            this.ThemeButtonOK.ForeColor = theme.Colors.ButtonForeColor;
            this.ThemeButtonOK.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.ThemeButtonCancel.BackColor = theme.Colors.ButtonBackColor;
            this.ThemeButtonCancel.ForeColor = theme.Colors.ButtonForeColor;
            this.ThemeButtonCancel.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.ThemeButtonSaveAs.BackColor = theme.Colors.ButtonBackColor;
            this.ThemeButtonSaveAs.ForeColor = theme.Colors.ButtonForeColor;
            this.ThemeButtonSaveAs.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.ThemeLabel.ForeColor = theme.Colors.LabelTextColor;
            this.ThemeListComboBox.BackColor = theme.Colors.TextBoxBackColor;
            this.ThemeListComboBox.ForeColor = theme.Colors.TextBoxForeColor;
            this.ThemeNameLabel.ForeColor = theme.Colors.LabelTextColor;
            this.ThemeAuthorLabel.ForeColor = theme.Colors.LabelTextColor;
            this.ThemeVersionLabel.ForeColor = theme.Colors.LabelTextColor;
            this.ThemeColorsLabel.ForeColor = theme.Colors.LabelTextColor;
            this.ThemeNameTextBox.ForeColor = theme.Colors.TextBoxForeColor;
            this.ThemeAuthorTextBox.ForeColor = theme.Colors.TextBoxForeColor;
            this.ThemeVersionTextBox.ForeColor = theme.Colors.TextBoxForeColor;
            this.ThemeNameTextBox.BackColor = theme.Colors.TextBoxBackColor;
            this.ThemeAuthorTextBox.BackColor = theme.Colors.TextBoxBackColor;
            this.ThemeVersionTextBox.BackColor = theme.Colors.TextBoxBackColor;
            this.ThemeColorsList.BackColor = theme.Colors.TextBoxBackColor;
            this.ThemeColorsList.ForeColor = theme.Colors.TextBoxForeColor;
            this.CheckDarkTheme.ForeColor = theme.Colors.LabelTextColor;

            CurrentTheme = theme;

            this.ThemeColorsList.Items.Clear();

            if (Configurations.Default.ThemeFile == "Automatic" || filename == "Automatic") // todo: make this better
            {
                ThemeButtonSaveAs.Enabled = false;
                ThemeNameTextBox.Enabled = false;
                ThemeAuthorTextBox.Enabled = false;
                ThemeVersionTextBox.Enabled = false;
                CheckDarkTheme.Enabled = false;

                ThemeNameTextBox.Text = "Automatic";
                ThemeAuthorTextBox.Text = "N/A";
                ThemeVersionTextBox.Text = "N/A";
                CheckDarkTheme.Checked = false;
            }
            else
            {
                ThemeButtonSaveAs.Enabled = true;
                ThemeNameTextBox.Enabled = true;
                ThemeAuthorTextBox.Enabled = true;
                ThemeVersionTextBox.Enabled = true;
                CheckDarkTheme.Enabled = true;

                ThemeNameTextBox.Text = CurrentTheme.Name;
                ThemeAuthorTextBox.Text = CurrentTheme.Author;
                ThemeVersionTextBox.Text = CurrentTheme.Version;
                CheckDarkTheme.Checked = CurrentTheme.DarkTheme;

                this.ThemeColorsList.BeginUpdate();

                foreach (var color in CurrentTheme.Colors.GetType().GetProperties())
                {
                    var colorValue = color.GetValue(CurrentTheme.Colors);
                    if (colorValue is not null)
                    {
                        var lwi = new ListViewItem(color.Name);
                        lwi.BackColor = colorValue is System.Drawing.Color c ? c : System.Drawing.Color.Black;
                        lwi.ForeColor = lwi.BackColor.GetBrightness() < 0.5f ? System.Drawing.Color.White : System.Drawing.Color.Black;
                        this.ThemeColorsList.Items.Add(lwi);
                    }
                }

                this.ThemeColorsList.EndUpdate();
            }

        }

        private void ThemeColorsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = ThemeColorsList.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null)
            {
                var ColorSelector = new ColorDialog();
                ColorSelector.Color = item.BackColor;

                if (ColorSelector.ShowDialog() == DialogResult.OK)
                {
                    item.BackColor = ColorSelector.Color;

                    foreach (var color in CurrentTheme.Colors.GetType().GetProperties())
                    {
                        var colorValue = color.GetValue(CurrentTheme.Colors);
                        if (colorValue is not null)
                        {
                            if (color.Name.Equals(item.Text, StringComparison.OrdinalIgnoreCase))
                            {
                                // Update the color in the theme
                                color.SetValue(CurrentTheme.Colors, ColorSelector.Color);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                this.ThemeColorsList.SelectedItems.Clear();
            }
        }

        private void ThemeButtonOK_Click(object sender, EventArgs e)
        {
            if (this.ThemeListComboBox.SelectedItem is string selectedTheme)
            {
                Configurations.Default.ThemeFile = selectedTheme;
                Configurations.Default.Save();
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ThemeButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ThemeButtonSaveAs_Click(object sender, EventArgs e)
        {
            // Save current theme
            using var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                DefaultExt = ".json",
                Filter = "json Files|*.json",
                OverwritePrompt = true,
                SupportMultiDottedExtensions = true,
                Title = "Save Theme",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentTheme.Name = ThemeNameTextBox.Text;
                CurrentTheme.Author = ThemeAuthorTextBox.Text;
                CurrentTheme.Version = ThemeVersionTextBox.Text;
                CurrentTheme.DarkTheme = CheckDarkTheme.Checked;

                Theme.Serialize(dialog.FileName, CurrentTheme);

                MessageBox.Show($"File {dialog.FileName} has been saved.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.GetThemes();
            }
        }

        private void ThemeListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTheme = this.ThemeListComboBox.SelectedItem?.ToString() ?? string.Empty;
            this.ToggleTheme(Path.Combine("Themes", selectedTheme));
        }

        private void CheckDarkTheme_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
