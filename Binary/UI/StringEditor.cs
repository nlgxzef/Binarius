﻿using Binary.Interact;
using Binary.Prompt;
using Binary.Properties;
using Binary.Tools;

using CoreExtensions.Management;

using Endscript.Enums;

using Nikki.Support.Shared.Class;
using Nikki.Utils;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

		private static readonly Color _modified_light = Color.FromArgb(60, 60, 255);
		private static readonly Color _modified_dark = Color.FromArgb(160, 140, 0);
		private static readonly Color _highlight_light = Color.FromArgb(60, 255, 60);
		private static readonly Color _highlight_dark = Color.FromArgb(160, 20, 30);

		public StringEditor(STRBlock str, string path)
		{
			this.InitializeComponent();
			this.splitContainer1.FixedPanel = FixedPanel.Panel1;
			this.STR = str;
			this._strpath = path;
			this._openforms = new List<Form>();
			this._modified = new HashSet<string>();
			this.Commands = new List<string>();
			this.Text = $"{this.STR.CollectionName} Editor";
			this.StrEditorListView.Columns[^1].Width = -2;
			this.ToggleTheme();
			this.LoadListView();
			this.ToggleMenuStripControls();
		}

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            // Renderers
            this.StrEditorMenuStrip.Renderer = new Theme.MenuStripRenderer();

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
				item.SubItems.Add(Utils.UTF8toISO(record.Text));

				if (this._modified.Contains(item.SubItems[1].Text))
				{

					item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

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
			using var creator = new StringCreator();

			while (true)
			{

				if (creator.ShowDialog() == DialogResult.OK)
				{

					try
					{

						var text = Utils.ISOtoUTF8(creator.Value);
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
				this.STR.RemoveRecord(key);
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
			using var creator = new StringCreator(record);

			while (true)
			{

				if (creator.ShowDialog() == DialogResult.OK)
				{

					try
					{

						var text = Utils.ISOtoUTF8(creator.Value);
						record.SetValue("Key", creator.Key);
						record.SetValue("Label", creator.Label);
						record.SetValue("Text", text);

                        var stringKey = Hashing.BinString(Convert.ToUInt32(prev, 16), LookupReturn.NULLREF);

						this.GenerateUpdateStringCommand(stringKey ?? prev, TText, text);
						this.GenerateUpdateStringCommand(stringKey ?? prev, Label, creator.Label);
						this.GenerateUpdateStringCommand(stringKey ?? prev, Key, creator.Key);
						this._modified.Add(creator.Key);

						var item = this.StrEditorListView.SelectedItems[0];
						item.SubItems[1].Text = creator.Key;
						item.SubItems[2].Text = creator.Label;
						item.SubItems[3].Text = creator.Value;
						this.StringEditorTextBox.Text = creator.Value;
						item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

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

					this.StrEditorListView.BeginUpdate();

					for (int i = 0; i < this.StrEditorListView.Items.Count; ++i)
					{

						var item = this.StrEditorListView.Items[i];
						var key = item.SubItems[1].Text;
						var record = this.STR.GetRecord(key);

						var options = check.Value
							? RegexOptions.Multiline | RegexOptions.CultureInvariant
							: RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

						record.Text = Regex.Replace(record.Text, input.Value, with.Value, options);
						this.GenerateUpdateStringCommand(key, TText, record.Text);

						if (record.Text != item.SubItems[3].Text)
						{

							this._modified.Add(item.SubItems[1].Text);
							item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

						}

						item.SubItems[3].Text = Utils.UTF8toISO(record.Text);

					}

					this.GenericFindSelection();
					this.StrEditorListView.EndUpdate();

				}

			}

		}

		private void FindPreviousToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Considering it is activated
			if (this.StrEditorListView.Items.Count == 0) return;

			var index = this.StrEditorListView.SelectedIndices.Count == 0
				? this.StrEditorListView.Items.Count - 1 : this.StrEditorListView.SelectedIndices[0] - 1;

			var match = Configurations.Default.DarkTheme ? _highlight_dark : _highlight_light;

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

			var match = Configurations.Default.DarkTheme ? _highlight_dark : _highlight_light;

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
			if (this.StrEditorListView.SelectedItems.Count == 0)
			{

				this.StringEditorTextBox.Text = String.Empty;
				this.ToggleMenuStripControls();
				return;

			}

			var key = this.GetSelectedKey();
			var record = this.STR.GetRecord(key);

			if (record == null) return;

			this.StringEditorTextBox.Text = this.StrEditorListView.SelectedItems[0].SubItems[3].Text;
			this.ToggleMenuStripControls();
		}

		private void StrEditorListView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.EditStringToolStripMenuItem_Click(this.EditStringToolStripMenuItem, EventArgs.Empty);
		}

		private void StrEditorListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
            Theme.Deserialize(Path.Combine("Themes", Configurations.Default.ThemeFile), out var theme);
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

						item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

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

						item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

					}

					if (item.SubItems[1].Text.ToUpperInvariant().Contains(find))
					{

						item.BackColor = Configurations.Default.DarkTheme ? _highlight_dark : _highlight_light;

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

						item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

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

						item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

					}

					if (item.SubItems[2].Text.ToUpperInvariant().Contains(find))
					{

						item.BackColor = Configurations.Default.DarkTheme ? _highlight_dark : _highlight_light;

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

						item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

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

						item.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

					}

					if (item.SubItems[3].Text.ToUpperInvariant().Contains(find))
					{

						item.BackColor = Configurations.Default.DarkTheme ? _highlight_dark : _highlight_light;

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

			if (selected.SubItems[3].Text != this.StringEditorTextBox.Text)
			{

				this._modified.Add(selected.SubItems[1].Text);
				selected.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;

			}

			selected.SubItems[3].Text = this.StringEditorTextBox.Text ?? String.Empty;
			record.Text = Utils.ISOtoUTF8(selected.SubItems[3].Text);

			var upper = record.Text.ToUpperInvariant();

			if (!String.IsNullOrEmpty(this.TextBoxText.Text))
			{

				selected.BackColor = !upper.Contains(this.TextBoxText.Text.ToUpperInvariant())
					? this.StrEditorListView.BackColor
					: Configurations.Default.DarkTheme
						? _highlight_dark
						: _highlight_light;

			}
		}

		private void StringEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.StringEditorTextBox.ContainsFocus)
			{

				this.StringEditorTextBox_Validated(this.StringEditorTextBox, EventArgs.Empty);

			}

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
			if (value.Contains(' ')) value = $"\"{value}\"";
			var command = $"{eCommandType.update_string} {this._strpath} {key} {property} {value}";
			this.Commands.Add(command);
		}

		private void GenerateAddStringCommand(string key, string label, string text)
		{
			if (label.Contains(' ')) label = $"\"{label}\"";
			if (text.Contains(' ')) text = $"\"{text}\"";
			var command = $"{eCommandType.add_string} {this._strpath} {key} {label} {text}";
			this.Commands.Add(command);
		}

		private void GenerateRemoveStringCommand(string key)
		{
			var command = $"{eCommandType.remove_string} {this._strpath} {key}";
			this.Commands.Add(command);
		}

		#endregion

		private void StringEditorTextBox_Validated(object sender, EventArgs e)
		{
			// Considering it is enabled because string record was selected

			var key = this.GetSelectedKey();
			var record = this.STR.GetRecord(key);

			if (record == null) return;
			if (this.StrEditorListView.SelectedItems.Count == 0) return;

			var selected = this.StrEditorListView.SelectedItems[0];

			if (selected.SubItems[3].Text == this.StringEditorTextBox.Text) return;

			this.GenerateUpdateStringCommand(selected.SubItems[1].Text, TText, record.Text);
			selected.BackColor = Configurations.Default.DarkTheme ? _modified_dark : _modified_light;
			this._modified.Add(selected.SubItems[1].Text);
		}
	}
}
