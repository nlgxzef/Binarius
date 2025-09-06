using Binary.Properties;

using Endscript.Core;

using Nikki.Reflection.Abstract;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;



namespace Binary.Interact
{
    public partial class LanMakerFileSelector : Form
    {
        private bool IsValidDirectoryChosen => Directory.Exists(this.RootPath);
        public int IconIndex { get; private set; }
        public bool WasCreated { get; private set; }
        public string RootPath { get; private set; }
        public List<string> Files { get; set; }

        public LanMakerFileSelector(string _rootpath) : this()
        {
            this.RootPath = _rootpath;
        }

        public LanMakerFileSelector()
        {
            this.InitializeComponent();
            this.ToggleTheme();
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;
            this.LanMakerFileSelectorButtonOK.BackColor = theme.Colors.ButtonBackColor;
            this.LanMakerFileSelectorButtonOK.ForeColor = theme.Colors.ButtonForeColor;
            this.LanMakerFileSelectorButtonOK.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.LanMakerFileSelectorButtonCancel.BackColor = theme.Colors.ButtonBackColor;
            this.LanMakerFileSelectorButtonCancel.ForeColor = theme.Colors.ButtonForeColor;
            this.LanMakerFileSelectorButtonCancel.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.FileListTreeView.BackColor = theme.Colors.MainBackColor;
            this.FileListTreeView.ForeColor = theme.Colors.MainForeColor;

            this.IconIndex = theme.DarkTheme ? 1 : 0;
        }


        private void LanMakerFileSelectorButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void LanMakerFileSelectorButtonOK_Click(object sender, EventArgs e)
        {
            this.GetNewFileList();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void GetNewFileList(TreeNode tn)
        {
            foreach (TreeNode tds in tn.Nodes)
            {
                if (tds.Nodes.Count > 0)
                {
                    this.GetNewFileList(tds);
                }
                else if (tds.Checked && tds.Tag != null && File.Exists(tds.Tag.ToString()))
                {
                    string relativePath = Path.GetRelativePath(this.RootPath, tds.Tag.ToString());
                    this.Files.Add(relativePath);
                }
            }
        }

        private void GetNewFileList()
        {
            this.Files.Clear();

            foreach (TreeNode tds in FileListTreeView.Nodes)
            {
                this.GetNewFileList(tds);
            }
        }

        private bool IsFileInList(string name)
        {
            if (this.Files == null) return false;

            foreach (var file in this.Files)
            {
                if (file.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool LoadFiles(string path, TreeNode tn)
        {
            bool expand = false;

            string[] Files = Directory.GetFiles(path, "*.*");
            // Loop through them to see files
            foreach (string file in Files)
            {
                var ext = Path.GetExtension(file).ToLower();

                if (ext == ".bin" || ext == ".bun" || ext == ".lzc") // Add editable files
                {
                    FileInfo fi = new FileInfo(file);

                    CustomTreeNode tds = new CustomTreeNode(fi.Name)
                    {
                        Tag = fi.FullName,
                        ImageIndex = this.IconIndex,
                        ShowCheckBox = true
                    };

                    tn.Nodes.Add(tds);

                    if (this.IsFileInList(Path.GetRelativePath(this.RootPath, fi.FullName)))
                    {
                        tds.Checked = true;
                        expand = true;
                    }
                }
            }

            return expand;
        }

        private void LoadDirectories(string path, TreeNode tn)
        {
            // Get all subdirectories
            string[] dirs = Directory.GetDirectories(path);
            // Loop through them to see if they have any other subdirectories
            foreach (string d in dirs)
            {
                DirectoryInfo di = new DirectoryInfo(d);
                CustomTreeNode tds = new CustomTreeNode(di.Name)
                {
                    Tag = di.FullName,
                    ShowCheckBox = false
                };

                tn.Nodes.Add(tds);

                this.LoadDirectories(d, tds);
                bool expand = this.LoadFiles(d, tds);
                if (expand)
                {
                    tds.Expand();
                }
            }
        }

        private void LanMakerFileSelector_Load(object sender, EventArgs e)
        {
            FileListTreeView.Nodes.Clear();

            if (this.IsValidDirectoryChosen)
            {
                FileListTreeView.BeginUpdate();

                CustomTreeNode root = new CustomTreeNode(this.RootPath)
                {
                    Tag = this.RootPath,
                    ShowCheckBox = false
                };

                FileListTreeView.Nodes.Add(root); // Add root node

                // Recursively add directories and files
                this.LoadDirectories(this.RootPath, root);
                root.Expand();
                FileListTreeView.EndUpdate();
            }
            else
            {
                MessageBox.Show("The specified directory is invalid. Please select a valid directory.", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
