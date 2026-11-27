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

        // True while the window itself fills the fields, so that is not taken as an edit
        private bool _loading;

        // Colors or details were edited and not saved to a file yet
        private bool _edited;

        // The list item shown, so a refused switch can go back to it
        private int _shownIndex = -1;

        public ThemeSelector()
        {
            this.InitializeComponent();
            Binary.UI.DialogLayout.Attach(this); // re-flow for the current DPI before showing

            this.ThemeNameTextBox.TextChanged += this.ThemeDetails_Changed;
            this.ThemeAuthorTextBox.TextChanged += this.ThemeDetails_Changed;
            this.ThemeVersionTextBox.TextChanged += this.ThemeDetails_Changed;
            this.CheckDarkTheme.CheckedChanged += this.ThemeDetails_Changed;

            this.GetThemes(Configurations.Default.ThemeFile);
        }

        private void ThemeDetails_Changed(object sender, EventArgs e)
        {
            if (!this._loading) this._edited = true;
        }

        /// <summary>
        /// Fills the list and selects <paramref name="select"/>, or the first item when it is not there.
        /// Selecting shows that theme.
        /// </summary>
        private void GetThemes(string select)
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

            this.ThemeListComboBox.EndUpdate();

            // Exact match: FindString would also take a theme whose name only starts the same way
            int index = this.ThemeListComboBox.FindStringExact(select ?? String.Empty);
            this._shownIndex = -1;
            this.ThemeListComboBox.SelectedIndex = index != -1 ? index : 0;
            if (this._shownIndex != this.ThemeListComboBox.SelectedIndex) this.ShowSelectedTheme(); // same index: no change event
        }

        private void ShowSelectedTheme()
        {
            string selected = this.ThemeListComboBox.SelectedItem?.ToString() ?? String.Empty;
            this._shownIndex = this.ThemeListComboBox.SelectedIndex;
            this.ToggleTheme(selected);
        }

        private void ToggleTheme(string selected)
        {
            bool automatic = selected == "Automatic";
            string filename = automatic
                ? Path.Combine("Themes", Theme.GetAutomaticThemeFile((GameINT)Configurations.Default.CurrentGame))
                : Path.Combine("Themes", selected);

            this._loading = true;
            this._edited = false;
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

            if (automatic) // shows the current game's theme, which can't be edited from here
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

            this._loading = false;
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
                                this._edited = true;
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

        /// <summary>
        /// Asks what to do with edits that are not saved to a file. Returns false when the user cancels.
        /// </summary>
        private bool ResolveEdits(string action)
        {
            if (!this._edited) return true;

            var answer = MessageBox.Show(this,
                "The colors or details of this theme were changed but not saved to a file.\n\n" +
                $"Save them as a theme before {action}?\n\nYes: choose where to save.\nNo: discard the changes.",
                "Unsaved Theme", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            return answer switch
            {
                DialogResult.Yes => this.SaveThemeAs(),
                DialogResult.No => true,
                _ => false,
            };
        }

        private void ThemeButtonOK_Click(object sender, EventArgs e)
        {
            if (!this.ResolveEdits("using it")) return;

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
            this.SaveThemeAs();
        }

        /// <summary>
        /// Saves the shown theme to a file the user picks. A theme saved in the Themes folder is selected
        /// in the list. Returns false when nothing was saved.
        /// </summary>
        private bool SaveThemeAs()
        {
            string folder = Path.GetFullPath("Themes");
            string shown = this.ThemeListComboBox.SelectedItem?.ToString() ?? String.Empty;

            using var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                DefaultExt = ".json",
                Filter = "json Files|*.json",
                InitialDirectory = folder,
                FileName = shown.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? shown : String.Empty,
                OverwritePrompt = true,
                SupportMultiDottedExtensions = true,
                Title = "Save Theme",
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return false;

            CurrentTheme.Name = ThemeNameTextBox.Text;
            CurrentTheme.Author = ThemeAuthorTextBox.Text;
            CurrentTheme.Version = ThemeVersionTextBox.Text;
            CurrentTheme.DarkTheme = CheckDarkTheme.Checked;

            try
            {
                Theme.Serialize(dialog.FileName, CurrentTheme);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            this._edited = false;
            bool inThemes = String.Equals(Path.GetDirectoryName(Path.GetFullPath(dialog.FileName)), folder, StringComparison.OrdinalIgnoreCase);

            MessageBox.Show(this, $"File {dialog.FileName} has been saved." + (inThemes ? String.Empty :
                "\n\nIt is outside the Themes folder, so it won't be in the list. Move it there to use it."),
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Show the saved theme; a theme saved elsewhere leaves the list as it was
            this.GetThemes(inThemes ? Path.GetFileName(dialog.FileName) : shown);
            return true;
        }

        private void ThemeListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // -1 while the list is refilled
            if (this.ThemeListComboBox.SelectedIndex == -1 || this.ThemeListComboBox.SelectedIndex == this._shownIndex) return;

            string target = this.ThemeListComboBox.SelectedItem.ToString();

            if (this._shownIndex != -1 && !this.ResolveEdits("switching themes"))
            {
                this.ThemeListComboBox.SelectedIndex = this._shownIndex; // back to the edited theme, raises no prompt
                return;
            }

            // Saving refills the list and selects the saved theme: go on to the one the user picked
            int index = this.ThemeListComboBox.FindStringExact(target);
            if (index != -1 && index != this.ThemeListComboBox.SelectedIndex)
            {
                this.ThemeListComboBox.SelectedIndex = index;
                return;
            }

            this.ShowSelectedTheme();
        }

        private void CheckDarkTheme_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
