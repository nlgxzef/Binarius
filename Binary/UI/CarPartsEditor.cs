using Binary.Interact;
using Binary.Prompt;
using Binary.Properties;
using Binary.Tools;

using Nikki.Core;
using Nikki.Support.Shared.Class;
using Nikki.Support.Shared.Parts.CarParts;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;



namespace Binary.UI
{
	public partial class CarPartsEditor : Form
	{
		private DBModelPart Model { get; }
		private readonly List<Form> _openforms;

		public CarPartsEditor(DBModelPart model)
		{
			this._openforms = new List<Form>();
			this.InitializeComponent();
			this.ToggleTheme();
			this.Model = model;
			this.Text = $"{this.Model.CollectionName} Editor";
			this.LoadTreeView();
			this.ToggleMenuStripControls(null);

			this.FindAndReplaceToolStripMenuItem.Enabled = true;
		}

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            // Renderers
            this.CarPartsEditorMenuStrip.Renderer = new Theme.MenuStripRenderer();

			// Primary colors and controls
			this.BackColor = theme.Colors.MainBackColor;
			this.ForeColor = theme.Colors.MainForeColor;

			// Tree view
			this.CarPartsTreeView.BackColor = theme.Colors.PrimBackColor;
			this.CarPartsTreeView.ForeColor = theme.Colors.PrimForeColor;

			// Property grid
			this.CarPartsPropertyGrid.BackColor = theme.Colors.PrimBackColor;
			this.CarPartsPropertyGrid.CategorySplitterColor = theme.Colors.ButtonBackColor;
			this.CarPartsPropertyGrid.CategoryForeColor = theme.Colors.TextBoxForeColor;
			this.CarPartsPropertyGrid.CommandsBackColor = theme.Colors.PrimBackColor;
			this.CarPartsPropertyGrid.CommandsForeColor = theme.Colors.PrimForeColor;
			this.CarPartsPropertyGrid.CommandsBorderColor = theme.Colors.PrimBackColor;
			this.CarPartsPropertyGrid.DisabledItemForeColor = theme.Colors.LabelTextColor;
			this.CarPartsPropertyGrid.LineColor = theme.Colors.ButtonBackColor;
			this.CarPartsPropertyGrid.SelectedItemWithFocusBackColor = theme.Colors.FocusedBackColor;
			this.CarPartsPropertyGrid.SelectedItemWithFocusForeColor = theme.Colors.FocusedForeColor;
			this.CarPartsPropertyGrid.ViewBorderColor = theme.Colors.RegBorderColor;
			this.CarPartsPropertyGrid.ViewBackColor = theme.Colors.PrimBackColor;
			this.CarPartsPropertyGrid.ViewForeColor = theme.Colors.PrimForeColor;
			this.CarPartsPropertyGrid.HelpBackColor = theme.Colors.PrimBackColor;
			this.CarPartsPropertyGrid.HelpForeColor = theme.Colors.PrimForeColor;
			this.CarPartsPropertyGrid.HelpBorderColor = theme.Colors.RegBorderColor;

			// Menu strip and menu items
			this.CarPartsEditorMenuStrip.ForeColor = theme.Colors.LabelTextColor;
			this.AddPartToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.AddPartToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.RemovePartToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.RemovePartToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.CopyPartToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.CopyPartToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.MoveUpPartsToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.MoveUpPartsToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.MoveDownPartsToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.MoveDownPartsToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.MoveFirstPartsToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.MoveFirstPartsToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.MoveLastPartsToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.MoveLastPartsToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.ReversePartsToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.ReversePartsToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.SortPartsByNameToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.SortPartsByNameToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.FindAndReplaceToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.FindAndReplaceToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.AddAttributeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.AddAttributeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.RemoveAttributeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;			
			this.RemoveAttributeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.MoveUpAttributesToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;			
			this.MoveUpAttributesToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.MoveDownAttributesToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;			
			this.MoveDownAttributesToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.ReverseAttributesToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;			
			this.ReverseAttributesToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.SortAttributesByKeyToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;			
			this.SortAttributesByKeyToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.AddCustomAttributeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.AddCustomAttributeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.HasherToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.HasherToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.RaiderToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.RaiderToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
		}

		#endregion

		#region Methods

		private object GetSelectedObject(TreeNode node)
		{
			// Since there are only 2 levels overall, check node level
			if (node is null)
			{

				return null;

			}
			else if (node.Level == 0)
			{

				return this.Model.GetRealPart(node.Index);

			}
			else
			{

				var index = node.Parent.Index;
				return this.Model.GetRealPart(index).GetAttribute(node.Index);

			}
		}

		private void LoadTreeView(string selected = null)
		{
			this.CarPartsTreeView.Nodes.Clear();
			this.CarPartsTreeView.BeginUpdate();
			var nodes = new TreeNode[this.Model.CarPartsCount];
			int count = 0;

			foreach (var realpart in this.Model.ModelCarParts)
			{

				var level0 = new TreeNode(realpart.PartName);

				foreach (var attribute in realpart.Attributes)
				{

					var level1 = new TreeNode(attribute.ToString());
					level0.Nodes.Add(level1);

				}

				nodes[count++] = level0;

			}

			this.CarPartsTreeView.Nodes.AddRange(nodes);
			this.CarPartsTreeView.EndUpdate();

			if (!String.IsNullOrEmpty(selected))
			{

				this.RecursiveNodeSelection(selected, this.CarPartsTreeView.Nodes);

			}
		}

		private void RecursiveNodeSelection(string path, TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{

				if (node.FullPath == path)
				{

					this.CarPartsTreeView.SelectedNode = node;
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

				node.BackColor = String.IsNullOrEmpty(match) || !node.Text.Contains(match)
					? this.CarPartsTreeView.BackColor
					: Configurations.Default.DarkTheme
						? Color.FromArgb(160, 20, 30)
						: Color.FromArgb(60, 255, 60);

				if (node.Nodes.Count > 0) this.RecursiveTreeHiglights(match, node.Nodes);

			}

		}

		private void ToggleMenuStripControls(TreeNode node)
		{
			if (node is null)
			{

				this.AddPartToolStripMenuItem.Enabled = true;
				this.RemovePartToolStripMenuItem.Enabled = false;
				this.CopyPartToolStripMenuItem.Enabled = false;
				this.MoveUpPartsToolStripMenuItem.Enabled = false;
				this.MoveDownPartsToolStripMenuItem.Enabled = false;
				this.MoveFirstPartsToolStripMenuItem.Enabled = false;
				this.MoveLastPartsToolStripMenuItem.Enabled = false;
				this.ReversePartsToolStripMenuItem.Enabled = false;
				this.SortPartsByNameToolStripMenuItem.Enabled = false;
				this.AddAttributeToolStripMenuItem.Enabled = false;
				this.RemoveAttributeToolStripMenuItem.Enabled = false;
				this.MoveUpAttributesToolStripMenuItem.Enabled = false;
				this.MoveDownAttributesToolStripMenuItem.Enabled = false;
				this.ReverseAttributesToolStripMenuItem.Enabled = false;
				this.SortAttributesByKeyToolStripMenuItem.Enabled = false;
				this.AddCustomAttributeToolStripMenuItem.Enabled = false;

			}
			else
			{

				this.AddPartToolStripMenuItem.Enabled = node.Level == 0;
				this.RemovePartToolStripMenuItem.Enabled = node.Level == 0;
				this.CopyPartToolStripMenuItem.Enabled = node.Level == 0;
				this.MoveUpPartsToolStripMenuItem.Enabled = node.Level == 0;
				this.MoveDownPartsToolStripMenuItem.Enabled = node.Level == 0;
				this.MoveFirstPartsToolStripMenuItem.Enabled = node.Level == 0;
				this.MoveLastPartsToolStripMenuItem.Enabled = node.Level == 0;
				this.ReversePartsToolStripMenuItem.Enabled = node.Level == 0;
				this.SortPartsByNameToolStripMenuItem.Enabled = node.Level == 0;
				this.AddAttributeToolStripMenuItem.Enabled = node.Level == 0;
				this.RemoveAttributeToolStripMenuItem.Enabled = node.Level == 1;
				this.MoveUpAttributesToolStripMenuItem.Enabled = node.Level == 1;
				this.MoveDownAttributesToolStripMenuItem.Enabled = node.Level == 1;
				this.ReverseAttributesToolStripMenuItem.Enabled = node.Level == 0;
				this.SortAttributesByKeyToolStripMenuItem.Enabled = node.Level == 0;
				this.AddCustomAttributeToolStripMenuItem.Enabled = node.Level == 0;

			}
		}

		#endregion

		#region Menu Strip Controls

		private void AddPartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node or no node at all

			this.Model.AddRealPart();
			this.CarPartsTreeView.Nodes.Add(this.Model.GetLastPart().PartName);
		}

		private void RemovePartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node

			var index = this.CarPartsTreeView.SelectedNode.Index;
			this.Model.RemovePart(index);
			this.CarPartsTreeView.Nodes.RemoveAt(index);

			if (this.CarPartsTreeView.Nodes.Count == 0)
			{

				this.ToggleMenuStripControls(this.CarPartsTreeView.SelectedNode);
				this.CarPartsPropertyGrid.SelectedObject = null;

			}
		}

		private void CopyPartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node

			var count = this.Model.ModelCarParts.Count;
			var index = this.CarPartsTreeView.SelectedNode.Index;
			this.Model.ClonePart(index);

			var realpart = this.Model.GetLastPart();
			this.Model.ModelCarParts.RemoveAt(count);
			this.Model.ModelCarParts.Insert(index + 1, realpart);

			var level0 = new TreeNode(realpart.PartName);

			foreach (var attribute in realpart.Attributes)
			{

				var level1 = new TreeNode(attribute.ToString());
				level0.Nodes.Add(level1);

			}

			this.CarPartsTreeView.Nodes.Insert(index + 1, level0);
		}

		private void MoveUpPartsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node

			var path = this.CarPartsTreeView.SelectedNode.FullPath;
			var index1 = this.CarPartsTreeView.SelectedNode.Index;
			var index2 = index1 - 1;

			if (index2 < 0)
			{

				MessageBox.Show("Unable to move up because selected node is the up most node",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;

			}

			// Switch parts
			this.CarPartsTreeView.BeginUpdate();

			var temp = this.Model.ModelCarParts[index1];
			this.Model.ModelCarParts[index1] = this.Model.ModelCarParts[index2];
			this.Model.ModelCarParts[index2] = temp;

			var node1 = this.CarPartsTreeView.Nodes[index1];
			var node2 = this.CarPartsTreeView.Nodes[index2];

			node1.Text = this.Model.ModelCarParts[index1].PartName;
			node2.Text = this.Model.ModelCarParts[index2].PartName;

			node1.Nodes.Clear();
			node2.Nodes.Clear();

			foreach (var attribute in this.Model.ModelCarParts[index1].Attributes)
			{

				node1.Nodes.Add(new TreeNode(attribute.ToString()));

			}

			foreach (var attribute in this.Model.ModelCarParts[index2].Attributes)
			{

				node2.Nodes.Add(new TreeNode(attribute.ToString()));

			}

			this.CarPartsTreeView.SelectedNode = this.CarPartsTreeView.Nodes[index2];
			this.CarPartsTreeView.EndUpdate();
		}

		private void MoveDownPartsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node

			var path = this.CarPartsTreeView.SelectedNode.FullPath;
			var index1 = this.CarPartsTreeView.SelectedNode.Index;
			var index2 = index1 + 1;

			if (index2 >= this.Model.CarPartsCount)
			{

				MessageBox.Show("Unable to move down because selected node is the down most node",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;

			}

			// Switch parts
			this.CarPartsTreeView.BeginUpdate();

			var temp = this.Model.ModelCarParts[index1];
			this.Model.ModelCarParts[index1] = this.Model.ModelCarParts[index2];
			this.Model.ModelCarParts[index2] = temp;

			var node1 = this.CarPartsTreeView.Nodes[index1];
			var node2 = this.CarPartsTreeView.Nodes[index2];

			node1.Text = this.Model.ModelCarParts[index1].PartName;
			node2.Text = this.Model.ModelCarParts[index2].PartName;

			node1.Nodes.Clear();
			node2.Nodes.Clear();

			foreach (var attribute in this.Model.ModelCarParts[index1].Attributes)
			{

				node1.Nodes.Add(new TreeNode(attribute.ToString()));

			}

			foreach (var attribute in this.Model.ModelCarParts[index2].Attributes)
			{

				node2.Nodes.Add(new TreeNode(attribute.ToString()));

			}

			this.CarPartsTreeView.SelectedNode = this.CarPartsTreeView.Nodes[index2];
			this.CarPartsTreeView.EndUpdate();
		}

		private void MoveFirstPartsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node

			if (this.CarPartsTreeView.Nodes.Count == 0 || this.CarPartsTreeView.Nodes.Count == 1) return;

			var path = this.CarPartsTreeView.SelectedNode.FullPath;
			var index = this.CarPartsTreeView.SelectedNode.Index;
			var part = this.Model.ModelCarParts[index];

			// Remove part
			this.Model.RemovePart(index);
			this.CarPartsTreeView.Nodes.RemoveAt(index);

			// Add to the front
			this.Model.ModelCarParts.Insert(0, part);
			this.CarPartsTreeView.Nodes.Insert(0, part.PartName);

			foreach (var attribute in part.Attributes)
			{

				this.CarPartsTreeView.Nodes[0].Nodes.Add(attribute.ToString());

			}

			this.CarPartsTreeView.SelectedNode = this.CarPartsTreeView.Nodes[0];
		}

		private void MoveLastPartsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node

			if (this.CarPartsTreeView.Nodes.Count == 0 || this.CarPartsTreeView.Nodes.Count == 1) return;

			var path = this.CarPartsTreeView.SelectedNode.FullPath;
			var index = this.CarPartsTreeView.SelectedNode.Index;
			var part = this.Model.ModelCarParts[index];

			// Remove part
			this.Model.RemovePart(index);
			this.CarPartsTreeView.Nodes.RemoveAt(index);

			// Add to the back
			this.Model.ModelCarParts.Add(part);
			this.CarPartsTreeView.Nodes.Add(part.PartName);

			foreach (var attribute in part.Attributes)
			{

				this.CarPartsTreeView.Nodes[^1].Nodes.Add(attribute.ToString());

			}

			this.CarPartsTreeView.SelectedNode = this.CarPartsTreeView.Nodes[^1];
		}

		private void ReversePartsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Model.ReverseParts();
			this.LoadTreeView(this.CarPartsTreeView.SelectedNode.FullPath);
		}

		private void SortPartsByNameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Model.SortByProperty(nameof(RealCarPart.PartName));
			this.LoadTreeView(this.CarPartsTreeView.SelectedNode.FullPath);
		}

		private void FindAndReplaceToolStripMenuItem_Click(object sende, EventArgs e)
		{
			if (this.CarPartsTreeView.Nodes.Count == 0) return;

			// 4 windows:
			//     1. String to replace with
			//     2. String to replace itself
			//     3. Do case sensitive search
			//     4. Replace only PartLabel or absolutely everything


			// 1 & 2
			using var with = new Input("Enter string to replace with");
			using var input = new Input("Enter string to search for",
										new Predicate<string>(_ => !String.IsNullOrEmpty(_)),
										"Input string cannot be null or empty");

			if (input.ShowDialog() == DialogResult.OK && with.ShowDialog() == DialogResult.OK)
			{

				// 3
				using var check = new Check("Make case-sensitive replace?", false);

				if (check.ShowDialog() == DialogResult.OK)
				{

					// Make regex options based on result of 3
					var options = check.Value
						? RegexOptions.Multiline | RegexOptions.CultureInvariant
						: RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

					// 4
					using var all = new Check("Make replacement only in PartLabel?", false, true);

					if (all.ShowDialog() == DialogResult.OK)
					{

						this.CarPartsTreeView.BeginUpdate();

						for (int i = 0; i < this.Model.CarPartsCount; ++i)
						{

							var part = this.Model.GetRealPart(i);
							part.MakeReplace(all.Value, input.Value, with.Value, options);
							this.CarPartsTreeView.Nodes[i].Text = part.PartName;

						}

						this.CarPartsTreeView.EndUpdate();
						this.CarPartsPropertyGrid.Refresh();

					}

				}

			}
		}

		private void AddAttributeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node

			using var creator = new AttribCreator(this.Model.GameINT)
			{
				StartPosition = FormStartPosition.CenterScreen
			};

			if (creator.ShowDialog() == DialogResult.OK)
			{

				var node = this.CarPartsTreeView.SelectedNode;
				var realpart = this.Model.GetRealPart(node.Index);
				realpart.AddAttribute(creator.KeyChosen);
				var attribute = realpart.Attributes[^1];
				node.Nodes.Add(attribute.ToString());
				this.CarPartsPropertyGrid.Refresh();

			}
		}

		private void RemoveAttributeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in an attribute node

			var node = this.CarPartsTreeView.SelectedNode;
			var realpart = this.Model.GetRealPart(node.Parent.Index);
			realpart.Attributes.RemoveAt(node.Index);
			this.CarPartsTreeView.SelectedNode = node.Parent;
			node.Parent.Text = realpart.PartName;
			node.Parent.Nodes.RemoveAt(node.Index);
			this.CarPartsPropertyGrid.Refresh();
		}

		private void MoveUpAttributesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in an attribute node

			var path = this.CarPartsTreeView.SelectedNode.FullPath;
			var index1 = this.CarPartsTreeView.SelectedNode.Index;
			var index2 = index1 - 1;
			var parent = this.CarPartsTreeView.SelectedNode.Parent.Index;
			var realpart = this.Model.GetRealPart(parent);

			if (index2 < 0)
			{

				MessageBox.Show("Unable to move up because selected node is the up most node",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;

			}

			// Switch attributes
			var temp = realpart.Attributes[index1];
			realpart.Attributes[index1] = realpart.Attributes[index2];
			realpart.Attributes[index2] = temp;

			this.CarPartsTreeView.Nodes[parent].Nodes[index1].Text = realpart.Attributes[index1].ToString();
			this.CarPartsTreeView.Nodes[parent].Nodes[index2].Text = realpart.Attributes[index2].ToString();

			this.CarPartsTreeView.SelectedNode = this.CarPartsTreeView.Nodes[parent].Nodes[index2];
		}

		private void MoveDownAttributesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in an attribute node

			var path = this.CarPartsTreeView.SelectedNode.FullPath;
			var index1 = this.CarPartsTreeView.SelectedNode.Index;
			var index2 = index1 + 1;
			var parent = this.CarPartsTreeView.SelectedNode.Parent.Index;
			var realpart = this.Model.GetRealPart(parent);

			if (index2 >= realpart.Length)
			{

				MessageBox.Show("Unable to move up because selected node is the up most node",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;

			}

			// Switch attributes
			var temp = realpart.Attributes[index1];
			realpart.Attributes[index1] = realpart.Attributes[index2];
			realpart.Attributes[index2] = temp;

			this.CarPartsTreeView.Nodes[parent].Nodes[index1].Text = realpart.Attributes[index1].ToString();
			this.CarPartsTreeView.Nodes[parent].Nodes[index2].Text = realpart.Attributes[index2].ToString();

			this.CarPartsTreeView.SelectedNode = this.CarPartsTreeView.Nodes[parent].Nodes[index2];
		}

		private void ReverseAttributesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Model.GetRealPart(this.CarPartsTreeView.SelectedNode.Index).Attributes.Reverse();
			this.LoadTreeView(this.CarPartsTreeView.SelectedNode.FullPath);
		}

		private void SortAttributesByKeyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var realpart = this.Model.GetRealPart(this.CarPartsTreeView.SelectedNode.Index);
			realpart.Attributes.Sort((x, y) => x.Key.CompareTo(y.Key));
			this.LoadTreeView(this.CarPartsTreeView.SelectedNode.FullPath);
		}

		private void AddCustomAttributeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using var creator = new CustomAttribCreator(this.Model.GameINT)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            if (creator.ShowDialog() == DialogResult.OK)
            {

                var node = this.CarPartsTreeView.SelectedNode;
                var realpart = this.Model.GetRealPart(node.Index);
                realpart.AddCustomAttribute(creator.Value, creator.Type);
                var attribute = realpart.Attributes[^1];
                node.Nodes.Add(attribute.ToString());
                this.CarPartsPropertyGrid.Refresh();

            }
        }

		private void HasherToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var hasher = new Hasher() { StartPosition = FormStartPosition.CenterScreen };
			this._openforms.Add(hasher);
			hasher.Show();
		}

		private void RaiderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var raider = new Raider() { StartPosition = FormStartPosition.CenterScreen };
			this._openforms.Add(raider);
			raider.Show();
		}

		#endregion

		#region TreeView and Grid

		private void CarPartsTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (this.CarPartsTreeView.SelectedNode != null)
			{

				this.CarPartsTreeView.SelectedNode.ForeColor = this.CarPartsTreeView.ForeColor;

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

		private void CarPartsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			Console.WriteLine(e.Node?.FullPath);
			var selected = this.GetSelectedObject(e.Node);
			this.ToggleMenuStripControls(e.Node);
			this.CarPartsPropertyGrid.SelectedObject = selected;
		}

		private void CarPartsPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			this.CarPartsTreeView.SelectedNode.Text = this.CarPartsPropertyGrid.SelectedObject.ToString();
			this.CarPartsPropertyGrid.Refresh();

			if (this.CarPartsTreeView.SelectedNode.Level == 1)
			{

				var index = this.CarPartsTreeView.SelectedNode.Parent.Index;
				this.CarPartsTreeView.SelectedNode.Parent.Text = this.Model.ModelCarParts[index].ToString();

			}
		}

		#endregion

		private void CarPartsEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			foreach (var form in this._openforms)
			{

				try { form.Close(); }
				catch { }

			}
		}
	}
}
