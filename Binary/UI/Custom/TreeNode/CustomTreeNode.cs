using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class CustomTreeNode : TreeNode
    {
        public bool ShowCheckBox { get; set; }

        public CustomTreeNode() : this("") { }

        public CustomTreeNode(string text) : base(text)
        {
            this.ShowCheckBox = false;
            this.Checked = false;
        }
    }
}
