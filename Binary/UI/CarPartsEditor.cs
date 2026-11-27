using Binary.Interact;
using Binary.Prompt;
using Binary.Properties;
using Binary.Tools;

using Endscript.Core;
using Endscript.Enums;

using Nikki.Core;
using Nikki.Reflection.Interface;
using Nikki.Support.Shared.Class;
using Nikki.Support.Shared.Parts.CarParts;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;



namespace Binary.UI
{
	public partial class CarPartsEditor : Form
	{
		private DBModelPart Model { get; }
		private readonly List<Form> _openforms;
		private readonly string _filename;

		// How scripts find the selected part, from before it is edited in the grid
		private string _selectedPart;

		// Copied attribute, kept until Binarius closes so it can be pasted in another car's parts too
		private static CPAttribute s_clipboard;
		private static GameINT s_clipboardGame;

		private readonly ToolStripMenuItem CopyAttributeToolStripMenuItem;
		private readonly ToolStripMenuItem PasteAttributeToolStripMenuItem;
		private readonly ToolStripMenuItem ScriptAttributeToolStripMenuItem;
		private readonly ToolStripMenuItem ScriptPartToolStripMenuItem;

		// Separators of the menus, themed with the items
		private readonly List<ToolStripSeparator> _separators = new List<ToolStripSeparator>();

		private ToolStripSeparator Separator()
		{
			var separator = new ToolStripSeparator();
			this._separators.Add(separator);
			return separator;
		}

		/// <summary>
		/// Endscript commands for the changes made, to put in the command prompt.
		/// </summary>
		public List<string> Commands { get; } = new List<string>();

        private Color HighlightColor1 { get; set; }
        private Color HighlightColor2 { get; set; }
        private Color HighlightColor3 { get; set; }

		/// <param name="model">Collection to edit.</param>
		/// <param name="filename">File of the collection, for the commands.</param>
        public CarPartsEditor(DBModelPart model, string filename)
		{
			this._openforms = new List<Form>();
			this._filename = filename;
			this.InitializeComponent();

			this.CopyAttributeToolStripMenuItem = new ToolStripMenuItem("Copy Attribute", null, this.CopyAttributeToolStripMenuItem_Click)
			{
				ShortcutKeys = Keys.Alt | Keys.C,
				ToolTipText = "Copies the selected attribute, to paste it over another part, also of another car.",
			};
			this.PasteAttributeToolStripMenuItem = new ToolStripMenuItem("Paste Attribute", null, this.PasteAttributeToolStripMenuItem_Click)
			{
				ShortcutKeys = Keys.Alt | Keys.V,
				ToolTipText = "Pastes the copied attribute over the selected part: over the same attribute if it has it, or as a new one.",
			};
			this.ScriptAttributeToolStripMenuItem = new ToolStripMenuItem("Script Attribute", null, this.ScriptAttributeToolStripMenuItem_Click)
			{
				ToolTipText = "Writes the commands that give the part this attribute with its values.",
			};
			this.ScriptPartToolStripMenuItem = new ToolStripMenuItem("Script Part", null, this.ScriptPartToolStripMenuItem_Click)
			{
				ToolTipText = "Writes the commands that add this part, with all its attributes, to the collection.",
			};

			// Grouped by what they do
			this.CarPartsToolStripMenuItem.DropDownItems.Clear();
			this.CarPartsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.AddPartToolStripMenuItem, this.CopyPartToolStripMenuItem, this.RemovePartToolStripMenuItem,
				this.Separator(),
				this.MoveUpPartsToolStripMenuItem, this.MoveDownPartsToolStripMenuItem, this.MoveFirstPartsToolStripMenuItem, this.MoveLastPartsToolStripMenuItem,
				this.Separator(),
				this.ReversePartsToolStripMenuItem, this.SortPartsByNameToolStripMenuItem,
				this.Separator(),
				this.FindAndReplaceToolStripMenuItem,
				this.Separator(),
				this.ScriptPartToolStripMenuItem,
			});

			this.AttributesToolStripMenuItem.DropDownItems.Clear();
			this.AttributesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.AddAttributeToolStripMenuItem, this.AddCustomAttributeToolStripMenuItem, this.RemoveAttributeToolStripMenuItem,
				this.Separator(),
				this.MoveUpAttributesToolStripMenuItem, this.MoveDownAttributesToolStripMenuItem,
				this.Separator(),
				this.ReverseAttributesToolStripMenuItem, this.SortAttributesByKeyToolStripMenuItem,
				this.Separator(),
				this.CopyAttributeToolStripMenuItem, this.PasteAttributeToolStripMenuItem,
				this.Separator(),
				this.ScriptAttributeToolStripMenuItem,
			});

			this.ToggleTheme();
			this.Model = model;
			this.Text = $"Car Parts Editor : {this.Model.CollectionName}";
			this.LoadTreeView();
			this.ToggleMenuStripControls(null);

			this.FindAndReplaceToolStripMenuItem.Enabled = true;
		}

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

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
            this.CarPartsEditorMenuStrip.MenuStripGradientBegin = theme.Colors.MenuStripGradientBegin;
            this.CarPartsEditorMenuStrip.MenuStripGradientEnd = theme.Colors.MenuStripGradientEnd;
            this.CarPartsEditorMenuStrip.MenuStripForeColor = theme.Colors.LabelTextColor;
            this.CarPartsEditorMenuStrip.MenuBorder = theme.Colors.MenuBorder;
            this.CarPartsEditorMenuStrip.MenuItemBorder = theme.Colors.MenuItemBorder;
            this.CarPartsEditorMenuStrip.MenuItemPressedGradientBegin = theme.Colors.MenuItemPressedGradientBegin;
            this.CarPartsEditorMenuStrip.MenuItemPressedGradientMiddle = theme.Colors.MenuItemPressedGradientMiddle;
            this.CarPartsEditorMenuStrip.MenuItemPressedGradientEnd = theme.Colors.MenuItemPressedGradientEnd;
            this.CarPartsEditorMenuStrip.MenuItemSelected = theme.Colors.MenuItemSelected;
            this.CarPartsEditorMenuStrip.MenuItemSelectedGradientBegin = theme.Colors.MenuItemSelectedGradientBegin;
            this.CarPartsEditorMenuStrip.MenuItemSelectedGradientEnd = theme.Colors.MenuItemSelectedGradientEnd;
            this.CarPartsEditorMenuStrip.ImageMarginGradientBegin = theme.Colors.MenuItemPressedGradientBegin;
            this.CarPartsEditorMenuStrip.ImageMarginGradientMiddle = theme.Colors.MenuItemPressedGradientMiddle;
            this.CarPartsEditorMenuStrip.ImageMarginGradientEnd = theme.Colors.MenuItemPressedGradientEnd;
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
			this.CopyAttributeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.CopyAttributeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.PasteAttributeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.PasteAttributeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.ScriptAttributeToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.ScriptAttributeToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.ScriptPartToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.ScriptPartToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;

			foreach (var separator in this._separators)
			{

				separator.BackColor = theme.Colors.MenuItemBackColor;
				separator.ForeColor = theme.Colors.MenuItemForeColor;

			}

            // Highlight colors
            this.HighlightColor1 = theme.Colors.SearchHighlightBackColor;
            this.HighlightColor2 = theme.Colors.SelectedNodeForeColor;
            this.HighlightColor3 = theme.Colors.FirstSelectedNodeForeColor;
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

				nodes[count++] = MakePartNode(realpart);

			}

			this.CarPartsTreeView.Nodes.AddRange(nodes);
			this.CarPartsTreeView.EndUpdate();

			if (!String.IsNullOrEmpty(selected))
			{

				this.RecursiveNodeSelection(selected, this.CarPartsTreeView.Nodes);

			}
		}

		private static TreeNode MakePartNode(RealCarPart realpart)
		{
			var level0 = new TreeNode(realpart.PartName);

			foreach (var attribute in realpart.Attributes)
			{

				level0.Nodes.Add(new TreeNode(attribute.ToString()));

			}

			return level0;
		}

		// Shows a part again after its attributes changed, keeping it expanded and selected as it was
		private void RefreshPartNode(int index, int selectAttribute = -1)
		{
			var node = this.CarPartsTreeView.Nodes[index];
			var expanded = node.IsExpanded;
			var fresh = MakePartNode(this.Model.ModelCarParts[index]);

			this.CarPartsTreeView.BeginUpdate();
			node.Text = fresh.Text;
			node.Nodes.Clear();
			foreach (TreeNode child in fresh.Nodes.Cast<TreeNode>().ToList()) { fresh.Nodes.Remove(child); node.Nodes.Add(child); }
			if (expanded) node.Expand();
			this.CarPartsTreeView.EndUpdate();

			this.CarPartsTreeView.SelectedNode = selectAttribute >= 0 && selectAttribute < node.Nodes.Count ? node.Nodes[selectAttribute] : node;
			this.CarPartsPropertyGrid.Refresh();
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
                    : this.HighlightColor1;

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
				this.CopyAttributeToolStripMenuItem.Enabled = false;
				this.PasteAttributeToolStripMenuItem.Enabled = false;
				this.ScriptAttributeToolStripMenuItem.Enabled = false;
				this.ScriptPartToolStripMenuItem.Enabled = false;

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
				this.CopyAttributeToolStripMenuItem.Enabled = node.Level == 1;
				this.PasteAttributeToolStripMenuItem.Enabled = s_clipboard != null;
				this.ScriptAttributeToolStripMenuItem.Enabled = node.Level == 1;
				this.ScriptPartToolStripMenuItem.Enabled = node.Level == 0;

			}
		}

		#endregion

		#region Commands

		// Writes a command for what was just done
		private void Script(eCommandType type, params string[] args) =>
			this.Commands.Add(PartsScript.Line(this._filename, this.Model, type, args));

		// How a script finds a part now
		private string PartRef(int index) => PartsScript.PartReference(this.Model, index);

		private int SelectedPartIndex()
		{
			var node = this.CarPartsTreeView.SelectedNode;
			return node is null ? -1 : node.Level == 0 ? node.Index : node.Parent.Index;
		}

		// How a script finds the selected part before it is edited in the grid, as an edit can change its name
		private void RememberSelectedPart()
		{
			var index = this.SelectedPartIndex();
			this._selectedPart = index < 0 ? null : this.PartRef(index);
		}

		#endregion

		#region Menu Strip Controls

		private void AddPartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Parts are found by name in scripts, so a new part gets one
			var hint = this.Model.GameINT == GameINT.Underground1 || this.Model.GameINT == GameINT.Underground2 || this.Model.GameINT == GameINT.MostWanted
				? "It is the part's PartLabel."
				: "It is the part's PART_NAME_BASE_HASH; the name attributes can change it later.";

			using var input = new Input($"Enter name of the new part. {hint}",
				_ => !String.IsNullOrEmpty(_) && !_.StartsWith('#') && PartsScript.TryFindPart(this.Model, _) < 0,
				"The name can't be empty, start with #, or be the same as another part");

			if (input.ShowDialog() != DialogResult.OK) return;

			this.Model.AddRealPart();
			this.Model.GetLastPart().SetScriptName(input.Value);
			this.Script(eCommandType.add_inparts, input.Value);
			this.CarPartsTreeView.Nodes.Add(MakePartNode(this.Model.GetLastPart()));
			this.CarPartsTreeView.SelectedNode = this.CarPartsTreeView.Nodes[^1];
		}

		private void RemovePartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in a real car part node

			var index = this.CarPartsTreeView.SelectedNode.Index;
			this.Script(eCommandType.remove_inparts, this.PartRef(index));
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
			this.Script(eCommandType.copy_inparts, this.PartRef(index));
			this.Model.ClonePart(index);

			var realpart = this.Model.GetLastPart();
			this.Model.ModelCarParts.RemoveAt(count);
			this.Model.ModelCarParts.Insert(index + 1, realpart);

			this.CarPartsTreeView.Nodes.Insert(index + 1, MakePartNode(realpart));
		}

		// Moves the selected part; false when it can't go there
		private void MovePart(string where)
		{
			var index = this.CarPartsTreeView.SelectedNode.Index;
			var count = this.Model.CarPartsCount;
			int to = where switch { "up" => index - 1, "down" => index + 1, "first" => 0, _ => count - 1 };

			if (to < 0 || to >= count)
			{

				MessageBox.Show($"Unable to move {where} because selected node is the {(to < 0 ? "up" : "down")} most node",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;

			}

			if (to == index) return;

			this.Script(eCommandType.move_inparts, this.PartRef(index), where);

			var part = this.Model.ModelCarParts[index];
			this.Model.ModelCarParts.RemoveAt(index);
			this.Model.ModelCarParts.Insert(to, part);

			this.CarPartsTreeView.BeginUpdate();
			var node = this.CarPartsTreeView.Nodes[index];
			this.CarPartsTreeView.Nodes.RemoveAt(index);
			this.CarPartsTreeView.Nodes.Insert(to, node);
			this.CarPartsTreeView.EndUpdate();
			this.CarPartsTreeView.SelectedNode = node;
		}

		private void MoveUpPartsToolStripMenuItem_Click(object sender, EventArgs e) => this.MovePart("up");

		private void MoveDownPartsToolStripMenuItem_Click(object sender, EventArgs e) => this.MovePart("down");

		private void MoveFirstPartsToolStripMenuItem_Click(object sender, EventArgs e) => this.MovePart("first");

		private void MoveLastPartsToolStripMenuItem_Click(object sender, EventArgs e) => this.MovePart("last");

		private void ReversePartsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Model.ReverseParts();
			this.Script(eCommandType.reverse_inparts);
			this.LoadTreeView(this.CarPartsTreeView.SelectedNode.FullPath);
		}

		private void SortPartsByNameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Model.SortByProperty(nameof(RealCarPart.PartName));
			this.Script(eCommandType.sort_inparts);
			this.LoadTreeView(this.CarPartsTreeView.SelectedNode.FullPath);
		}

		private void FindAndReplaceToolStripMenuItem_Click(object sende, EventArgs e)
		{
			if (this.CarPartsTreeView.Nodes.Count == 0) return;

			if (FindAndReplace(this.Model, null, null, out var line, this._filename))
			{

				this.Commands.Add(line);
				this.LoadTreeView(this.CarPartsTreeView.SelectedNode?.FullPath);
				this.CarPartsPropertyGrid.Refresh();
				this.RememberSelectedPart();

			}
		}

		/// <summary>
		/// Asks what to find and replace in all parts of a collection, as the Find and Replace menu item does, and
		/// does it.
		/// </summary>
		/// <param name="model">Collection to change.</param>
		/// <param name="find">Text to find, or null to ask for it.</param>
		/// <param name="replace">Text to replace with, or null to ask for it.</param>
		/// <param name="line">The command for it.</param>
		/// <param name="filename">File of the collection, for the command.</param>
		/// <returns>True if it was done.</returns>
		internal static bool FindAndReplace(DBModelPart model, string find, string replace, out string line, string filename)
		{
			line = null;

			// 4 windows:
			//     1. String to replace with
			//     2. String to replace itself
			//     3. Do case sensitive search
			//     4. Replace only PartLabel or absolutely everything

			// 1 & 2
			if (find is null || replace is null)
			{

				using var with = new Input("Enter string to replace with");
				using var input = new Input("Enter string to search for",
											new Predicate<string>(_ => !String.IsNullOrEmpty(_)),
											"Input string cannot be null or empty");

				if (input.ShowDialog() != DialogResult.OK || with.ShowDialog() != DialogResult.OK) return false;

				find = input.Value;
				replace = with.Value;

			}

			// 3
			using var check = new Check("Make case-sensitive replace?", false);
			if (check.ShowDialog() != DialogResult.OK) return false;

			// Make regex options based on result of 3
			var options = check.Value
				? RegexOptions.Multiline | RegexOptions.CultureInvariant
				: RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

			// 4
			using var all = new Check("Make replacement only in PartLabel?", false, true);
			if (all.ShowDialog() != DialogResult.OK) return false;

			foreach (var part in model.ModelCarParts) part.MakeReplace(all.Value, find, replace, options);

			line = PartsScript.Line(filename, model, eCommandType.replace_inparts,
				all.Value ? "label" : "all", check.Value ? "case" : "nocase", find, replace);
			return true;
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
				var reference = this.PartRef(node.Index);

				if (realpart.GetAttribute(creator.KeyChosen) != null)
				{

					MessageBox.Show($"The part already has attribute 0x{creator.KeyChosen:X8}.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;

				}

				realpart.AddAttribute(creator.KeyChosen);
				this.Script(eCommandType.add_attribute_inparts, reference, PartsScript.AttributeReference(realpart.Attributes[^1]));
				this.RefreshPartNode(node.Index, realpart.Length - 1);

			}
		}

		private void RemoveAttributeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Assuming we are in an attribute node

			var node = this.CarPartsTreeView.SelectedNode;
			var index = node.Parent.Index;
			var realpart = this.Model.GetRealPart(index);
			this.Script(eCommandType.remove_attribute_inparts, this.PartRef(index), PartsScript.AttributeReference(realpart.Attributes[node.Index]));
			realpart.Attributes.RemoveAt(node.Index);
			this.RefreshPartNode(index);
		}

		private void MoveAttribute(string where)
		{
			var node = this.CarPartsTreeView.SelectedNode;
			var index1 = node.Index;
			var index2 = where == "up" ? index1 - 1 : index1 + 1;
			var parent = node.Parent.Index;
			var realpart = this.Model.GetRealPart(parent);

			if (index2 < 0 || index2 >= realpart.Length)
			{

				MessageBox.Show($"Unable to move {where} because selected node is the {(index2 < 0 ? "up" : "down")} most node",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;

			}

			this.Script(eCommandType.move_attribute_inparts, this.PartRef(parent), PartsScript.AttributeReference(realpart.Attributes[index1]), where);

			// Switch attributes
			var temp = realpart.Attributes[index1];
			realpart.Attributes[index1] = realpart.Attributes[index2];
			realpart.Attributes[index2] = temp;

			this.CarPartsTreeView.Nodes[parent].Nodes[index1].Text = realpart.Attributes[index1].ToString();
			this.CarPartsTreeView.Nodes[parent].Nodes[index2].Text = realpart.Attributes[index2].ToString();

			this.CarPartsTreeView.SelectedNode = this.CarPartsTreeView.Nodes[parent].Nodes[index2];
		}

		private void MoveUpAttributesToolStripMenuItem_Click(object sender, EventArgs e) => this.MoveAttribute("up");

		private void MoveDownAttributesToolStripMenuItem_Click(object sender, EventArgs e) => this.MoveAttribute("down");

		private void ReverseAttributesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var index = this.CarPartsTreeView.SelectedNode.Index;
			this.Script(eCommandType.reverse_attribute_inparts, this.PartRef(index));
			this.Model.GetRealPart(index).Attributes.Reverse();
			this.LoadTreeView(this.CarPartsTreeView.SelectedNode.FullPath);
		}

		private void SortAttributesByKeyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var index = this.CarPartsTreeView.SelectedNode.Index;
			this.Script(eCommandType.sort_attribute_inparts, this.PartRef(index));
			var realpart = this.Model.GetRealPart(index);
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
				var reference = this.PartRef(node.Index);
                realpart.AddCustomAttribute(creator.Value, creator.Type);
				this.Script(eCommandType.add_custom_attribute_inparts, reference, creator.Value, creator.Type.ToString());
				this.RefreshPartNode(node.Index, realpart.Length - 1);

            }
        }

		private void CopyAttributeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!(this.GetSelectedObject(this.CarPartsTreeView.SelectedNode) is CPAttribute attribute)) return;

			s_clipboard = (CPAttribute)attribute.PlainCopy();
			s_clipboardGame = this.Model.GameINT;
			this.ToggleMenuStripControls(this.CarPartsTreeView.SelectedNode);
		}

		private void PasteAttributeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var index = this.SelectedPartIndex();
			if (s_clipboard is null || index < 0) return;

			if (s_clipboardGame != this.Model.GameINT)
			{

				MessageBox.Show($"The copied attribute is from {s_clipboardGame}, so it can't be pasted in {this.Model.GameINT}.",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;

			}

			// The commands before pasting, as pasting can change the name of the part
			var part = this.Model.ModelCarParts[index];
			var lines = PartsScript.ScriptAttribute(this._filename, this.Model, index, s_clipboard);
			var copy = (CPAttribute)s_clipboard.PlainCopy();
			var existing = PartsScript.TryFindAttribute(part, PartsScript.AttributeReference(copy));

			// Over the same attribute, or added at the end
			if (existing >= 0 && part.Attributes[existing].GetType() == copy.GetType()) part.Attributes[existing] = copy;
			else if (existing >= 0) { MessageBox.Show("The part has an attribute with that key of another kind.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
			else part.Attributes.Add(copy);

			this.Commands.AddRange(lines);
			this.RefreshPartNode(index, existing >= 0 ? existing : part.Length - 1);
			this.RememberSelectedPart();
		}

		private void ScriptPartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var index = this.SelectedPartIndex();
			if (index < 0) return;

			this.Commands.AddRange(PartsScript.ScriptPart(this._filename, this.Model, index));
		}

		private void ScriptAttributeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var node = this.CarPartsTreeView.SelectedNode;
			if (node is null || node.Level != 1) return;

			var attribute = this.Model.ModelCarParts[node.Parent.Index].Attributes[node.Index];
			this.Commands.AddRange(PartsScript.ScriptAttribute(this._filename, this.Model, node.Parent.Index, attribute));
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

                e.Node.ForeColor = this.HighlightColor2;

			}
			else
			{

                e.Node.ForeColor = this.HighlightColor3;

			}
		}

		private void CarPartsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var selected = this.GetSelectedObject(e.Node);
			this.ToggleMenuStripControls(e.Node);
			this.CarPartsPropertyGrid.SelectedObject = selected;
			this.RememberSelectedPart();
		}

		private void CarPartsPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			this.ScriptGridChange(e);

			this.CarPartsTreeView.SelectedNode.Text = this.CarPartsPropertyGrid.SelectedObject.ToString();
			this.CarPartsPropertyGrid.Refresh();

			if (this.CarPartsTreeView.SelectedNode.Level == 1)
			{

				var index = this.CarPartsTreeView.SelectedNode.Parent.Index;
				this.CarPartsTreeView.SelectedNode.Parent.Text = this.Model.ModelCarParts[index].ToString();

			}

			this.RememberSelectedPart();
		}

		// Writes the command for a value changed in the grid, finding the part by the name it had before
		private void ScriptGridChange(PropertyValueChangedEventArgs e)
		{
			var part = this._selectedPart;
			var item = e.ChangedItem;
			if (part is null || item?.PropertyDescriptor is null) return;

			var property = item.PropertyDescriptor.Name;
			var parent = item.Parent?.GridItemType == GridItemType.Property ? item.Parent.PropertyDescriptor?.Name : null;

			switch (this.CarPartsPropertyGrid.SelectedObject)
			{
				case RealCarPart realpart:
					if (parent is null && realpart.GetAccessibles().Contains(property))
						this.Script(eCommandType.update_inparts, part, property, realpart.GetValue(property));
					else if (parent != null && PartsScript.GetStructs(realpart).Contains(parent))
						this.Script(eCommandType.update_inparts, part, parent, property, PartsScript.GetStruct(realpart, parent).GetValue(property));
					else
						this.Commands.Add($"// {property} of part {part} was changed, but endscript commands can't change it");
					return;

				case ICustomAttrib custom:
					var attribute = (CPAttribute)custom;

					if (property == nameof(ICustomAttrib.Name))
					{

						// A new name is a new attribute
						this.Script(eCommandType.remove_attribute_inparts, part, e.OldValue?.ToString() ?? String.Empty);
						this.Script(eCommandType.add_custom_attribute_inparts,
							new[] { part, custom.Name, custom.Type.ToString() }.Concat(PartsScript.GetCustomValues(attribute)).ToArray());

					}
					else if (property == nameof(ICustomAttrib.Type))
					{

						this.Script(eCommandType.add_custom_attribute_inparts,
							new[] { part, custom.Name, custom.Type.ToString() }.Concat(PartsScript.GetCustomValues(attribute)).ToArray());

					}
					else
					{

						this.Script(eCommandType.update_custom_attribute_inparts,
							new[] { part, custom.Name }.Concat(PartsScript.GetCustomValues(attribute)).ToArray());

					}
					return;

				case CPAttribute attrib:
					// A new type is a new key: the command finds the attribute by the one it had
					var label = property == "Type" && e.OldValue is Enum old ? old.ToString() : PartsScript.AttributeReference(attrib);

					if (attrib.GetAccessibles().Contains(property))
						this.Script(eCommandType.update_attribute_inparts, part, label, property, attrib.GetValue(property));
					else
						this.Commands.Add($"// {property} of attribute {label} of part {part} was changed, but endscript commands can't change it");
					return;
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
