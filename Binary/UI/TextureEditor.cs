﻿using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Binary.Tools;
using Binary.Prompt;
using Binary.Interact;
using Binary.Properties;
using Nikki.Utils;
using Nikki.Utils.EA;
using Nikki.Reflection.Enum;
using Nikki.Support.Shared.Class;
using ILWrapper.Enums;
using Endscript.Enums;
using CoreExtensions.Management;
using CoreExtensions.Text;



namespace Binary.UI
{
	public partial class TextureEditor : Form
	{
		private TPKBlock TPK { get; }
		private readonly List<Form> _openforms;
		private readonly string _tpkpath;
		public List<string> Commands { get; }
		private int _last_column_clicked = -1;

		public TextureEditor(TPKBlock tpk, string path)
		{
			this.InitializeComponent();
			this.TPK = tpk;
			this._tpkpath = path;
			this._openforms = new List<Form>();
			this.Commands = new List<string>();
			this.Text = $"{this.TPK.CollectionName} Editor";
			this.TexEditorImage.BackColor = Color.FromArgb(0, 0, 0, 0);
			this.TexEditorImage.BorderStyle = BorderStyle.FixedSingle;
			this.TexEditorImage.Width = this.panel1.Width;
			this.TexEditorImage.Height = this.panel1.Height;
			this.TexEditorListView.Columns[^1].Width = -2;
			this.ToggleTheme();
			this.LoadListView();
			this.ToggleMenuStripControls();
			
		}

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            // Renderers
            this.TexEditorMenuStrip.Renderer = new Theme.MenuStripRenderer();

			// Primary colors and controls
			this.BackColor = theme.Colors.MainBackColor;
			this.ForeColor = theme.Colors.MainForeColor;

			// Image
			this.panel1.BackgroundImage = Configurations.Default.DarkTheme
				? Resources.DarkTransparent : Resources.LightTransparent;

			// List view
			this.TexEditorListView.BackColor = theme.Colors.PrimBackColor;
			this.TexEditorListView.ForeColor = theme.Colors.PrimForeColor;

			// Property grid
			this.TexEditorPropertyGrid.BackColor = theme.Colors.PrimBackColor;
			this.TexEditorPropertyGrid.CategorySplitterColor = theme.Colors.ButtonBackColor;
			this.TexEditorPropertyGrid.CategoryForeColor = theme.Colors.TextBoxForeColor;
			this.TexEditorPropertyGrid.CommandsBackColor = theme.Colors.PrimBackColor;
			this.TexEditorPropertyGrid.CommandsForeColor = theme.Colors.PrimForeColor;
			this.TexEditorPropertyGrid.CommandsBorderColor = theme.Colors.PrimBackColor;
			this.TexEditorPropertyGrid.DisabledItemForeColor = theme.Colors.LabelTextColor;
			this.TexEditorPropertyGrid.LineColor = theme.Colors.ButtonBackColor;
			this.TexEditorPropertyGrid.SelectedItemWithFocusBackColor = theme.Colors.FocusedBackColor;
			this.TexEditorPropertyGrid.SelectedItemWithFocusForeColor = theme.Colors.FocusedForeColor;
			this.TexEditorPropertyGrid.ViewBorderColor = theme.Colors.RegBorderColor;
			this.TexEditorPropertyGrid.ViewBackColor = theme.Colors.PrimBackColor;
			this.TexEditorPropertyGrid.ViewForeColor = theme.Colors.PrimForeColor;

			// Menu strip and menu items
			this.TexEditorMenuStrip.ForeColor = theme.Colors.LabelTextColor;
			
			this.TexEditorAddTextureItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorAddTextureItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorCopyTextureItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorCopyTextureItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorExportAllItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorExportAllItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorExportTextureItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorExportTextureItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorFindReplaceItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorFindReplaceItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorImportFromItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorImportFromItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorHasherItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorHasherItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorRaiderItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorRaiderItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorRemoveTextureItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorRemoveTextureItem.ForeColor = theme.Colors.MenuItemForeColor;
			this.TexEditorReplaceTextureItem.BackColor = theme.Colors.MenuItemBackColor;
			this.TexEditorReplaceTextureItem.ForeColor = theme.Colors.MenuItemForeColor;
		}

		#endregion

		#region Methods

		private void LoadListView(int index = -1)
		{
			this.TexEditorListView.Items.Clear();
			var list = this.TPK.GetTextures();
			this.TexEditorListView.BeginUpdate();

			var count = 0;

			foreach (var texture in list)
			{

				var item = new ListViewItem
				{
					Text = (count++).ToString()
				};

				var asString = texture.Compression.ToString();

				var compression = asString.Length > 8 ? asString.Substring(8) : asString;
				item.SubItems.Add($"0x{texture.BinKey:X8}");
				item.SubItems.Add(texture.CollectionName);
				item.SubItems.Add(compression);

				if (texture.BinKey != texture.CollectionName.BinHash())
				{

					item.BackColor = Configurations.Default.DarkTheme
						? Color.FromArgb(70, 0, 20) : Color.FromArgb(255, 100, 100);

				}

				this.TexEditorListView.Items.Add(item);

			}

			this.TexEditorListView.EndUpdate();

			if (index < 0 || index >= this.TexEditorListView.Items.Count) return;

			this.TexEditorListView.Items[index].Selected = true;
			this.TexEditorListView.Select();
			this.TexEditorListView.HideSelection = false;
			this.TexEditorListView.Items[index].EnsureVisible();
		}

		private void ToggleMenuStripControls()
		{
			if (this.TexEditorListView.SelectedItems.Count == 0)
			{

				this.TexEditorRemoveTextureItem.Enabled = false;
				this.TexEditorCopyTextureItem.Enabled = false;
				this.TexEditorReplaceTextureItem.Enabled = false;
				this.TexEditorExportTextureItem.Enabled = false;

			}
			else
			{

				this.TexEditorRemoveTextureItem.Enabled = true;
				this.TexEditorCopyTextureItem.Enabled = true;
				this.TexEditorReplaceTextureItem.Enabled = true;
				this.TexEditorExportTextureItem.Enabled = true;

			}
		}

		private uint GetSelectedKey()
		{
			return this.TexEditorListView.SelectedItems.Count == 0
				? UInt32.MaxValue
				: (this.TexEditorListView.SelectedItems[0].SubItems[1].Text.TryHexStringToUInt32(out uint result) ? result : 0);
		}

		private void DisposeImage()
		{
			if (this.TexEditorImage.Image != null)
			{

				var disposer = this.TexEditorImage.Image;
				this.TexEditorImage.Image = null;
				disposer.Dispose();

			}
		}

		#endregion

		#region Menu Strip

		private void TexEditorAddTextureItem_Click(object sender, EventArgs e)
		{
			if (this.AddTextureDialog.ShowDialog() == DialogResult.OK)
			{

				var initial = Path.GetFileNameWithoutExtension(this.AddTextureDialog.FileName);
				using var input = new Input("Enter name of the new texture", null, null, initial);

				while (true) // use loop instead of recursion to prevent stack overflow
				{

					if (input.ShowDialog() == DialogResult.OK)
					{

						try
						{

							this.TPK.AddTexture(input.Value, this.AddTextureDialog.FileName);

							if (this.TexEditorListView.SelectedIndices.Count > 0)
							{

								this.LoadListView(this.TexEditorListView.SelectedIndices[0]);

							}
							else this.LoadListView();
							this.GenerateAddTextureCommand(input.Value, this.AddTextureDialog.FileName);
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
		}

		private void TexEditorRemoveTextureItem_Click(object sender, EventArgs e)
		{
			try
			{

				var index = this.TexEditorListView.SelectedIndices[0];
				var key = this.GetSelectedKey();
				this.TPK.RemoveTexture(key, KeyType.BINKEY);
				this.GenerateRemoveTextureCommand(this.TexEditorListView.Items[index].SubItems[1].Text);

				if (this.TPK.TextureCount == 0)
				{

					this.LoadListView();
					this.TexEditorPropertyGrid.SelectedObject = null;
					this.DisposeImage();
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

		private void TexEditorCopyTextureItem_Click(object sender, EventArgs e)
		{
			using var input = new Input("Enter name of the new texture");

			while (true) // use loop instead of recursion to prevent stack overflow
			{

				if (input.ShowDialog() == DialogResult.OK)
				{

					try
					{

						var index = this.TexEditorListView.SelectedIndices[0];
						var key = this.GetSelectedKey();
						this.TPK.CloneTexture(input.Value, key, KeyType.BINKEY);
						this.GenerateCopyTextureCommand(this.TexEditorListView.Items[index].SubItems[1].Text, input.Value);
						this.LoadListView(index);
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

		private void TexEditorReplaceTextureItem_Click(object sender, EventArgs e)
		{
			this.ReplaceTextureDialog.ShowDialog();
		}

		private void TexEditorExportTextureItem_Click(object sender, EventArgs e)
		{
			string FilterExt = "Direct Draw Surface files|*.dds|";
			FilterExt += "Portable Network Graphics files|*.png|";
			FilterExt += "Joint Photographic Group files|*.jpg|";
			FilterExt += "Bitmap Pixel Format files|*.bmp";

			var index = this.TexEditorListView.SelectedIndices[0];
			var key = this.GetSelectedKey();
			var texture = this.TPK.FindTexture(key, KeyType.BINKEY);

			this.ExportTextureDialog.Filter = FilterExt;
			this.ExportTextureDialog.FileName = texture.CollectionName;

			if (this.ExportTextureDialog.ShowDialog() == DialogResult.OK)
			{

				try
				{

					string path = this.ExportTextureDialog.FileName;
					string last = Path.GetExtension(path).ToUpperInvariant()[1..];
					var ext = (ImageType)Enum.Parse(typeof(ImageType), last);

					if (ext == ImageType.DDS)
					{

						using var bw = new BinaryWriter(File.Open(path, FileMode.Create));
						bw.Write(texture.GetDDSArray(false));

					}
					else
					{

						var data = texture.GetDDSArray(true);
						var image = new ILWrapper.Image(data);
						image.Save(path, ext);

					}

				}
				catch (Exception ex)
				{

					MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				}

			}
		}

		private void TexEditorExportAllItem_Click(object sender, EventArgs e)
		{
			using var browser = new FolderBrowserDialog()
			{
				AutoUpgradeEnabled = false,
				Description = "Select directory where all textures should be exported.",
				RootFolder = Environment.SpecialFolder.MyComputer,
				ShowNewFolderButton = true,
			};

			if (browser.ShowDialog() == DialogResult.OK)
			{

				var textures = this.TPK.GetTextures() as IEnumerable;
				foreach (Texture texture in textures)
				{

					var path = Path.Combine(browser.SelectedPath, texture.CollectionName) + ".dds";
					var data = texture.GetDDSArray(false);
					using var bw = new BinaryWriter(File.Open(path, FileMode.Create));
					bw.Write(data);

				}

				MessageBox.Show($"All textures have been exported", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

			}
		}

		private void TexEditorImportFromItem_Click(object sender, EventArgs e)
		{
			using var importer = new Importer()
			{
				StartPosition = FormStartPosition.CenterScreen
			};

			if (importer.ShowDialog() == DialogResult.OK)
			{

				using var browser = new FolderBrowserDialog()
				{
					AutoUpgradeEnabled = false,
					Description = "Select directory to import textures from.",
					RootFolder = Environment.SpecialFolder.MyComputer,
					ShowNewFolderButton = false,
				};

				if (browser.ShowDialog() == DialogResult.OK)
				{

					try
					{

						var type = (SerializeType)importer.SerializationIndex;

						foreach (var file in Directory.GetFiles(browser.SelectedPath))
						{

							var name = Path.GetFileNameWithoutExtension(file);
							var key = name.BinHash();
							var texture = this.TPK.FindTexture(key, KeyType.BINKEY);

							if (texture is null)
							{

								this.TPK.AddTexture(name, file);

							}
							else if (type == SerializeType.Synchronize)
							{

								texture.Reload(file);

							}
							else if (type == SerializeType.Override)
							{

								this.TPK.RemoveTexture(key, KeyType.BINKEY);
								this.TPK.AddTexture(name, file);
	
							}

						}

						MessageBox.Show($"All textures have been imported", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
						this.GenerateBindTexturesCommand(type, browser.SelectedPath);
						if (this.TexEditorListView.SelectedIndices.Count == 0) this.LoadListView();
						else this.LoadListView(this.TexEditorListView.SelectedIndices[0]);

					}
					catch (Exception ex)
					{

						MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					}

				}

			}
		}

		private void TexEditorFindReplaceItem_Click(object sender, EventArgs e)
		{
			if (this.TexEditorListView.Items.Count == 0) return;

			using var with = new Input("Enter string to replace with");
			using var input = new Input("Enter string to search for",
										new Predicate<string>(_ => !String.IsNullOrEmpty(_)),
										"Input string cannot be null or empty");

			if (input.ShowDialog() == DialogResult.OK && with.ShowDialog() == DialogResult.OK)
			{

				using var check = new Check("Make case-sensitive replace?", false);

				if (check.ShowDialog() == DialogResult.OK)
				{

					var options = check.Value
						? RegexOptions.Multiline | RegexOptions.CultureInvariant
						: RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

					this.TexEditorListView.BeginUpdate();

					for (int i = 0; i < this.TPK.TextureCount; ++i)
					{

						var texture = this.TPK.Textures[i];
						var key = $"0x{texture.BinKey:X8}";
						var oname = texture.CollectionName;
						if (oname.Contains(' ')) oname = $"\"{oname}\"";

						if (texture.BinKey != texture.CollectionName.BinHash()) continue;
						var cname = Regex.Replace(texture.CollectionName, input.Value, with.Value, options);
						if (cname == texture.CollectionName) continue;

						texture.CollectionName = cname;
						this.TexEditorListView.Items[i].SubItems[1].Text = $"0x{texture.BinKey:X8}";
						this.TexEditorListView.Items[i].SubItems[2].Text = texture.CollectionName;

						this.GenerateUpdateTextureCommand(oname, "CollectionName", cname);

					}

					this.TexEditorListView.EndUpdate();
					this.TexEditorPropertyGrid.Refresh();

				}

			}
		}

		private void TexEditorHasherItem_Click(object sender, EventArgs e)
		{
			var hasher = new Hasher() { StartPosition = FormStartPosition.CenterScreen };
			this._openforms.Add(hasher);
			hasher.Show();
		}

		private void TexEditorRaiderItem_Click(object sender, EventArgs e)
		{
			var raider = new Raider() { StartPosition = FormStartPosition.CenterScreen };
			this._openforms.Add(raider);
			raider.Show();
		}

		#endregion

		#region List View

		private void TexEditorListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.TexEditorListView.SelectedItems.Count == 0)
			{

				this.TexEditorPropertyGrid.SelectedObject = null;
				this.DisposeImage();
				this.panel1.AutoScroll = false;
				this.TexEditorImage.Width = this.panel1.Width;
				this.TexEditorImage.Height = this.panel1.Height;
				this.ToggleMenuStripControls();
				return;

			}

			var item = this.TexEditorListView.SelectedItems[0];
			var key = Convert.ToUInt32(item.SubItems[1].Text, 16);

			var texture = this.TPK.FindTexture(key, KeyType.BINKEY);

			if (texture == null) return;

			this.TexEditorPropertyGrid.SelectedObject = texture;

			try
			{

				var data = texture.GetDDSArray(true);
				var image = new ILWrapper.Image(data);

				using var ms = new MemoryStream();
				image.Save(ms, ImageType.PNG);

				this.DisposeImage();

				this.TexEditorImage.BorderStyle = BorderStyle.None;
				this.TexEditorImage.Image = Image.FromStream(ms);
				this.TexEditorImage.BorderStyle = BorderStyle.FixedSingle;
				this.panel1.AutoScroll = true;

			}
			catch (Exception ex)
			{

				MessageBox.Show($"Unable to preview texture: {ex.GetLowestMessage()}", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);

			}

			this.ToggleMenuStripControls();
		}

		private void TexEditorListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			e.NewWidth = this.TexEditorListView.Columns[e.ColumnIndex].Width;
			e.Cancel = true;
		}

		private void TexEditorListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
            Theme.Deserialize(Path.Combine("Themes", Configurations.Default.ThemeFile), out var theme);
            var brush = new SolidBrush(theme.Colors.TextBoxBackColor);
            e.Graphics.FillRectangle(brush, e.Bounds);
			e.DrawText();
		}

		private void TexEditorListView_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void TexEditorListView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			uint key;
			int index;

			if (this.TexEditorPropertyGrid.SelectedObject is null)
			{

				key = 0xFFFFFFFF;
				index = -1;

			}
			else
			{

				key = ((Texture)this.TexEditorPropertyGrid.SelectedObject).BinKey;
				index = 0;

			}

			switch (e.Column)
			{
				case 1: // BinKey
					this.TPK.SortTexturesByType(false);

					if (this._last_column_clicked == 1)
					{

						this.TPK.Textures.Reverse();
						this._last_column_clicked = -1;

					}
					else this._last_column_clicked = 1;
						
					if (index == 0) index = this.TPK.GetTextureIndex(key, KeyType.BINKEY);
					this.LoadListView(index);
					break;

				case 2: // CollectionName
					this.TPK.SortTexturesByType(true);
					
					if (this._last_column_clicked == 2)
					{

						this.TPK.Textures.Reverse();
						this._last_column_clicked = -1;

					}
					else this._last_column_clicked = 2;

					if (index == 0) index = this.TPK.GetTextureIndex(key, KeyType.BINKEY);
					this.LoadListView(index);
					break;

				default:
					break;

			}
		}

		#endregion

		#region Events

		private void TexEditorPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			var key = this.TexEditorListView.SelectedItems[0].SubItems[1].Text;

			if (e.ChangedItem.Label == nameof(Texture.ClassName) ||
				e.ChangedItem.Label == nameof(Texture.ClassKey) ||
				e.ChangedItem.Label == nameof(Texture.MipmapBiasInt) ||
				e.ChangedItem.Label == nameof(Texture.MipmapBiasType))
			{

				this.TexEditorPropertyGrid.Refresh();

			}
			else if (e.ChangedItem.Label == nameof(Texture.CollectionName))
			{

				var name = e.ChangedItem.Value.ToString();
				this.TexEditorListView.SelectedItems[0].SubItems[1].Text = name.BinHash().FastToHexString(false);
				this.TexEditorListView.SelectedItems[0].SubItems[2].Text = name;
				this.TexEditorPropertyGrid.Refresh();

			}

			this.GenerateUpdateTextureCommand(key, e.ChangedItem.Label, e.ChangedItem.Value.ToString());
		}

		private void AddTextureDialog_FileOk(object sender, CancelEventArgs e)
		{
			try
			{

				if (!Comp.IsDDSTexture(this.AddTextureDialog.FileName, out string error))
				{

					throw new Exception(error);

				}

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				e.Cancel = true;

			}
		}

		private void ReplaceTextureDialog_FileOk(object sender, CancelEventArgs e)
		{
			var hash = this.TexEditorListView.SelectedItems[0].SubItems[1].Text;
			var key = Convert.ToUInt32(hash, 16);

			try
			{

				if (!Comp.IsDDSTexture(this.ReplaceTextureDialog.FileName, out string error))
				{

					throw new Exception(error);

				}

				var texture = this.TPK.FindTexture(key, KeyType.BINKEY);
				texture.Reload(this.ReplaceTextureDialog.FileName);
				this.LoadListView(this.TexEditorListView.SelectedIndices[0]);
				this.GenerateReplaceTextureCommand(hash, this.ReplaceTextureDialog.FileName);

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				e.Cancel = true;

			}
		}

		private void TextureEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.DisposeImage();

			foreach (var form in this._openforms)
			{

				try { form.Close(); }
				catch { }

			}
		}

		#endregion

		#region Commands

		private void GenerateUpdateTextureCommand(string key, string property, string value)
		{
			if (property.Contains(' ')) property = $"\"{property}\"";
			if (value.Contains(' ')) property = $"\"{value}\"";
			var command = $"{eCommandType.update_texture} {this._tpkpath} {key} {property} {value}";
			this.Commands.Add(command);
		}

		private void GenerateAddTextureCommand(string name, string file)
		{
			if (name.Contains(' ')) name = $"\"{name}\"";
			if (file.Contains(' ')) file = $"\"{file}\"";
			var command = $"{eCommandType.add_texture} {this._tpkpath} {name} {file}";
			this.Commands.Add(command);
		}

		private void GenerateRemoveTextureCommand(string key)
		{
			var command = $"{eCommandType.remove_texture} {this._tpkpath} {key}";
			this.Commands.Add(command);
		}

		private void GenerateCopyTextureCommand(string key, string name)
		{
			var command = $"{eCommandType.copy_texture} {this._tpkpath} {key} {name}";
			this.Commands.Add(command);
		}

		private void GenerateReplaceTextureCommand(string key, string file)
		{
			if (file.Contains(' ')) file = $"\"{file}\"";
			var command = $"{eCommandType.replace_texture} {this._tpkpath} {key} {file}";
			this.Commands.Add(command);
		}

		private void GenerateBindTexturesCommand(SerializeType type, string directory)
		{
			string import = type.ToString().ToLowerInvariant();
			if (directory.Contains(' ')) directory = $"\"{directory}\"";
			var command = $"{eCommandType.bind_textures} {import} {this._tpkpath} {directory}";
			this.Commands.Add(command);
		}

		#endregion
	}
}
