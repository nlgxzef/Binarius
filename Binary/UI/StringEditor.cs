using Binary.Interact;
using Binary.Prompt;
using Binary.Properties;
using Binary.Tools;

using CoreExtensions.Management;
using CoreExtensions.Text;

using Endscript.Core;
using Endscript.Enums;

using Nikki.Support.Shared.Class;
using Nikki.Support.Shared.Parts.STRParts;
using Nikki.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Binary.UI
{
	public partial class StringEditor : Form
	{
		private const string Key = "Key";
		private const string Label = "Label";
		private const string TText = "Text";
		private STRBlock STR { get; }
		private readonly List<Form> _openforms;
		private readonly HashSet<string> _modified;
		public List<string> Commands { get; }
		private readonly string _strpath;

		// The selected string before typing, and why what is typed can't be written
		private StringRecord _original;
		private string _refused;


        private Color HighlightColor { get; set; }
        private Color ModifiedColor { get; set; }

        public StringEditor(STRBlock str, string path)
		{
			this.InitializeComponent();
			this.splitContainer1.FixedPanel = FixedPanel.Panel1;
			this.STR = str;
			this._strpath = path;
			this._openforms = new List<Form>();
			this._modified = new HashSet<string>();
			this.Commands = new List<string>();
			this.Text = $"String Editor : {this.STR.CollectionName}";
			this.StrEditorListView.Columns[^1].Width = -2;
			this.ToggleTheme();
			this.LoadListView();
			this.ToggleMenuStripControls();
		}

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

			// Primary colors and controls
			this.BackColor = theme.Colors.MainBackColor;
			this.ForeColor = theme.Colors.MainForeColor;

			// List view
			this.StrEditorListView.BackColor = theme.Colors.PrimBackColor;
			this.StrEditorListView.ForeColor = theme.Colors.PrimForeColor;

			// Labels
			this.label1.ForeColor = theme.Colors.LabelTextColor;
			this.label2.ForeColor = theme.Colors.LabelTextColor;
			this.label3.ForeColor = theme.Colors.LabelTextColor;

			// Text boxes
			this.TextBoxKey.BackColor = theme.Colors.TextBoxBackColor;
			this.TextBoxKey.ForeColor = theme.Colors.TextBoxForeColor;
			this.TextBoxLabel.BackColor = theme.Colors.TextBoxBackColor;
			this.TextBoxLabel.ForeColor = theme.Colors.TextBoxForeColor;
			this.TextBoxText.BackColor = theme.Colors.TextBoxBackColor;
			this.TextBoxText.ForeColor = theme.Colors.TextBoxForeColor;
			this.StringEditorTextBox.BackColor = theme.Colors.TextBoxBackColor;
			this.StringEditorTextBox.ForeColor = theme.Colors.TextBoxForeColor;

            // Menu strip and menu items
            this.StrEditorMenuStrip.MenuStripGradientBegin = theme.Colors.MenuStripGradientBegin;
            this.StrEditorMenuStrip.MenuStripGradientEnd = theme.Colors.MenuStripGradientEnd;
            this.StrEditorMenuStrip.MenuStripForeColor = theme.Colors.LabelTextColor;
            this.StrEditorMenuStrip.MenuBorder = theme.Colors.MenuBorder;
            this.StrEditorMenuStrip.MenuItemBorder = theme.Colors.MenuItemBorder;
            this.StrEditorMenuStrip.MenuItemPressedGradientBegin = theme.Colors.MenuItemPressedGradientBegin;
            this.StrEditorMenuStrip.MenuItemPressedGradientMiddle = theme.Colors.MenuItemPressedGradientMiddle;
            this.StrEditorMenuStrip.MenuItemPressedGradientEnd = theme.Colors.MenuItemPressedGradientEnd;
            this.StrEditorMenuStrip.MenuItemSelected = theme.Colors.MenuItemSelected;
            this.StrEditorMenuStrip.MenuItemSelectedGradientBegin = theme.Colors.MenuItemSelectedGradientBegin;
            this.StrEditorMenuStrip.MenuItemSelectedGradientEnd = theme.Colors.MenuItemSelectedGradientEnd;
            this.StrEditorMenuStrip.ImageMarginGradientBegin = theme.Colors.MenuItemPressedGradientBegin;
            this.StrEditorMenuStrip.ImageMarginGradientMiddle = theme.Colors.MenuItemPressedGradientMiddle;
            this.StrEditorMenuStrip.ImageMarginGradientEnd = theme.Colors.MenuItemPressedGradientEnd;
            this.StrEditorMenuStrip.ForeColor = theme.Colors.LabelTextColor;
            this.AddStringToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.AddStringToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.RemoveStringToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.RemoveStringToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.EditStringToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.EditStringToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.ReplaceStringToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.ReplaceStringToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.FindPreviousToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.FindPreviousToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.FindNextToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.FindNextToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.HasherToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.HasherToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.RaiderToolStripMenuItem.BackColor = theme.Colors.MenuItemBackColor;
			this.RaiderToolStripMenuItem.ForeColor = theme.Colors.MenuItemForeColor;

            this.HighlightColor = theme.Colors.SearchHighlightBackColor;
            this.ModifiedColor = theme.Colors.ModifiedItemBackColor;

        }

		#endregion

		#region Methods

		private void LoadListView(int index = -1)
		{
			this.StrEditorListView.Items.Clear();
			this.StrEditorListView.BeginUpdate();

			var count = 0;

			foreach (var record in this.STR.GetRecords())
			{

				var item = new ListViewItem()
				{
					Text = (count++).ToString()
				};

				item.SubItems.Add($"0x{record.Key:X8}");
				item.SubItems.Add(record.Label);
				item.SubItems.Add(record.Text);

				if (this._modified.Contains(item.SubItems[1].Text))
				{

                    item.BackColor = this.ModifiedColor;

                }

				this.StrEditorListView.Items.Add(item);

			}

			this.StrEditorListView.EndUpdate();

			if (index < 0 || index >= this.StrEditorListView.Items.Count) return;

			this.FastItemSelection(index);
		}

		private void ToggleMenuStripControls()
		{
			if (this.StrEditorListView.SelectedItems.Count == 0)
			{

				this.RemoveStringToolStripMenuItem.Enabled = false;
				this.EditStringToolStripMenuItem.Enabled = false;
				this.StringEditorTextBox.Enabled = false;

			}
			else
			{

				this.RemoveStringToolStripMenuItem.Enabled = true;
				this.EditStringToolStripMenuItem.Enabled = true;
				this.StringEditorTextBox.Enabled = true;

			}
		}

		private uint GetSelectedKey()
		{
			return this.StrEditorListView.SelectedItems.Count == 0
				? UInt32.MaxValue
				: Convert.ToUInt32(this.StrEditorListView.SelectedItems[0].SubItems[1].Text, 16);
		}

		private int FastFindIndex(uint key)
		{
			int count = 0;

			foreach (var record in this.STR.GetRecords())
			{

				if (record.Key == key) return count;
				++count;

			}

			return -1;
		}

		private void FastItemSelection(int index)
		{
			this.StrEditorListView.Items[index].Selected = true;
			this.StrEditorListView.Select();
			this.StrEditorListView.HideSelection = false;
			this.StrEditorListView.Items[index].EnsureVisible();
		}

		private void GenericFindSelection()
		{
			if (!String.IsNullOrEmpty(this.TextBoxKey.Text))
			{

				this.TextBoxKey_TextChanged(this.TextBoxKey, EventArgs.Empty);

			}
			else if (!String.IsNullOrEmpty(this.TextBoxLabel.Text))
			{

				this.TextBoxLabel_TextChanged(this.TextBoxLabel, EventArgs.Empty);

			}
			else if (!String.IsNullOrEmpty(this.TextBoxText.Text))
			{

				this.TextBoxText_TextChanged(this.TextBoxText, EventArgs.Empty);

			}
		}

		#endregion

		#region Menu Strip

		private void AddStringToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.CommitTyped();

			using var creator = new StringCreator();

			while (true)
			{

				if (creator.ShowDialog() == DialogResult.OK)
				{

					try
					{

						var text = creator.Value;
						this.STR.AddRecord(creator.Key, creator.Label, text);
						this.GenerateAddStringCommand(creator.Key, creator.Label, text);
						this._modified.Add(creator.Key);
						this.LoadListView();
						var index = this.FastFindIndex(Convert.ToUInt32(creator.Key, 16));
						this.FastItemSelection(index);
						this.GenericFindSelection();
						break;

					}
					catch (Exception ex)
					{

						MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					}

				}
				else
				{

					break;

				}

			}
		}

		private void RemoveStringToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{

				var index = this.StrEditorListView.SelectedIndices[0];
				var key = this.GetSelectedKey();
				this.CommitTyped();
				this.STR.RemoveRecord(key);
				this._original = null;
				this.GenerateRemoveStringCommand(this.StrEditorListView.Items[index].SubItems[1].Text);

				if (this.STR.StringRecordCount == 0)
				{

					this.LoadListView();
					this.StringEditorTextBox.Text = String.Empty;
					this.ToggleMenuStripControls();
					return;

				}

				if (index == 0) this.LoadListView(0);
				else this.LoadListView(index - 1);

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}
		}

		private void EditStringToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var prev = this.StrEditorListView.SelectedItems[0].SubItems[1].Text;
			var record = this.STR.GetRecord(prev);
			this.CommitTyped();
			using var creator = new StringCreator(record);

			while (true)
			{

				if (creator.ShowDialog() == DialogResult.OK)
				{

					try
					{

						var text = creator.Value;
						var previous = record.Text;

						// Before anything is changed
						var problem = record.CheckText(text);
						if (problem != null) throw new ArgumentException(problem);

						record.SetValue("Key", creator.Key);
						record.SetValue("Label", creator.Label);
						record.SetValue("Text", text);

                        var stringKey = Hashing.BinString(Convert.ToUInt32(prev, 16), LookupReturn.NULLREF);

						var name = this.GetCommandKey(prev, stringKey);

						if (text != previous) this.GenerateUpdateStringCommand(name, TText, text);
						this.GenerateUpdateStringCommand(name, Label, creator.Label);
						this.GenerateUpdateStringCommand(name, Key, creator.Key);
						this._modified.Add(creator.Key);

						var item = this.StrEditorListView.SelectedItems[0];
						item.SubItems[1].Text = creator.Key;
						item.SubItems[2].Text = creator.Label;
						item.SubItems[3].Text = creator.Value;
						this._original = record.PlainCopy() as StringRecord;
						this._refused = null;
						this.StringEditorTextBox.Text = creator.Value;
						item.BackColor = this.ModifiedColor;

						this.GenericFindSelection();
						break;

					}
					catch (Exception ex)
					{

						MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					}

				}
				else
				{

					break;
					
				}

			}
		}

		private void ReplaceStringToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.StrEditorListView.Items.Count == 0) return;

			using var with = new Input("Enter string to replace with");
			using var input = new Input("Enter string to search for",
										new Predicate<string>(_ => !String.IsNullOrEmpty(_)),
										"Input string cannot be null or empty");
			
			if (input.ShowDialog() == DialogResult.OK && with.ShowDialog() == DialogResult.OK)
			{

				using var check = new Check("Make case-sensitive replace?", false);

				if (check.ShowDialog() == DialogResult.OK)
				{

					this.CommitTyped();
					this.StrEditorListView.BeginUpdate();
					var skipped = new List<string>();

					for (int i = 0; i < this.StrEditorListView.Items.Count; ++i)
					{

						var item = this.StrEditorListView.Items[i];
						var key = item.SubItems[1].Text;
						var record = this.STR.GetRecord(key);

						var options = check.Value
							? RegexOptions.Multiline | RegexOptions.CultureInvariant
							: RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

						var replaced = Regex.Replace(record.Text, input.Value, with.Value, options);

						if (replaced == record.Text) continue;

						var problem = record.CheckText(replaced);

						if (problem != null)
						{

							skipped.Add(problem);
							continue;

						}

						record.Text = replaced;
						this.GenerateUpdateStringCommand(key, TText, record.Text);

						if (record.Text != item.SubItems[3].Text)
						{

							this._modified.Add(item.SubItems[1].Text);
							item.BackColor = this.ModifiedColor;

						}

						item.SubItems[3].Text = record.Text;

					}

					this.GenericFindSelection();
					this.StrEditorListView.EndUpdate();
					this.ShowSelectedText();

					if (skipped.Count > 0)
					{

						const int shown = 15;
						var message = $"{skipped.Count} string(s) could not be replaced:" + Environment.NewLine + Environment.NewLine +
							String.Join(Environment.NewLine, skipped.Take(shown));

						if (skipped.Count > shown) message += Environment.NewLine + $"...and {skipped.Count - shown} more.";

						MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

					}

				}

			}

		}

		private void FindPreviousToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Considering it is activated
			if (this.StrEditorListView.Items.Count == 0) return;

			var index = this.StrEditorListView.SelectedIndices.Count == 0
				? this.StrEditorListView.Items.Count - 1 : this.StrEditorListView.SelectedIndices[0] - 1;

			var match = this.HighlightColor;

			for (int i = index; i >= 0; --i)
			{

				if (this.StrEditorListView.Items[i].BackColor == match)
				{

					this.FastItemSelection(i);
					return;

				}

			}

			for (int i = this.StrEditorListView.Items.Count - 1; i > index; --i)
			{

				if (this.StrEditorListView.Items[i].BackColor == match)
				{

					this.FastItemSelection(i);
					return;

				}

			}
		}

		private void FindNextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Considering it is activated
			if (this.StrEditorListView.Items.Count == 0) return;

			var index = this.StrEditorListView.SelectedIndices.Count == 0
				? 0 : this.StrEditorListView.SelectedIndices[0] + 1;

			var match = this.HighlightColor;

			for (int i = index; i < this.StrEditorListView.Items.Count; ++i)
			{

				if (this.StrEditorListView.Items[i].BackColor == match)
				{

					this.FastItemSelection(i);
					return;

				}

			}

			for (int i = 0; i < index; ++i)
			{

				if (this.StrEditorListView.Items[i].BackColor == match)
				{

					this.FastItemSelection(i);
					return;

				}

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

		#region List View

		private void StrEditorListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			// The box is left without losing focus when the selection is changed for it
			this.CommitTyped();
			this._original = null;

			if (this._refused != null)
			{

				var refused = this._refused;
				this._refused = null;
				MessageBox.Show(refused, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

			}

			if (this.StrEditorListView.SelectedItems.Count == 0)
			{

				this.StringEditorTextBox.Text = String.Empty;
				this.ToggleMenuStripControls();
				return;

			}

			var key = this.GetSelectedKey();
			var record = this.STR.GetRecord(key);

			if (record == null) return;

			this._original = record.PlainCopy() as StringRecord;
			this.StringEditorTextBox.Text = this.StrEditorListView.SelectedItems[0].SubItems[3].Text;
			this.ToggleMenuStripControls();
		}

		private void StrEditorListView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.EditStringToolStripMenuItem_Click(this.EditStringToolStripMenuItem, EventArgs.Empty);
		}

		private void StrEditorListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);
            var brush = new SolidBrush(theme.Colors.TextBoxBackColor);
			e.Graphics.FillRectangle(brush, e.Bounds);
			e.DrawText();
		}

		private void StrEditorListView_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void StrEditorListView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			uint key;
			int index;

			if (this.StrEditorListView.SelectedItems.Count == 0)
			{

				key = 0xFFFFFFFF;
				index = -1;

			}
			else
			{

				key = this.GetSelectedKey();
				index = 0;

			}

			switch (e.Column)
			{
				case 1: // Key
					this.STR.SortRecordsByKey();
					if (index == 0) index = this.FastFindIndex(key);
					this.LoadListView(index);
					break;

				case 2: // Label
					this.STR.SortRecordsByLabel();
					if (index == 0) index = this.FastFindIndex(key);
					this.LoadListView(index);
					break;

				case 3: // Text
					this.STR.SortRecordsByText();
					if (index == 0) index = this.FastFindIndex(key);
					this.LoadListView(index);
					break;

				default:
					return;

			}

			this.GenericFindSelection();
		}

		#endregion

		#region Events

		private void TextBoxKey_TextChanged(object sender, EventArgs e)
		{
			this.StrEditorListView.BeginUpdate();

			if (String.IsNullOrEmpty(this.TextBoxKey.Text))
			{

				foreach (ListViewItem item in this.StrEditorListView.Items)
				{

					item.BackColor = this.StrEditorListView.BackColor;
					
					if (this._modified.Contains(item.SubItems[1].Text))
					{

						item.BackColor = this.ModifiedColor;

					}

				}

				this.TextBoxLabel.Enabled = true;
				this.TextBoxText.Enabled = true;
				this.FindNextToolStripMenuItem.Enabled = false;
				this.FindPreviousToolStripMenuItem.Enabled = false;

			}
			else
			{

				var find = this.TextBoxKey.Text.ToUpperInvariant();

				foreach (ListViewItem item in this.StrEditorListView.Items)
				{

					item.BackColor = this.StrEditorListView.BackColor;

					if (this._modified.Contains(item.SubItems[1].Text))
					{

						item.BackColor = this.ModifiedColor;

					}

					if (item.SubItems[1].Text.ToUpperInvariant().Contains(find))
					{

						item.BackColor = this.HighlightColor;

					}

				}

				this.TextBoxLabel.Enabled = false;
				this.TextBoxText.Enabled = false;
				this.FindNextToolStripMenuItem.Enabled = true;
				this.FindPreviousToolStripMenuItem.Enabled = true;

			}

			this.StrEditorListView.EndUpdate();
		}

		private void TextBoxLabel_TextChanged(object sender, EventArgs e)
		{
			this.StrEditorListView.BeginUpdate();

			if (String.IsNullOrEmpty(this.TextBoxLabel.Text))
			{

				foreach (ListViewItem item in this.StrEditorListView.Items)
				{

					item.BackColor = this.StrEditorListView.BackColor;
					
					if (this._modified.Contains(item.SubItems[1].Text))
					{

						item.BackColor = this.ModifiedColor;

					}

				}

				this.TextBoxKey.Enabled = true;
				this.TextBoxText.Enabled = true;
				this.FindNextToolStripMenuItem.Enabled = false;
				this.FindPreviousToolStripMenuItem.Enabled = false;

			}
			else
			{

				var find = this.TextBoxLabel.Text.ToUpperInvariant();

				foreach (ListViewItem item in this.StrEditorListView.Items)
				{

					item.BackColor = this.StrEditorListView.BackColor;

					if (this._modified.Contains(item.SubItems[1].Text))
					{

						item.BackColor = this.ModifiedColor;

					}

					if (item.SubItems[2].Text.ToUpperInvariant().Contains(find))
					{

						item.BackColor = this.HighlightColor;

					}

				}

				this.TextBoxKey.Enabled = false;
				this.TextBoxText.Enabled = false;
				this.FindNextToolStripMenuItem.Enabled = true;
				this.FindPreviousToolStripMenuItem.Enabled = true;

			}

			this.StrEditorListView.EndUpdate();
		}

		private void TextBoxText_TextChanged(object sender, EventArgs e)
		{
			this.StrEditorListView.BeginUpdate();

			if (String.IsNullOrEmpty(this.TextBoxText.Text))
			{

				foreach (ListViewItem item in this.StrEditorListView.Items)
				{

					item.BackColor = this.StrEditorListView.BackColor;
					
					if (this._modified.Contains(item.SubItems[1].Text))
					{

						item.BackColor = this.ModifiedColor;

					}

				}

				this.TextBoxKey.Enabled = true;
				this.TextBoxLabel.Enabled = true;
				this.FindNextToolStripMenuItem.Enabled = false;
				this.FindPreviousToolStripMenuItem.Enabled = false;

			}
			else
			{

				var find = this.TextBoxText.Text.ToUpperInvariant();

				foreach (ListViewItem item in this.StrEditorListView.Items)
				{

					item.BackColor = this.StrEditorListView.BackColor;

					if (this._modified.Contains(item.SubItems[1].Text))
					{

						item.BackColor = this.ModifiedColor;

					}

					if (item.SubItems[3].Text.ToUpperInvariant().Contains(find))
					{

						item.BackColor = this.HighlightColor;

					}

				}

				this.TextBoxKey.Enabled = false;
				this.TextBoxLabel.Enabled = false;
				this.FindNextToolStripMenuItem.Enabled = true;
				this.FindPreviousToolStripMenuItem.Enabled = true;

			}

			this.StrEditorListView.EndUpdate();
		}

		private void StringEditorTextBox_TextChanged(object sender, EventArgs e)
		{
			// Considering it is enabled because string record was selected

			if (this.StrEditorListView.SelectedItems.Count == 0) return;

			var key = this.GetSelectedKey();
			var record = this.STR.GetRecord(key);

			if (record == null) return;

			var selected = this.StrEditorListView.SelectedItems[0];
			var typed = this.StringEditorTextBox.Text ?? String.Empty;

			if (this._original is null || this._original.Key != record.Key) this._original = record.PlainCopy() as StringRecord;

			// While it can't be written it stays in the box only, and the string is what it was
			this._refused = record.CheckText(typed);

			if (this._refused != null || typed == this._original.Text) record.CopyTextFrom(this._original);
			else record.Text = typed;

			if (selected.SubItems[3].Text != record.Text)
			{

				this._modified.Add(selected.SubItems[1].Text);
				selected.BackColor = this.ModifiedColor;

			}

			selected.SubItems[3].Text = record.Text;

			var upper = record.Text.ToUpperInvariant();

			if (!String.IsNullOrEmpty(this.TextBoxText.Text))
			{

				selected.BackColor = !upper.Contains(this.TextBoxText.Text.ToUpperInvariant())
					? this.StrEditorListView.BackColor
					: this.HighlightColor;

			}
		}

		private void StringEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Staying to correct the text
			if (e.Cancel) return;

			this.CommitTyped();

			foreach (var form in this._openforms)
			{

				try { form.Close(); }
				catch { }

			}
		}

		#endregion

		#region Commands

		private void GenerateUpdateStringCommand(string key, string property, string value)
		{
			var argument = ToArgument(value);

			if (argument is null)
			{

				this.GenerateNote($"{property} of string {key}");
				return;

			}

			var command = $"{eCommandType.update_string} {this._strpath} {key} {property} {argument}";
			this.Commands.Add(command);
		}

		private void GenerateAddStringCommand(string key, string label, string text)
		{
			var labelArgument = ToArgument(label);
			var textArgument = ToArgument(text);

			if (labelArgument is null || textArgument is null)
			{

				this.GenerateNote($"The new string {key}");
				return;

			}

			var command = $"{eCommandType.add_string} {this._strpath} {key} {labelArgument} {textArgument}";
			this.Commands.Add(command);
		}

		// In place of a command that would do something else
		private void GenerateNote(string what)
		{
			this.Commands.Add($"// {what} is not in this script: its line breaks can't be written in a script line.");
		}

		// A label to name the string with, where it leads to the same key
		private string GetCommandKey(string key, string resolved)
		{
			if (String.IsNullOrEmpty(resolved) || resolved.BinHash() != Convert.ToUInt32(key, 16)) return key;

			return ToArgument(resolved, false) ?? key; // in the place of a key, not text
		}

		// A value as an argument of a script line (quotes escaped where needed), or null for a line break
		private static string ToArgument(string value, bool text = true) => EndArguments.Quote(value, text);

		private void GenerateRemoveStringCommand(string key)
		{
			var command = $"{eCommandType.remove_string} {this._strpath} {key}";
			this.Commands.Add(command);
		}

		#endregion

		private void StringEditorTextBox_Validating(object sender, CancelEventArgs e)
		{
			if (this._refused is null) return;

			var message = this._refused + Environment.NewLine + Environment.NewLine +
				"Retry to correct the text, or Cancel to keep the previous one.";

			if (MessageBox.Show(message, "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
			{

				e.Cancel = true;
				return;

			}

			this._refused = null;
			this.StringEditorTextBox.Text = this._original?.Text ?? String.Empty;
		}

		private void ShowSelectedText()
		{
			if (this.StrEditorListView.SelectedItems.Count == 0) return;

			var record = this.STR.GetRecord(this.GetSelectedKey());

			if (record == null) return;

			this._original = record.PlainCopy() as StringRecord;

			// Text that is being corrected stays
			if (this._refused is null) this.StringEditorTextBox.Text = record.Text;
		}

		private void StringEditorTextBox_Validated(object sender, EventArgs e) => this.CommitTyped();

		// The box changes the string while typing, so its command is made when the typing is over
		private void CommitTyped()
		{
			if (this._original is null) return;

			var record = this.STR.GetRecord(this._original.Key);

			if (record == null || record.Text == this._original.Text) return;

			this.GenerateUpdateStringCommand($"0x{record.Key:X8}", TText, record.Text);
			this._original = record.PlainCopy() as StringRecord;
		}
	}
}
