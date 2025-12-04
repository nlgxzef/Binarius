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
using Nikki.Reflection.Abstract;
using Nikki.Reflection.Interface;
using Nikki.Support.Carbon.Framework;
using Nikki.Support.Shared.Class;
using Nikki.Utils;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Binary
{
    public partial class Editor : Form
    {
        private BaseProfile Profile { get; set; }
        private readonly List<Form> _openforms;
        private const string empty = "\"\"";
        private const string space = " ";
        private bool _edited = false;

        private Color HighlightColor1 { get; set; }
        private Color HighlightColor2 { get; set; }
        private Color HighlightColor3 { get; set; }

        public Editor()
        {
            this._openforms = new List<Form>();
            this.InitializeComponent();
            this.splitContainer2.FixedPanel = FixedPanel.Panel1;
            this.ToggleTheme();
            this.ManageExperimentalFeatures();
            this.Text = $"Binarius - v{this.ProductVersion}";
        }

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            // Primary colors and controls
            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;

            // Tree view
            this.EditorTreeView.BackColor = theme.Colors.PrimBackColor;
            this.EditorTreeView.ForeColor = theme.Colors.PrimForeColor;
            this.EditorTreeView.SelectedImageIndex = theme.DarkTheme ? 3 : 2;
            this.EditorTreeView.ImageIndex = theme.DarkTheme ? 1 : 0;

            // Property grid
            this.EditorPropertyGrid.BackColor = theme.Colors.PrimBackColor;
            this.EditorPropertyGrid.CategorySplitterColor = theme.Colors.ButtonBackColor;
            this.EditorPropertyGrid.CategoryForeColor = theme.Colors.TextBoxForeColor;
            this.EditorPropertyGrid.CommandsBackColor = theme.Colors.PrimBackColor;
            this.EditorPropertyGrid.CommandsForeColor = theme.Colors.PrimForeColor;
            this.EditorPropertyGrid.CommandsBorderColor = theme.Colors.PrimBackColor;
            this.EditorPropertyGrid.DisabledItemForeColor = theme.Colors.LabelTextColor;
            this.EditorPropertyGrid.LineColor = theme.Colors.ButtonBackColor;
            this.EditorPropertyGrid.SelectedItemWithFocusBackColor = theme.Colors.FocusedBackColor;
            this.EditorPropertyGrid.SelectedItemWithFocusForeColor = theme.Colors.FocusedForeColor;
            this.EditorPropertyGrid.ViewBorderColor = theme.Colors.RegBorderColor;
            this.EditorPropertyGrid.ViewBackColor = theme.Colors.PrimBackColor;
            this.EditorPropertyGrid.ViewForeColor = theme.Colors.PrimForeColor;

            // Menu strip and menu items
            this.EditorMenuStrip.MenuStripGradientBegin = theme.Colors.MenuStripGradientBegin;
            this.EditorMenuStrip.MenuStripGradientEnd = theme.Colors.MenuStripGradientEnd;
            this.EditorMenuStrip.MenuStripForeColor = theme.Colors.LabelTextColor;
            this.EditorMenuStrip.MenuBorder = theme.Colors.MenuBorder;
            this.EditorMenuStrip.MenuItemBorder = theme.Colors.MenuItemBorder;
            this.EditorMenuStrip.MenuItemPressedGradientBegin = theme.Colors.MenuItemPressedGradientBegin;
            this.EditorMenuStrip.MenuItemPressedGradientMiddle = theme.Colors.MenuItemPressedGradientMiddle;
            this.EditorMenuStrip.MenuItemPressedGradientEnd = theme.Colors.MenuItemPressedGradientEnd;
            this.EditorMenuStrip.MenuItemSelected = theme.Colors.MenuItemSelected;
            this.EditorMenuStrip.MenuItemSelectedGradientBegin = theme.Colors.MenuItemSelectedGradientBegin;
            this.EditorMenuStrip.MenuItemSelectedGradientEnd = theme.Colors.MenuItemSelectedGradientEnd;
            this.EditorMenuStrip.ImageMarginGradientBegin = theme.Colors.MenuItemPressedGradientBegin;
            this.EditorMenuStrip.ImageMarginGradientMiddle = theme.Colors.MenuItemPressedGradientMiddle;
            this.EditorMenuStrip.ImageMarginGradientEnd = theme.Colors.MenuItemPressedGradientEnd;

            this.EMSMainNewLauncher.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSMainNewLauncher.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSMainLoadFiles.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSMainLoadFiles.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSMainReloadFiles.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSMainReloadFiles.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSMainSaveFiles.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSMainSaveFiles.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSMainImportEndscript.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSMainImportEndscript.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSMainExit.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSMainExit.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSToolsHasher.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSToolsHasher.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSToolsRaider.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSToolsRaider.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSToolsSwatcher.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSToolsSwatcher.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSOptionsCreate.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSOptionsCreate.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSOptionsRestore.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSOptionsRestore.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSOptionsUnlock.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSOptionsUnlock.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSOptionsSpeedReflect.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSOptionsSpeedReflect.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSOptionsToggle.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSOptionsToggle.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSOptionsSettings.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSOptionsSettings.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSScriptingProcess.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSScriptingProcess.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSScriptingRunAll.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSScriptingRunAll.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSScriptingGenerate.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSScriptingGenerate.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSScriptingClear.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSScriptingClear.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSWindowsRun.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSWindowsRun.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSWindowsOpenDir.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSWindowsOpenDir.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSWindowsNew.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSWindowsNew.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSHelpAbout.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSHelpAbout.ForeColor = theme.Colors.MenuItemForeColor;
            this.EMSHelpTutorials.BackColor = theme.Colors.MenuItemBackColor;
            this.EMSHelpTutorials.ForeColor = theme.Colors.MenuItemForeColor;

            // Context menu
            this.EditorContextMenu.BackColor = theme.Colors.MainBackColor;
            this.EditorContextMenu.ForeColor = theme.Colors.LabelTextColor;
            this.addNodeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.addNodeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.removeNodeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.removeNodeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.exportNodeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.exportNodeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.importNodeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.importNodeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.scriptNodeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.scriptNodeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.copyNodeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.copyNodeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.openEditorToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.openEditorToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.toolStripSeparator1.ForeColor = theme.Colors.MenuItemForeColor;
            this.toolStripSeparator1.BackColor = theme.Colors.MenuItemBackColor;
            this.toolStripSeparator2.ForeColor = theme.Colors.MenuItemForeColor;
            this.toolStripSeparator2.BackColor = theme.Colors.MenuItemBackColor;
            this.toolStripSeparator3.ForeColor = theme.Colors.MenuItemForeColor;
            this.toolStripSeparator3.BackColor = theme.Colors.MenuItemBackColor;
            this.toolStripSeparator4.ForeColor = theme.Colors.MenuItemForeColor;
            this.toolStripSeparator4.BackColor = theme.Colors.MenuItemBackColor;
            this.toolStripSeparator5.ForeColor = theme.Colors.MenuItemForeColor;
            this.toolStripSeparator5.BackColor = theme.Colors.MenuItemBackColor;
            this.moveNodeDownToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.moveNodeDownToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.moveNodeUpToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.moveNodeUpToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.moveNodeFirstToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.moveNodeFirstToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.moveNodeLastToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.moveNodeLastToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;

            // Status strip
            //this.EditorStatusStrip.BackColor = Color.Transparent;
            this.EditorStatusStrip.StatusStripForeColor = theme.Colors.LabelTextColor;
            this.EditorStatusStrip.StatusStripGradientBegin = theme.Colors.StatusStripGradientBegin;
            this.EditorStatusStrip.StatusStripGradientEnd = theme.Colors.StatusStripGradientEnd;

            // Textboxes
            this.EditorCommandPrompt.BackColor = theme.Colors.PrimBackColor;
            this.EditorCommandPrompt.ForeColor = theme.Colors.PrimForeColor;
            this.EditorFindTextBox.BackColor = theme.Colors.TextBoxBackColor;
            this.EditorFindTextBox.ForeColor = theme.Colors.TextBoxForeColor;

            // Highlight colors
            this.HighlightColor1 = theme.DarkTheme
                        ? Color.FromArgb(160, 20, 30)
                        : Color.FromArgb(60, 255, 60);

            this.HighlightColor2 = theme.DarkTheme
                        ? Color.FromArgb(255, 230, 0)
                        : Color.FromArgb(255, 20, 20);

            this.HighlightColor3 = theme.DarkTheme
                        ? Color.FromArgb(255, 230, 0)
                        : Color.FromArgb(255, 90, 0);

        }

        #endregion

        #region Manage Controls

        private void CreateBackupsForFiles(bool forced)
        {
            try
            {

                if (this.Profile.Count > 0)
                {

                    foreach (var sdb in this.Profile)
                    {

                        string from = sdb.FullPath;
                        string to = $"{sdb.FullPath}.bacc";

                        if (forced || (!forced && !File.Exists(to)))
                        {

                            File.Copy(from, to, true);

                        }

                    }

                    if (forced)
                    {

                        _ = MessageBox.Show("All files have been successfully backed up.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
                else
                {

                    throw new Exception("No files are open and directory is not chosen");

                }

            }
            catch (Exception ex)
            {

                if (forced)
                {

                    _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }

        private void ToggleControlsAfterLoad(bool enable)
        {
            this.EMSMainReloadFiles.Enabled = enable;
            this.EMSMainSaveFiles.Enabled = enable;
            this.EMSMainImportEndscript.Enabled = enable;
            this.EMSOptionsCreate.Enabled = enable;
            this.EMSOptionsRestore.Enabled = enable;
            this.EMSOptionsUnlock.Enabled = enable;
            this.EMSOptionsSpeedReflect.Enabled = enable;
            this.EMSWindowsRun.Enabled = enable;
            this.EMSWindowsOpenDir.Enabled = enable;
        }

        private void ManageExperimentalFeatures()
        {
            bool enable = Configurations.Default.SoonFeature;

            this.toolStripSeparator5.Visible = enable;
            this.moveNodeDownToolStripMenuItem.Visible = enable;
            this.moveNodeUpToolStripMenuItem.Visible = enable;
            this.moveNodeFirstToolStripMenuItem.Visible = enable;
            this.moveNodeLastToolStripMenuItem.Visible = enable;
        }

        private void ManageButtonOpenEditor(IReflective reflective)
        {
            this.openEditorToolStripMenuItem.Enabled =
                reflective is FNGroup or
                STRBlock or
                TPKBlock or
                DBModelPart or
                GCareer or
                VectorVinyl;
        }

        private void ManageButtonAddNode(TreeNode node)
        {
            if (node == null || node.Level != 1)
            {

                this.addNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var sdb = this.Profile.Find(_ => _.Filename == node.Parent.Text);

            if (sdb == null)
            {

                this.addNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var manager = sdb.Database.GetManager(node.Text);
            this.addNodeToolStripMenuItem.Enabled = manager != null && !manager.IsReadOnly;
        }

        private void ManageButtonRemoveNode(TreeNode node)
        {
            if (node == null || node.Level != 2)
            {

                this.removeNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var sdb = this.Profile.Find(_ => _.Filename == node.Parent.Parent.Text);

            if (sdb == null)
            {

                this.removeNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var manager = sdb.Database.GetManager(node.Parent.Text);
            this.removeNodeToolStripMenuItem.Enabled = manager != null && !manager.IsReadOnly;
        }

        private void ManageButtonCopyNode(TreeNode node)
        {
            if (node == null || node.Level != 2)
            {

                this.copyNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var sdb = this.Profile.Find(_ => _.Filename == node.Parent.Parent.Text);

            if (sdb == null)
            {

                this.copyNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var manager = sdb.Database.GetManager(node.Parent.Text);
            this.copyNodeToolStripMenuItem.Enabled = manager != null && !manager.IsReadOnly;
        }

        private void ManageButtonExportNode(TreeNode node)
        {
            if (node == null || node.Level != 2)
            {

                this.exportNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var sdb = this.Profile.Find(_ => _.Filename == node.Parent.Parent.Text);

            if (sdb == null)
            {

                this.exportNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var manager = sdb.Database.GetManager(node.Parent.Text);
            this.exportNodeToolStripMenuItem.Enabled = manager != null;
        }

        private void ManageButtonImportNode(TreeNode node)
        {
            if (node == null || node.Level != 1)
            {

                this.importNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var sdb = this.Profile.Find(_ => _.Filename == node.Parent.Text);

            if (sdb == null)
            {

                this.importNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var manager = sdb.Database.GetManager(node.Text);
            this.importNodeToolStripMenuItem.Enabled = manager != null;
        }

        private void ManageButtonScriptNode(TreeNode node)
        {
            if (node == null || node.Level != 2)
            {

                this.scriptNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var sdb = this.Profile.Find(_ => _.Filename == node.Parent.Parent.Text);

            if (sdb == null)
            {

                this.scriptNodeToolStripMenuItem.Enabled = false;
                return;

            }

            var manager = sdb.Database.GetManager(node.Parent.Text);
            this.scriptNodeToolStripMenuItem.Enabled =
                manager != null &&
                !typeof(DBModelPart).IsAssignableFrom(manager.CollectionType) &&
                !typeof(FNGroup).IsAssignableFrom(manager.CollectionType) &&
                !typeof(STRBlock).IsAssignableFrom(manager.CollectionType);
        }

        private void ManageButtonMoveNode(TreeNode node)
        {


            if (node == null || node.Level != 2)
            {

                this.moveNodeDownToolStripMenuItem.Enabled = false;
                this.moveNodeUpToolStripMenuItem.Enabled = false;
                this.moveNodeFirstToolStripMenuItem.Enabled = false;
                this.moveNodeLastToolStripMenuItem.Enabled = false;
                return;

            }

            var sdb = this.Profile.Find(_ => _.Filename == node.Parent.Parent.Text);

            if (sdb == null)
            {

                this.moveNodeDownToolStripMenuItem.Enabled = false;
                this.moveNodeUpToolStripMenuItem.Enabled = false;
                this.moveNodeFirstToolStripMenuItem.Enabled = false;
                this.moveNodeLastToolStripMenuItem.Enabled = false;
                return;

            }

            var manager = sdb.Database.GetManager(node.Parent.Text);
            this.moveNodeDownToolStripMenuItem.Enabled = manager != null && !manager.IsReadOnly;
            this.moveNodeUpToolStripMenuItem.Enabled = this.moveNodeDownToolStripMenuItem.Enabled;
            this.moveNodeFirstToolStripMenuItem.Enabled = this.moveNodeDownToolStripMenuItem.Enabled;
            this.moveNodeLastToolStripMenuItem.Enabled = this.moveNodeDownToolStripMenuItem.Enabled;
        }

        #endregion

        #region Menu Strip Controls

        private void EMSMainNewLauncher_Click(object sender, EventArgs e)
        {
            using var form = new LanMaker();
            _ = form.ShowDialog();

            if (form.WasCreated)
            {

                var result = MessageBox.Show("New launcher was created. Would you like to load it?", "Prompt",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {

                    this.LoadProfile(form.NewLanPath, true);

                }

            }

        }

        static string m_loadFilesLastDir = null;

        private void EMSMainLoadFiles_Click(object sender, EventArgs e)
        {
            if (this._edited)
            {

                var result = MessageBox.Show("You have unsaved changes. Are you sure you want to load " +
                    "another database?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            using var browser = new OpenFileDialog()
            {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Binary/ius End Launcher Files|*.end;*.endlauncher|Binary End Launcher Files|*.end|Binarius End Launcher Files|*.endlauncher|All Files|*.*",
                Multiselect = false,
                Title = "Load End Launcher",
            };

            if (!String.IsNullOrEmpty(m_loadFilesLastDir))
            {
                browser.InitialDirectory = m_loadFilesLastDir;
            }

            if (browser.ShowDialog() == DialogResult.OK)
            {
                m_loadFilesLastDir = Path.GetDirectoryName(browser.FileName);
                this.LoadProfile(browser.FileName, true);

            }
        }

        private void EMSMainReloadFiles_Click(object sender, EventArgs e)
        {
            string file = Configurations.Default.LaunchFile;

            if (File.Exists(file))
            {

                if (this._edited)
                {

                    var result = MessageBox.Show("You have unsaved changes. Are you sure you reload " +
                        "database?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                this.LoadProfile(file, true);

            }
            else
            {

                _ = MessageBox.Show($"Launch file {file} does not exist or was moved.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void EMSMainSaveFiles_Click(object sender, EventArgs e) => this.SaveProfile();

        static string m_importEndscriptLastDir = null;

        private void EMSMainImportEndscript_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "Binary/ius Endscript Files|*.end;*.endscript|Binary Endscript Files|*.end|Binarius Endscript Files|*.endscript|All Files|*.*",
                Multiselect = false,
                Title = "Import Endscript",
            };

            if (!String.IsNullOrEmpty(m_importEndscriptLastDir))
            {
                dialog.InitialDirectory = m_importEndscriptLastDir;
            }

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var parser = new EndScriptParser(dialog.FileName);
            BaseCommand[] commands;

            m_importEndscriptLastDir = Path.GetDirectoryName(dialog.FileName);

            try
            {

                commands = parser.Read();

            }
            catch (Exception ex)
            {

                string error = $"Error has occured -> File: {parser.CurrentFile}, Line: {parser.CurrentIndex}" +
                    Environment.NewLine + $"Command: [{parser.CurrentLine}]" + Environment.NewLine +
                    $"Error: {ex.GetLowestMessage()}";

                _ = MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            var manager = new EndScriptManager(this.Profile, commands, dialog.FileName);

            try
            {

                manager.CommandChase();

                while (!manager.ProcessScript())
                {

                    var command = manager.CurrentCommand;

                    if (command is InfoboxCommand infobox)
                    {

                        using var input = new Info(infobox.Description);
                        _ = input.ShowDialog();

                    }
                    else if (command is CheckboxCommand checkbox)
                    {

                        using var input = new Check(checkbox.Description, true);
                        _ = input.ShowDialog();
                        checkbox.Choice = input.Value ? 1 : 0;

                    }
                    else if (command is ComboboxCommand combobox)
                    {

                        string[] options = new string[combobox.Options.Length];
                        for (int i = 0; i < options.Length; ++i)
                        {
                            options[i] = combobox.Options[i].Name;
                        }

                        using var input = new Combo(options, combobox.Description, true);
                        _ = input.ShowDialog();
                        combobox.Choice = input.Value < 0 ? 0 : input.Value;

                    }

                }

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            string script = Path.GetFileName(dialog.FileName);

            if (manager.Errors.Any())
            {

                Utils.WriteErrorsToLog(manager.Errors, dialog.FileName);

                using var prompt = new CheckError($"Script {script} has been applied, however, {manager.Errors.Count()} errors " +
                    $"have been detected. Check EndError.log for more information. Do not forget to save changes!",
                    "Open log", true, true);

                _ = prompt.ShowDialog();

                if (prompt.Value)
                {
                    var psi = new ProcessStartInfo("EndError.log")
                    {
                        UseShellExecute = true
                    };

                    _ = Process.Start(psi);
                }
            }
            else
            {

                _ = MessageBox.Show($"Script {script} has been successfully applied. Do not forget to save changes!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            this.LoadTreeView(this.EditorTreeView.SelectedNode?.FullPath);
        }

        private void EMSMainExit_Click(object sender, EventArgs e) => this.Close();

        private void EMSToolsHasher_Click(object sender, EventArgs e)
        {
            var hasher = new Hasher() { StartPosition = FormStartPosition.CenterScreen };
            this._openforms.Add(hasher);
            hasher.Show();
        }

        private void EMSToolsRaider_Click(object sender, EventArgs e)
        {
            var raider = new Raider() { StartPosition = FormStartPosition.CenterScreen };
            this._openforms.Add(raider);
            raider.Show();
        }

        private void EMSToolsSwatcher_Click(object sender, EventArgs e)
        {
            var swatcher = new Swatcher() { StartPosition = FormStartPosition.CenterScreen };
            this._openforms.Add(swatcher);
            swatcher.Show();
        }

        private void EMSOptionsCreate_Click(object sender, EventArgs e) => this.CreateBackupsForFiles(true);

        private void EMSOptionsRestore_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.Profile.Count > 0)
                {

                    int count = 0;

                    foreach (var sdb in this.Profile)
                    {

                        string from = $"{sdb.FullPath}.bacc";
                        string to = sdb.FullPath;
                        if (File.Exists(from)) { File.Copy(from, to, true); ++count; }

                    }

                    if (count == 0)
                    {

                        _ = MessageBox.Show("No backup files were found.", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;

                    }

                    this.LoadProfile(Configurations.Default.LaunchFile, true);

                    _ = MessageBox.Show("All backups have been successfully restored.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {

                    throw new Exception("No files are open and directory is not chosen");

                }

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void EMSOptionsUnlock_Click(object sender, EventArgs e)
        {
#if !DEBUG
            try
            {
#endif

                if (this.Profile?.Count > 0)
                {

                    string path = this.Profile[0].Folder;

                    MemoryUnlock.FastUnlock(path + @"\GLOBAL\CARHEADERSMEMORYFILE.BIN");
                    MemoryUnlock.FastUnlock(path + @"\GLOBAL\FRONTENDMEMORYFILE.BIN");
                    MemoryUnlock.FastUnlock(path + @"\GLOBAL\INGAMEMEMORYFILE.BIN");
                    MemoryUnlock.FastUnlock(path + @"\GLOBAL\PERMANENTMEMORYFILE.BIN");
                    MemoryUnlock.LongUnlock(path + @"\GLOBAL\GLOBALMEMORYFILE.BIN");

                    MessageBox.Show("Memory files were successfully unlocked for modding.", "Success");

                }
                else
                {

                    throw new Exception("No files are open and directory is not chosen");

                }

#if !DEBUG
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
#endif
        }

        private void EMSOptionsToggle_Click(object sender, EventArgs e)
        {
            var themes = new ThemeSelector() { StartPosition = FormStartPosition.CenterScreen };
            this._openforms.Add(themes);
            themes.ShowDialog();

            if (themes.DialogResult == DialogResult.OK)
            {
                this.ToggleTheme();
            }
        }

        private void EMSOptionsSettings_Click(object sender, EventArgs e)
        {
            var settings = new Interact.Options() { StartPosition = FormStartPosition.CenterScreen };
            this._openforms.Add(settings);
            settings.ShowDialog();

            if (settings.DialogResult == DialogResult.OK)
            {
                // Refresh tree view if Hide Empty Managers option was changed
                foreach (var sdb in this.Profile)
                {
                    foreach (TreeNode n in this.EditorTreeView.Nodes)
                    {
                        if (n.FullPath == sdb.Filename)
                        {
                            Utils.toggleEmptyManagersInSDB(sdb, n);
                            break;
                        }
                    }
                }

                // Check experimental features
                this.ManageExperimentalFeatures();
            }
        }

        private void EMSScriptingProcess_Click(object sender, EventArgs e)
        {
            if (this.Profile is null)
            {

                MessageBox.Show("No profile was loaded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            int index = this.EditorCommandPrompt.GetLineFromCharIndex(this.EditorCommandPrompt.SelectionStart);
            if (index < 0 || index >= this.EditorCommandPrompt.Lines.Length)
            {
                return;
            }

            string line = this.EditorCommandPrompt.Lines[index];
            eCommandType result;

#if !DEBUG
            try
            {
#endif

                result = EndScriptParser.ExecuteSingleCommand(line, this.Profile);

#if !DEBUG
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
#endif

            if (result == eCommandType.empty)
            {
                return;
            }

            // Analize and check if refreshing is required
            if (result == eCommandType.update_collection)
            {

                // Two options: we are updating current CollectionName or not
                if (line.Contains("CollectionName"))
                {

                    this.EditorPropertyGrid.SelectedObject = null;
                    this.LoadTreeView(this.EditorTreeView.SelectedNode?.FullPath);

                }
                else
                {

                    this.EditorPropertyGrid.Refresh();

                }

            }
            else
            {

                this.EditorPropertyGrid.SelectedObject = null;
                this.LoadTreeView(this.EditorTreeView.SelectedNode?.FullPath);

            }
        }

        private void EMSScriptingRunAll_Click(object sender, EventArgs e)
        {
            if (this.Profile is null)
            {

                _ = MessageBox.Show("No profile was loaded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            if (this.EditorCommandPrompt.Lines.Length == 0)
            {
                return;
            }

            this.EditorPropertyGrid.SelectedObject = null;

            string command = String.Empty;
            int count = 0;

            try
            {

                foreach (string line in this.EditorCommandPrompt.Lines)
                {

                    command = line;
                    _ = EndScriptParser.ExecuteSingleCommand(line, this.Profile);
                    ++count;

                }

            }
            catch (Exception ex)
            {

                string error = $"Error has occured -> Line: {count}" + Environment.NewLine +
                    $"Command: [{command}]" + Environment.NewLine + $"Error: {ex.GetLowestMessage()}";

                _ = MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            this.LoadTreeView(this.EditorTreeView.SelectedNode?.FullPath);
        }

        private void EMSScriptingGenerate_Click(object sender, EventArgs e)
        {
            if (this.EditorPropertyGrid.SelectedObject != null &&
                this.EditorPropertyGrid.SelectedGridItem.GridItemType == GridItemType.Property)
            {

                string path = this.EditorTreeView.SelectedNode.FullPath;
                string property = this.EditorPropertyGrid.SelectedGridItem.Label;
                object value = this.EditorPropertyGrid.SelectedGridItem.Value;
                var type = value.GetType();

                if (this.EditorPropertyGrid.SelectedGridItem.PropertyDescriptor.IsReadOnly)
                {
                    return;
                }

                if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type))
                {
                    return;
                }

                string str = this.GenerateEndCommand(eCommandType.update_collection, path, property, value.ToString());
                this.WriteLineToEndCommandPrompt(str);

            }
        }

        private void EMSScriptingClear_Click(object sender, EventArgs e) => this.EditorCommandPrompt.Text = String.Empty;

        private void EMSWindowsRun_Click(object sender, EventArgs e)
        {
            try
            {

                // Considering at least ONE file is currently open
                if (this.Profile.Count > 0)
                {

                    string path = this.Profile[0].Folder;
                    var game = this.Profile[0].Database.GameINT;
                    string exe = path;

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

                    _ = Process.Start(new ProcessStartInfo(exe) { WorkingDirectory = path });

                }
                else
                {

                    throw new Exception("No files are open and directory is not chosen");

                }

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void EMSWindowsNew_Click(object sender, EventArgs e)
        {
            string path = Process.GetCurrentProcess().MainModule.FileName;
            _ = Process.Start(new ProcessStartInfo() { FileName = path });
        }

        private void EMSHelpAbout_Click(object sender, EventArgs e)
        {
            var about = new About() { StartPosition = FormStartPosition.CenterScreen };
            this._openforms.Add(about);
            about.Show();
        }

        private void EMSHelpTutorials_Click(object sender, EventArgs e) => MessageBox.Show("Join Discord server at the start page to get help and full tool documentation!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        private void EMSOptionsSpeedReflect_Click(object sender, EventArgs e)
        {
#if !DEBUG
            try
            {
#endif

                if (this.Profile?.Count > 0)
                {
                    string dir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                    string speedfrom = Path.Combine(dir, "SpeedReflect.asi");

                    if (!File.Exists(speedfrom))
                    {

                        MessageBox.Show("SpeedReflect.asi was not found in the Binary directory.", "Error");
                        return;

                    }

                    string path = this.Profile[0].Folder;

                    string speedto = Path.Combine(this.Profile.Directory, @"scripts\SpeedReflect.asi");
                    Directory.CreateDirectory(Path.Combine(this.Profile.Directory, "scripts"));
                    File.Copy(speedfrom, speedto, true);

                    MessageBox.Show("Succesfully installed SpeedReflect.asi.", "Success");

                }
                else
                {

                    throw new Exception("No files are open and directory is not chosen");

                }

#if !DEBUG
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
#endif
        }


        private void EMSWindowsOpenDir_Click(object sender, EventArgs e)
        {
            try
            {

                // Considering at least ONE file is currently open
                if (this.Profile.Count > 0)
                {
                    string path = this.Profile[0].Folder;
                    Process.Start("explorer.exe", path);

                }
                else
                {

                    throw new Exception("No files are open and directory is not chosen");

                }

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        #endregion

        #region Button Controls

        private void EditorButtonAddNode_Click(object sender, EventArgs e)
        {
            // This button is enabled only in managers, so it is 
            // safe to assume that we are in a manager TreeNode

            string fname = this.EditorTreeView.SelectedNode.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Text;

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            using var input = new Input("Enter name of the new collection");

            while (true) // use loop instead of recursion to prevent stack overflow
            {

                if (input.ShowDialog() == DialogResult.OK)
                {

                    try
                    {

                        manager.Add(input.Value);
                        string path = this.EditorTreeView.SelectedNode.FullPath;
                        string str = this.GenerateEndCommand(eCommandType.add_collection, path, input.Value);
                        this.WriteLineToEndCommandPrompt(str);
                        var collection = manager[manager.IndexOf(input.Value)] as Collectable;
                        _ = this.EditorTreeView.SelectedNode.Nodes.Add(Utils.GetCollectionNodes(collection));
                        this._edited = true;
                        break;

                    }
                    catch (Exception ex)
                    {

                        _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
                else
                {

                    break;

                }

            }
        }

        private void EditorButtonRemoveNode_Click(object sender, EventArgs e)
        {
            // This button is enabled only in collections, so it is 
            // safe to assume that we are in a collection TreeNode

            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            try
            {

                this.EditorPropertyGrid.SelectedObject = null;
                manager.Remove(cname);
                string str = this.GenerateEndCommand(eCommandType.remove_collection, this.EditorTreeView.SelectedNode.FullPath);
                this.WriteLineToEndCommandPrompt(str);
                this.EditorTreeView.SelectedNode.Remove();
                this._edited = true;

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void EditorButtonCopyNode_Click(object sender, EventArgs e)
        {
            // This button is enabled only in collections, so it is 
            // safe to assume that we are in a collection TreeNode

            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            using var input = new Input("Enter name of the new collection");

            while (true) // use loop instead of recursion to prevent stack overflow
            {

                if (input.ShowDialog() == DialogResult.OK)
                {

                    try
                    {

                        manager.Clone(input.Value, cname);
                        string path = this.EditorTreeView.SelectedNode.FullPath;
                        string str = this.GenerateEndCommand(eCommandType.copy_collection, path, input.Value);
                        this.WriteLineToEndCommandPrompt(str);
                        var collection = manager[manager.IndexOf(input.Value)] as Collectable;
                        _ = this.EditorTreeView.SelectedNode.Parent.Nodes.Add(Utils.GetCollectionNodes(collection));
                        this._edited = true;
                        break;

                    }
                    catch (Exception ex)
                    {

                        _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
                else
                {

                    break;

                }

            }
        }

        private void EditorButtonOpenEditor_Click(object sender, EventArgs e)
        {
            // This button is enabled only in collections, so it is 
            // safe to assume that we are in a collection TreeNode

            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            object collection = manager[manager.FindIndex(cname)];
            string path = this.GetCurrentSeparatedPath();

            if (collection is DBModelPart model)
            {

                using var editor = new CarPartsEditor(model)
                {
                    WindowState = this.WindowState
                };

                this.Hide();
                _ = editor.ShowDialog();
                this.EditorPropertyGrid.Refresh();
                this.Show();
                this._edited = true;

            }
            else if (collection is FNGroup fng)
            {
                if (MessageBox.Show("Binary cannot edit FNGroups, but can import and delete them. To edit FNGroups, use Felipe379's fork of FEngLib by heyitsleo. Would you like to open the GitHub page?",
                    "Binary", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Utils.OpenBrowser("https://github.com/Felipe379/FEngLib");
                }
            }
            else if (collection is TPKBlock tpk)
            {

                using var editor = new TextureEditor(tpk, path)
                {
                    WindowState = this.WindowState
                };

                this.Hide();
                _ = editor.ShowDialog();
                this.EditorPropertyGrid.Refresh();
                this.Show();
                this.WriteLineToEndCommandPrompt(editor.Commands);
                this._edited = true;

            }
            else if (collection is STRBlock str)
            {

                using var editor = new StringEditor(str, path)
                {
                    WindowState = this.WindowState
                };

                this.Hide();
                _ = editor.ShowDialog();
                this.EditorPropertyGrid.Refresh();
                this.Show();
                this.WriteLineToEndCommandPrompt(editor.Commands);
                this._edited = true;

            }
            else if (collection is GCareer gcr)
            {

                using var editor = new CareerEditor(gcr, path)
                {
                    WindowState = this.WindowState
                };

                this.Hide();
                _ = editor.ShowDialog();
                this.EditorPropertyGrid.Refresh();
                this.Show();
                this.WriteLineToEndCommandPrompt(editor.Commands);
                this._edited = true;

            }
            else if (collection is VectorVinyl vinyl)
            {

                using var editor = new VectorEditor(vinyl)
                {
                    WindowState = this.WindowState
                };

                this.Hide();
                _ = editor.ShowDialog();
                this.EditorPropertyGrid.Refresh();
                this.Show();
                this._edited = true;

            }
        }

        static string m_exportNodeLastDir = null;
        private void EditorButtonExportNode_Click(object sender, EventArgs e)
        {
            // This button is enabled only in collections, so it is 
            // safe to assume that we are in a collection TreeNode

            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);
            var collection = manager[manager.FindIndex(cname)];

            using var exporter = new Exporter(manager.AllowsNoSerialization)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            if (true) // Skip the serialization dialog.
            {

                using var dialog = new SaveFileDialog()
                {
                    AddExtension = true,
                    AutoUpgradeEnabled = true,
                    CheckPathExists = true,
                    DefaultExt = ".bin",
                    Filter = "Binary Files|*.bin|All Files|*.*",
                    FileName = cname,
                    OverwritePrompt = true,
                    SupportMultiDottedExtensions = true,
                    Title = "Export Collection",
                };

                if (!String.IsNullOrEmpty(m_exportNodeLastDir))
                {
                    dialog.InitialDirectory = m_exportNodeLastDir;
                }

                if (collection is FNGroup fng)
                {
                    dialog.DefaultExt = ".fng";
                    dialog.Filter = "FNG Files|*.fng|" + dialog.Filter;
                }
                else if (collection is TPKBlock tpk)
                {
                    dialog.DefaultExt = ".tpk";
                    dialog.Filter = "Texture Packages|*.tpk|" + dialog.Filter;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    m_exportNodeLastDir = Path.GetDirectoryName(dialog.FileName);

#if !DEBUG
                    try
                    {
#endif

                        using var bw = new BinaryWriter(File.Open(dialog.FileName, FileMode.Create));
                        manager.Export(cname, bw, exporter.Serialized);
                        MessageBox.Show($"Collection {cname} has been exported to path {dialog.FileName}", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

#if !DEBUG
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }
#endif

                }

            }
        }


        static string m_importNodeLastDir = null;
        private void EditorButtonImportNode_Click(object sender, EventArgs e)
        {
            // This button is enabled only in managers, so it is 
            // safe to assume that we are in a manager TreeNode

            string fname = this.EditorTreeView.SelectedNode.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Text;

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            using var importer = new Importer()
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            if (importer.ShowDialog() == DialogResult.OK)
            {

                using var dialog = new OpenFileDialog()
                {
                    AutoUpgradeEnabled = true,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    DefaultExt = ".bin",
                    Filter = "Binary Files|*.bin|All Files|*.*",
                    Multiselect = false,
                    SupportMultiDottedExtensions = true,
                    Title = "Import Collection from File"
                };

                if (!String.IsNullOrEmpty(m_importNodeLastDir))
                {
                    dialog.InitialDirectory = m_importNodeLastDir;
                }

                if (typeof(FNGroup).IsAssignableFrom(manager.CollectionType))
                {
                    dialog.DefaultExt = ".fng";
                    dialog.Filter = "FNG Files|*.fng|" + dialog.Filter;
                }
                else if (typeof(TPKBlock).IsAssignableFrom(manager.CollectionType))
                {
                    dialog.DefaultExt = ".tpk";
                    dialog.Filter = "Texture Packages|*.tpk|" + dialog.Filter;
                }


                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    m_importNodeLastDir = Path.GetDirectoryName(dialog.FileName);

#if !DEBUG
                    try
                    {
#endif

                        var type = (Nikki.Reflection.Enum.SerializeType)importer.SerializationIndex;
                        using var br = new BinaryReader(File.Open(dialog.FileName, FileMode.Open));
                        manager.Import(type, br);
                        MessageBox.Show($"File {dialog.FileName} has been imported with type {type}", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.LoadTreeView(this.EditorTreeView.SelectedNode.FullPath);
                        this._edited = true;

#if !DEBUG
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }
#endif

                }

            }
        }

        private void EditorButtonScriptNode_Click(object sender, EventArgs e)
        {
            // This button is enabled only in collections, so it is 
            // safe to assume that we are in a collection TreeNode

            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);
            var collection = manager[manager.IndexOf(cname)] as Collectable;

            var properties = collection.GetAccessibles().ToList();
            properties.Sort();

            var lines = new List<string>(properties.Count);
            string path = this.EditorTreeView.SelectedNode.FullPath;

            foreach (string property in properties)
            {

                if (property.Equals("CollectionName", StringComparison.InvariantCulture))
                {
                    continue;
                }

                string value = collection.GetValue(property);
                if (String.IsNullOrEmpty(value))
                {
                    value = empty;
                }

                lines.Add(this.GenerateEndCommand(eCommandType.update_collection, path, property, value));

            }

            foreach (TreeNode node in this.EditorTreeView.SelectedNode.Nodes)
            {

                if (node.Nodes == null)
                {
                    continue;
                }

                foreach (TreeNode subnode in node.Nodes)
                {

                    path = subnode.FullPath;
                    string expand = node.Text;
                    string name = subnode.Text;
                    var part = collection.GetSubPart(name, expand);
                    if (part == null)
                    {
                        continue;
                    }

                    foreach (string property in part.GetAccessibles())
                    {

                        string value = part.GetValue(property);
                        if (String.IsNullOrEmpty(value))
                        {
                            value = empty;
                        }

                        lines.Add(this.GenerateEndCommand(eCommandType.update_collection, path, property, value));

                    }

                }

            }

            this.WriteLineToEndCommandPrompt(lines);
        }

        private void moveNodeUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;
            var index1 = this.EditorTreeView.SelectedNode.Index;
            var index2 = index1 - 1;

            if (index2 < 0)
            {

                MessageBox.Show("Unable to move up because selected node is the up most node",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            this.EditorTreeView.BeginUpdate();

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            try
            {

                this.EditorPropertyGrid.SelectedObject = null;
                manager.Switch(index1, index2);
                string str = this.GenerateEndCommand(eCommandType.move_collection_up, this.EditorTreeView.SelectedNode.FullPath);
                this.WriteLineToEndCommandPrompt(str);
                Utils.MoveUp(this.EditorTreeView.SelectedNode);
                this._edited = true;

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            this.EditorTreeView.EndUpdate();
        }

        private void moveNodeDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;
            var index1 = this.EditorTreeView.SelectedNode.Index;
            var index2 = index1 + 1;

            if (index2 >= this.EditorTreeView.SelectedNode.Parent.Nodes.Count)
            {

                MessageBox.Show("Unable to move down because selected node is the down most node",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            this.EditorTreeView.BeginUpdate();

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            try
            {

                this.EditorPropertyGrid.SelectedObject = null;
                manager.Switch(index1, index2);
                string str = this.GenerateEndCommand(eCommandType.move_collection_down, this.EditorTreeView.SelectedNode.FullPath);
                this.WriteLineToEndCommandPrompt(str);
                Utils.MoveDown(this.EditorTreeView.SelectedNode);
                this._edited = true;

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            this.EditorTreeView.EndUpdate();
        }

        private void moveNodeFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;
            var index1 = this.EditorTreeView.SelectedNode.Index;

            this.EditorTreeView.BeginUpdate();

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            try
            {

                this.EditorPropertyGrid.SelectedObject = null;
                for (int index2 = index1 - 1; index2 >= 0; index1--, index2--)
                {
                    manager.Switch(index1, index2);
                    Utils.MoveUp(this.EditorTreeView.SelectedNode);
                }
                string str = this.GenerateEndCommand(eCommandType.move_collection_first, this.EditorTreeView.SelectedNode.FullPath);
                this.WriteLineToEndCommandPrompt(str);

                this._edited = true;

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            this.EditorTreeView.EndUpdate();
        }

        private void moveNodeLastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fname = this.EditorTreeView.SelectedNode.Parent.Parent.Text;
            string mname = this.EditorTreeView.SelectedNode.Parent.Text;
            string cname = this.EditorTreeView.SelectedNode.Text;
            var index1 = this.EditorTreeView.SelectedNode.Index;

            this.EditorTreeView.BeginUpdate();

            var sdb = this.Profile.Find(_ => _.Filename == fname);
            var manager = sdb.Database.GetManager(mname);

            try
            {

                this.EditorPropertyGrid.SelectedObject = null;
                for (int index2 = index1 + 1; index2 < this.EditorTreeView.SelectedNode.Parent.Nodes.Count; index1++, index2++)
                {
                    manager.Switch(index1, index2);
                    Utils.MoveDown(this.EditorTreeView.SelectedNode);
                }
                string str = this.GenerateEndCommand(eCommandType.move_collection_last, this.EditorTreeView.SelectedNode.FullPath);
                this.WriteLineToEndCommandPrompt(str);
                this._edited = true;

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            this.EditorTreeView.EndUpdate();
        }

        private void RecursiveNodeSelection(string path, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {

                if (node.FullPath == path)
                {

                    this.EditorTreeView.SelectedNode = node;
                    this.EditorTreeView.SelectedNode.EnsureVisible();
                    return;

                }
                else
                {

                    this.RecursiveNodeSelection(path, node.Nodes);

                }

            }
        }

        private void RecursiveTreeHiglights(string match, TreeNodeCollection nodes)
        {

            foreach (TreeNode node in nodes)
            {

                node.BackColor = String.IsNullOrEmpty(match) || !node.Text.Contains(match, StringComparison.OrdinalIgnoreCase)
                    ? this.EditorTreeView.BackColor
                    : this.HighlightColor1;

                if (node.Nodes.Count > 0)
                {
                    this.RecursiveTreeHiglights(match, node.Nodes);
                }
            }

        }

        #endregion

        #region Loading

        private void LoadProfile(string filename, bool showerrors)
        {
#if !DEBUG
            try
            {
#endif

                this._edited = false;
                Launch.Deserialize(filename, out var launch);
                launch.ThisDir = Path.GetDirectoryName(filename);

                FixLaunchDirectory(launch, filename);

                if (launch.UsageID != eUsage.Modder)
                {

                    throw new Exception($"Usage type of the endscript is stated to be {launch.Usage}, while should be Modder");

                }

                if (launch.GameID == GameINT.None)
                {

                    throw new Exception($"Invalid stated game type named {launch.Game}");

                }

                if (!Directory.Exists(launch.Directory))
                {

                    throw new DirectoryNotFoundException($"Directory named {launch.Directory} does not exist");

                }

                this.EditorPropertyGrid.SelectedObject = null;
                this.Profile = BaseProfile.NewProfile(launch.GameID, launch.Directory);
                this.EditorStatusLabel.Text = "Loading... Please wait...";

                var watch = new Stopwatch();
                watch.Start();

                string[] exceptions = this.Profile.Load(launch);

                watch.Stop();

                foreach (string exception in exceptions)
                {

                    MessageBox.Show(exception, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                this.EditorStatusLabel.Text = Utils.GetStatusString(launch.Files.Count, watch.ElapsedMilliseconds, filename, "Loading");
                this.LoadTreeView();
                this.ToggleControlsAfterLoad(true);

                if (Configurations.Default.AutoBackups)
                {
                    this.CreateBackupsForFiles(false);
                }

                Configurations.Default.LaunchFile = filename;
                Configurations.Default.CurrentGame = (int)this.Profile.GameINT;
                Configurations.Default.Save();
                this.Text = $"Binarius - v{this.ProductVersion} - {this.Profile.GameSTR}";
                this.ToggleTheme();

#if !DEBUG
            }
            catch (Exception e)
            {

                if (showerrors)
                {

                    MessageBox.Show(e.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                this.ToggleControlsAfterLoad(false);

            }
#endif
        }

        public static void FixLaunchDirectory(Launch launch, string filename)
        {
            string directory = Path.GetDirectoryName(filename);

            try
            {
                if (Path.IsPathRooted(launch.Directory))
                {
                    return;
                }

                string maybePath = Path.GetFullPath(Path.Combine(directory, launch.Directory));

                if (Directory.Exists(maybePath))
                {
                    launch.Directory = maybePath;
                }
            }
            catch { }
        }

        private void LoadTreeView(string selected = null)
        {
            this.EditorTreeView.BeginUpdate();

            this.EditorTreeView.Nodes.Clear();
            this.Profile.Sort();

            foreach (var sdb in this.Profile)
            {
                var n = Utils.GetTreeNodesFromSDB(sdb);
                this.EditorTreeView.Nodes.Add(n);
                Utils.toggleEmptyManagersInSDB(sdb, n);
                //_ = this.EditorTreeView.Nodes.Add(Utils.GetTreeNodesFromSDB(sdb));

            }

            if (!String.IsNullOrEmpty(selected))
            {

                this.RecursiveNodeSelection(selected, this.EditorTreeView.Nodes);

            }
            else
            {

                this.ManageButtonOpenEditor(null);
                this.ManageButtonAddNode(null);
                this.ManageButtonRemoveNode(null);
                this.ManageButtonCopyNode(null);
                this.ManageButtonExportNode(null);
                this.ManageButtonImportNode(null);
                this.ManageButtonScriptNode(null);
                this.ManageButtonMoveNode(null);

            }

            this.EditorTreeView.EndUpdate();

            this.EditorTreeView.SelectedNode?.EnsureVisible();
        }

        #endregion

        #region Saving

        private void SaveProfile()
        {
#if !DEBUG
            try
            {
#endif

                this.EditorStatusLabel.Text = "Saving... Please wait...";
                var watch = new Stopwatch();
                watch.Start();

                string[] exceptions = this.Profile.Save();

                watch.Stop();

                foreach (string exception in exceptions)
                {

                    MessageBox.Show(exception, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                string filename = Configurations.Default.LaunchFile;
                this.EditorStatusLabel.Text = Utils.GetStatusString(this.Profile.Count, watch.ElapsedMilliseconds, filename, "Saving");
                this._edited = false;

#if !DEBUG
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
#endif
        }

        #endregion

        #region TreeView Controls

        private void EditorTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.EditorTreeView.SelectedNode != null)
            {

                this.EditorTreeView.SelectedNode.ForeColor = this.EditorTreeView.ForeColor;

                e.Node.ForeColor = this.HighlightColor2;

            }
            else
            {

                e.Node.ForeColor = this.HighlightColor3;

            }
        }

        private void EditorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Console.WriteLine(e.Node.FullPath);
            var selected = Utils.GetReflective(e.Node.FullPath, this.EditorTreeView.PathSeparator, this.Profile);

            this.ManageButtonOpenEditor(selected);
            this.ManageButtonAddNode(e.Node);
            this.ManageButtonRemoveNode(e.Node);
            this.ManageButtonCopyNode(e.Node);
            this.ManageButtonExportNode(e.Node);
            this.ManageButtonImportNode(e.Node);
            this.ManageButtonScriptNode(e.Node);
            this.ManageButtonMoveNode(e.Node);

            this.EditorPropertyGrid.SelectedObject = selected;
            this.EditorNodeInfo.Text = $"| Index: {e.Node.Index} | {e.Node.Nodes.Count} subnodes";
        }

        private void EditorTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (this.openEditorToolStripMenuItem.Enabled)
            {

                this.EditorButtonOpenEditor_Click(null, EventArgs.Empty);

            }
        }

        private void EditorTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) EditorTreeView.SelectedNode = EditorTreeView.GetNodeAt(e.X, e.Y);
        }

        #endregion

        #region PropertyGrid Controls

        private void WriteLineToEndCommandPrompt(string line)
        {
            if (String.IsNullOrEmpty(this.EditorCommandPrompt.Text) ||
                this.EditorCommandPrompt.Text.EndsWith(Environment.NewLine))
            {

                this.EditorCommandPrompt.Text += line;

            }
            else
            {

                this.EditorCommandPrompt.Text += Environment.NewLine + line;

            }

            if (!this.EditorCommandPrompt.Text.EndsWith(Environment.NewLine))
            {

                this.EditorCommandPrompt.Text += Environment.NewLine;

            }

            this.EditorCommandPrompt.SelectionStart = this.EditorCommandPrompt.Text.Length - 1;
            this.EditorCommandPrompt.ScrollToCaret();
        }

        private void WriteLineToEndCommandPrompt(IEnumerable<string> lines)
        {
            if (lines == null || lines.Count() == 0)
            {
                return;
            }

            int size = lines.Aggregate(0, (result, str) => result += str.Length + 2);
            var sb = new StringBuilder(size);

            foreach (string line in lines)
            {

                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                _ = sb.Append(line + Environment.NewLine);

            }

            this.WriteLineToEndCommandPrompt(sb.ToString());
        }

        private string GetCurrentSeparatedPath()
        {
            string line = String.Empty;
            if (this.EditorTreeView.SelectedNode == null)
            {
                return line;
            }

            string[] splits = this.EditorTreeView.SelectedNode.FullPath.Split(this.EditorTreeView.PathSeparator);

            for (int loop = 0; loop < splits.Length - 1; ++loop)
            {

                string split = splits[loop];
                line += split.Contains(' ') ? $"\"{split}\"" + space : split + space;

            }

            line += splits[^1].Contains(' ') ? $"\"{splits[^1]}\"" : splits[^1];
            return line;
        }

        private string GenerateEndCommand(eCommandType type, string nodepath)
        {
            string line = type.ToString() + space;
            string[] splits = nodepath.Split(this.EditorTreeView.PathSeparator);

            for (int loop = 0; loop < splits.Length - 1; ++loop)
            {

                string split = splits[loop];
                line += split.Contains(' ') ? $"\"{split}\"" + space : split + space;

            }

            line += splits[^1].Contains(' ') ? $"\"{splits[^1]}\"" : splits[^1];
            return line;
        }

        private string GenerateEndCommand(eCommandType type, string nodepath, params string[] args)
        {
            string line = type.ToString() + space;
            string[] splits = nodepath.Split(this.EditorTreeView.PathSeparator);

            foreach (string split in splits)
            {

                line += split.Contains(' ') ? $"\"{split}\"" + space : split + space;

            }

            for (int loop = 0; loop < args.Length - 1; ++loop)
            {

                string arg = args[loop];
                line += arg.Contains(' ') ? $"\"{arg}\"" + space : arg + space;

            }

            line += args[^1].Contains(' ') ? $"\"{args[^1]}\"" : args[^1];
            return line;
        }

        private void EditorPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            string value = e.ChangedItem.Value.ToString();
            if (String.IsNullOrEmpty(value))
            {
                value = empty;
            }

            string str = this.GenerateEndCommand(eCommandType.update_collection, this.EditorTreeView.SelectedNode.FullPath,
                e.ChangedItem.Label, value);

            this.WriteLineToEndCommandPrompt(str);
            this._edited = true;

            if (e.ChangedItem.Label == "CollectionName")
            {

                this.EditorTreeView.SelectedNode.Text = e.ChangedItem.Value.ToString();
                this.EditorPropertyGrid.Refresh();

            }
        }

        private void EditorFindTextBox_TextChanged(object sender, EventArgs e) => this.RecursiveTreeHiglights(this.EditorFindTextBox.Text, this.EditorTreeView.Nodes);

        #endregion

        #region Editor Main

        private void Editor_Load(object sender, EventArgs e)
        {
            string file = Configurations.Default.LaunchFile;
            if (!File.Exists(file))
            {
                return;
            }
            else
            {
                this.LoadProfile(file, false);
            }
        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._edited)
            {

                var result = MessageBox.Show("You have unsaved changes. Are you sure you want to quit the editor?",
                    "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) { e.Cancel = true; return; }

            }

            this.Profile = null;

            foreach (var form in this._openforms)
            {

                try { form.Close(); }
                catch { }

            }
        }

        #endregion

        #region Exception

        internal void EmergencySaveDatabase()
        {
            string date = DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");

            string backup = $"DB_{date}";
            _ = Directory.CreateDirectory(backup);
            string end = Path.Combine(backup, "info.end");

            var serializer = new EndSerializer(this.Profile, end);
            serializer.Serialize();
        }

        #endregion



        
    }
}
