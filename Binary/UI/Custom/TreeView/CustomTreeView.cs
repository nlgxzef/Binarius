using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
    public partial class CustomTreeView : TreeView
    {
        public CustomTreeView()
        {
            this.InitializeComponent();
            this.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.FullRowSelect = false;
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            CustomTreeNode n = e.Node as CustomTreeNode;
            if (n == null) { e.DrawDefault = true; return; }

            Rectangle rect = new Rectangle(e.Bounds.Location,
                             new Size(this.ClientSize.Width, e.Bounds.Height));
            CheckBoxState cs = n.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
            Size glyph = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.CheckedNormal);

            int offset = n.ShowCheckBox ? glyph.Width + 2 : 0;

            CheckBoxRenderer.DrawParentBackground(e.Graphics, e.Bounds, this);
            e.Graphics.DrawString(n.Text, this.Font, new SolidBrush(this.ForeColor),
                                    e.Bounds.X + offset, e.Bounds.Y);
            
            if (n.ShowCheckBox)
            {
                CheckBoxRenderer.DrawCheckBox(
                    e.Graphics,
                    new Rectangle(e.Bounds.Left + 2, e.Bounds.Y + 2, glyph.Width, glyph.Height).Location,
                    cs);
            }
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            CustomTreeNode n = e.Node as CustomTreeNode;
            if (n == null) { return; }

            if (n.ShowCheckBox)
            {
                n.Checked = !n.Checked;

                this.Invalidate();
            }
        }
    }
}
