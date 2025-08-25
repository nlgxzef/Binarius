using Binary.Properties;

using CoreExtensions.Management;

using Nikki.Support.Shared.Class;

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;



namespace Binary.UI
{
    public partial class VectorEditor : Form
    {
        private VectorVinyl Vector { get; }

        public VectorEditor(VectorVinyl vinyl)
        {
            this.InitializeComponent();
            this.ToggleTheme();
            this.Vector = vinyl;
            this.Text = $"{this.Vector.CollectionName} Editor";
            this.LoadTreeView();
            this.ToggleMenuStripControls(null);
        }

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            // Renderers
            this.VectorMenuStrip.Renderer = new Theme.MenuStripRenderer();

            // Primary colors and controls
            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;

            // Tree view
            this.VectorTreeView.BackColor = theme.Colors.PrimBackColor;
            this.VectorTreeView.ForeColor = theme.Colors.PrimForeColor;

            // Property grid
            this.VectorPropertyGrid.BackColor = theme.Colors.PrimBackColor;
            this.VectorPropertyGrid.CategorySplitterColor = theme.Colors.ButtonBackColor;
            this.VectorPropertyGrid.CategoryForeColor = theme.Colors.TextBoxForeColor;
            this.VectorPropertyGrid.CommandsBackColor = theme.Colors.PrimBackColor;
            this.VectorPropertyGrid.CommandsForeColor = theme.Colors.PrimForeColor;
            this.VectorPropertyGrid.CommandsBorderColor = theme.Colors.PrimBackColor;
            this.VectorPropertyGrid.DisabledItemForeColor = theme.Colors.LabelTextColor;
            this.VectorPropertyGrid.LineColor = theme.Colors.ButtonBackColor;
            this.VectorPropertyGrid.SelectedItemWithFocusBackColor = theme.Colors.FocusedBackColor;
            this.VectorPropertyGrid.SelectedItemWithFocusForeColor = theme.Colors.FocusedForeColor;
            this.VectorPropertyGrid.ViewBorderColor = theme.Colors.RegBorderColor;
            this.VectorPropertyGrid.ViewBackColor = theme.Colors.PrimBackColor;
            this.VectorPropertyGrid.ViewForeColor = theme.Colors.PrimForeColor;
            this.VectorPropertyGrid.HelpBackColor = theme.Colors.PrimBackColor;
            this.VectorPropertyGrid.HelpForeColor = theme.Colors.PrimForeColor;
            this.VectorPropertyGrid.HelpBorderColor = theme.Colors.RegBorderColor;

            // Menu strip and menu items
            this.VectorMenuStrip.ForeColor = theme.Colors.LabelTextColor;
            this.ImportSVGToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.ImportSVGToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.ExportSVGToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.ExportSVGToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.PreviewToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.PreviewToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.AddPathSetToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.AddPathSetToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.RemovePathSetToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.RemovePathSetToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.MoveUpPathSetToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.MoveUpPathSetToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
            this.MoveDownPathSetToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
            this.MoveDownPathSetToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
        }

        #endregion

        #region Methods

        private object GetSelectedObject(TreeNode node)
        {
            if (node is null || node.Level == 1)
            {

                return null;

            }
            else if (node.Level == 0)
            {

                return this.Vector.GetPathSet(node.Index);

            }
            else if (node.Level == 2)
            {

                if (node.Parent.Text == "PathDatas")
                {

                    var set = this.Vector.GetPathSet(node.Parent.Parent.Index);
                    return set.PathDatas[node.Index];

                }
                else if (node.Parent.Text == "PathPoints")
                {

                    var set = this.Vector.GetPathSet(node.Parent.Parent.Index);
                    return set.PathPoints[node.Index];

                }

            }

            return null;
        }

        private void LoadTreeView(string selected = null)
        {
            this.VectorTreeView.Nodes.Clear();
            this.VectorTreeView.BeginUpdate();
            var nodes = new TreeNode[this.Vector.NumberOfPaths];

            for (int i = 0; i < this.Vector.NumberOfPaths; ++i)
            {
                _ = this.Vector.GetPathSet(i);
                var setnode = new TreeNode($"PathSet{i}");
                nodes[i] = setnode;

            }

            this.VectorTreeView.Nodes.AddRange(nodes);
            this.VectorTreeView.EndUpdate();

            if (!String.IsNullOrEmpty(selected))
            {

                this.RecursiveNodeSelection(selected, this.VectorTreeView.Nodes);

            }
        }

        private void RecursiveNodeSelection(string path, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {

                if (node.FullPath == path)
                {

                    this.VectorTreeView.SelectedNode = node;
                    return;

                }
                else
                {

                    this.RecursiveNodeSelection(path, node.Nodes);

                }

            }
        }

        private void ToggleMenuStripControls(TreeNode node)
        {
            this.ImportSVGToolStripMenuItem.Enabled = true;
            this.ExportSVGToolStripMenuItem.Enabled = true;
            this.PreviewToolStripMenuItem.Enabled = true;
            this.AddPathSetToolStripMenuItem.Enabled = true;

            if (node is null)
            {

                this.RemovePathSetToolStripMenuItem.Enabled = false;
                this.MoveUpPathSetToolStripMenuItem.Enabled = false;
                this.MoveDownPathSetToolStripMenuItem.Enabled = false;

            }
            else
            {

                this.MoveUpPathSetToolStripMenuItem.Enabled = true;
                this.RemovePathSetToolStripMenuItem.Enabled = true;
                this.MoveDownPathSetToolStripMenuItem.Enabled = true;

            }
        }

        #endregion

        #region Menu Strip

        private void ImportSVGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var browser = new OpenFileDialog()
            {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Scalable Vector Graphics Files | *.svg",
                Multiselect = false,
                Title = "Select .svg file to import",
            };

            if (browser.ShowDialog() == DialogResult.OK)
            {

#if !DEBUG
				try
				{
#endif

                this.Vector.ReadFromFile(browser.FileName);
                this.VectorPropertyGrid.SelectedObject = null;
                this.LoadTreeView();
                MessageBox.Show($"File {browser.FileName} has been successfully imported.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

#if !DEBUG
				}
				catch (Exception ex)
				{

					MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				}
#endif

            }
        }

        private void ExportSVGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] options = new string[]
            {
                "512x512",
                "1024x1024",
                "2048x2048",
                "4096x4096",
                "8192x8192",
                "16384x16384",
                "32768x32768",
                "65536x65536",
            };

            int[] resolutions = new int[]
            {
                512,
                1024,
                2048,
                4096,
                8192,
                16384,
                32768,
                65536,
            };

            string description = "Select resolution in which vector vinyl should be exported";

            using var selection = new Prompt.Combo(options, description, false);

            if (selection.ShowDialog() == DialogResult.OK)
            {

                using var dialog = new SaveFileDialog()
                {
                    AddExtension = true,
                    AutoUpgradeEnabled = true,
                    CheckPathExists = true,
                    DefaultExt = ".svg",
                    Filter = "Scalable Vector Graphics Files|*.svg|Any Files|*.*",
                    FileName = this.Vector.CollectionName,
                    OverwritePrompt = true,
                    SupportMultiDottedExtensions = true,
                    Title = "Select filename where vector should be exported",
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    try
                    {

                        string svg = this.Vector.GetSVGString(resolutions[selection.Value]);
                        File.WriteAllText(dialog.FileName, svg);
                        _ = MessageBox.Show($"Vector {this.Vector.CollectionName} has been successfully exported.", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch (Exception ex)
                    {

                        _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }

            }
        }

        private void PreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string svg = this.Vector.GetSVGString(1024);
            string dir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string file = Path.Combine(dir, "vectordev.html");
            File.WriteAllText(file, svg);

            try { _ = Process.Start(new ProcessStartInfo($"\"{file}\"") { UseShellExecute = true }); }
            catch (Exception ex) { _ = MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void AddPathSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = this.Vector.NumberOfPaths;
            this.Vector.AddPathSet();
            _ = this.VectorTreeView.Nodes.Add($"PathSet{count}");
        }

        private void RemovePathSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.VectorTreeView.SelectedNode is null)
            {
                return;
            }

            this.VectorPropertyGrid.SelectedObject = null;
            this.Vector.RemovePathSet(this.VectorTreeView.SelectedNode.Index);
            this.VectorTreeView.Nodes.RemoveAt(this.VectorTreeView.SelectedNode.Index);
        }

        private void MoveUpPathSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.VectorTreeView.SelectedNode is null)
            {
                return;
            }

            _ = this.VectorTreeView.SelectedNode.FullPath;
            int index1 = this.VectorTreeView.SelectedNode.Index;
            int index2 = index1 - 1;

            if (index2 < 0)
            {

                _ = MessageBox.Show("Unable to move up because selected node is the up most node",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            // Switch parts
            this.VectorTreeView.BeginUpdate();
            this.Vector.SwitchPaths(index1, index2);

            var node1 = this.VectorTreeView.Nodes[index1];
            var node2 = this.VectorTreeView.Nodes[index2];

            node1.Text = $"PathSet{index1}";
            node2.Text = $"PathSet{index2}";

            this.VectorTreeView.SelectedNode = this.VectorTreeView.Nodes[index2];
            this.VectorTreeView.EndUpdate();
        }

        private void MoveDownPathSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.VectorTreeView.SelectedNode is null)
            {
                return;
            }

            _ = this.VectorTreeView.SelectedNode.FullPath;
            int index1 = this.VectorTreeView.SelectedNode.Index;
            int index2 = index1 + 1;

            if (index2 >= this.Vector.NumberOfPaths)
            {

                _ = MessageBox.Show("Unable to move down because selected node is the down most node",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            // Switch parts
            this.VectorTreeView.BeginUpdate();
            this.Vector.SwitchPaths(index1, index2);

            var node1 = this.VectorTreeView.Nodes[index1];
            var node2 = this.VectorTreeView.Nodes[index2];

            node1.Text = $"PathSet{index1}";
            node2.Text = $"PathSet{index2}";

            this.VectorTreeView.SelectedNode = this.VectorTreeView.Nodes[index2];
            this.VectorTreeView.EndUpdate();
        }

        #endregion

        #region TreeView and Grid

        private void VectorTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.VectorTreeView.SelectedNode != null)
            {

                this.VectorTreeView.SelectedNode.ForeColor = this.VectorTreeView.ForeColor;

                e.Node.ForeColor = Configurations.Default.DarkTheme
                    ? Color.FromArgb(255, 230, 0)
                    : Color.FromArgb(255, 20, 20);

            }
            else
            {

                e.Node.ForeColor = Configurations.Default.DarkTheme
                    ? Color.FromArgb(255, 230, 0)
                    : Color.FromArgb(255, 90, 0);

            }
        }

        private void VectorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Console.WriteLine(e.Node?.FullPath);
            object selected = this.GetSelectedObject(e.Node);
            this.ToggleMenuStripControls(e.Node);
            this.VectorPropertyGrid.SelectedObject = selected;
        }

        private void VectorPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
        }

        #endregion
    }
}
