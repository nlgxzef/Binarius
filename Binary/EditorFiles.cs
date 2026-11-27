using Binary.Prompt;
using Binary.Properties;

using CoreExtensions.Management;

using Endscript.Core;

using Nikki.Reflection.Enum;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Binary
{
    /// <summary>
    /// Game file management in the editor: drag &amp; drop to open, and the tree context menu
    /// entries to add, rename (copy/move), close and delete game files. Every operation is
    /// also written to the End command prompt as equivalent Endscript commands.
    /// </summary>
    public partial class Editor
    {
        private ToolStripSeparator _fileSeparator;
        private ToolStripMenuItem _fileAddItem;
        private ToolStripMenuItem _fileCreateItem;
        private ToolStripMenuItem _fileRenameItem;
        private ToolStripMenuItem _fileCloseItem;
        private ToolStripMenuItem _fileDeleteItem;

        static string m_addGameFileLastDir = null;

        #region Setup

        private void InitializeFileFeatures()
        {
            this._fileSeparator = new ToolStripSeparator();
            this._fileAddItem = new ToolStripMenuItem("Add File...", null, this.FileAddItem_Click);
            this._fileCreateItem = new ToolStripMenuItem("Create Empty File...", null, this.FileCreateItem_Click);
            this._fileRenameItem = new ToolStripMenuItem("Rename File...", null, this.FileRenameItem_Click);
            this._fileCloseItem = new ToolStripMenuItem("Close File", null, this.FileCloseItem_Click);
            this._fileDeleteItem = new ToolStripMenuItem("Delete from Disk...", null, this.FileDeleteItem_Click);

            this.EditorContextMenu.Items.AddRange(new ToolStripItem[]
            {
                this._fileSeparator, this._fileAddItem, this._fileCreateItem, this._fileRenameItem, this._fileCloseItem, this._fileDeleteItem,
            });

            this.EditorContextMenu.Opening += this.EditorContextMenu_OpeningFiles;

            // Accept dropped files anywhere on the editor window. Windows blocks dragging from Explorer into
            // a program running as administrator, except through the older WM_DROPFILES message, so an
            // elevated editor uses that instead (set up once the window exists).
            if (!SaveLocation.IsElevated()) this.EnableFileDrop(this);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!SaveLocation.IsElevated()) return;

            // Let the non-elevated Explorer send the drop messages to this window
            foreach (var message in new uint[] { WM_DROPFILES, WM_COPYDATA, WM_COPYGLOBALDATA })
            {
                ChangeWindowMessageFilterEx(this.Handle, message, MSGFLT_ALLOW, IntPtr.Zero);
            }

            DragAcceptFiles(this.Handle, true);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DROPFILES)
            {
                var paths = ReadDroppedFiles(m.WParam);
                m.Result = IntPtr.Zero;
                if (paths.Length > 0) this.BeginInvoke(new Action(() => this.OpenGameFiles(paths)));
                return;
            }

            base.WndProc(ref m);
        }

        private static string[] ReadDroppedFiles(IntPtr drop)
        {
            try
            {
                uint count = DragQueryFile(drop, 0xFFFFFFFF, null, 0);
                var paths = new string[count];

                for (uint i = 0; i < count; ++i)
                {
                    var buffer = new System.Text.StringBuilder((int)DragQueryFile(drop, i, null, 0) + 1);
                    DragQueryFile(drop, i, buffer, (uint)buffer.Capacity);
                    paths[i] = buffer.ToString();
                }

                return paths;
            }
            finally
            {
                DragFinish(drop);
            }
        }

        private const uint WM_DROPFILES = 0x0233;
        private const uint WM_COPYDATA = 0x004A;
        private const uint WM_COPYGLOBALDATA = 0x0049;
        private const uint MSGFLT_ALLOW = 1;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool ChangeWindowMessageFilterEx(IntPtr hwnd, uint message, uint action, IntPtr changeInfo);

        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        private static extern void DragAcceptFiles(IntPtr hwnd, bool accept);

        [System.Runtime.InteropServices.DllImport("shell32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern uint DragQueryFile(IntPtr drop, uint index, System.Text.StringBuilder file, uint length);

        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        private static extern void DragFinish(IntPtr drop);

        private void ApplyFileMenuTheme(Theme theme)
        {
            if (this._fileAddItem is null) return;

            foreach (ToolStripItem item in new ToolStripItem[]
            {
                this._fileSeparator, this._fileAddItem, this._fileCreateItem, this._fileRenameItem, this._fileCloseItem, this._fileDeleteItem,
            })
            {
                item.BackColor = theme.Colors.MenuItemBackColor;
                item.ForeColor = theme.Colors.MenuItemForeColor;
            }
        }

        private void EnableFileDrop(Control control)
        {
            control.AllowDrop = true;
            control.DragEnter += this.Editor_FileDragEnter;
            control.DragOver += this.Editor_FileDragEnter;
            control.DragDrop += this.Editor_FileDragDrop;

            foreach (Control child in control.Controls) this.EnableFileDrop(child);
        }

        private void EditorContextMenu_OpeningFiles(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var loaded = this.Profile != null;
            var fileNode = loaded && this.EditorTreeView.SelectedNode is TreeNode node && node.Parent is null;

            this._fileAddItem.Enabled = loaded;
            this._fileCreateItem.Enabled = loaded;
            this._fileRenameItem.Enabled = fileNode;
            this._fileCloseItem.Enabled = fileNode;
            this._fileDeleteItem.Enabled = fileNode;
        }

        #endregion

        #region Drag & Drop

        private void Editor_FileDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Editor_FileDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is not string[] paths || paths.Length == 0) return;

            // Run after the drop completes so Explorer is not blocked while dialogs are open
            this.BeginInvoke(new Action(() => this.OpenGameFiles(paths)));
        }

        #endregion

        #region Opening

        private void FileAddItem_Click(object sender, EventArgs e)
        {
            if (this.Profile is null) return;

            using var dialog = new OpenFileDialog()
            {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Game Files|*.bin;*.bun;*.lzc|All Files|*.*",
                Multiselect = true,
                Title = "Add Files",
                InitialDirectory = m_addGameFileLastDir ?? (SaveRedirect.Active ? SaveRedirect.Output : this.Profile.Directory),
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            m_addGameFileLastDir = Path.GetDirectoryName(dialog.FileName);
            this.OpenGameFiles(dialog.FileNames);
        }

        /// <summary>
        /// Opens game files given as full paths. Files must be inside the game directory of the
        /// loaded launcher and readable by Binarius. Asks whether to add opened files to the launcher.
        /// </summary>
        internal void OpenGameFiles(IEnumerable<string> paths)
        {
            if (this.Profile is null)
            {
                this.ShowMessage("Open Game Files", "Loading loose files is not supported. Please load a launcher first (Main > Load Files). Dropped files must belong to the game folder of the loaded launcher.");
                return;
            }

            var opened = new List<SynchronizedDatabase>();
            var problems = new List<string>();
            string select = null;
            string alreadyOpen = null;

            Cursor.Current = Cursors.WaitCursor;

            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    problems.Add($"{Path.GetFileName(path)}: folders cannot be opened, drop the game files instead.");
                    continue;
                }

                var rel = this.GetRelativeGamePath(path);

                if (rel is null)
                {
                    problems.Add($"{Path.GetFileName(path)}: the file is not inside the game folder of the loaded launcher ({this.Profile.Directory}).");
                    continue;
                }

                var existing = this.Profile.Find(rel);

                if (existing != null)
                {
                    alreadyOpen = existing.Filename;
                    problems.Add($"{existing.Filename} is already open.");
                    continue;
                }

                if (!this.TryLoadGameFile(rel, out var sdb, out var error))
                {
                    problems.Add(error);
                    continue;
                }

                this.Profile.Add(sdb);
                opened.Add(sdb);
                select = sdb.Filename;
            }

            Cursor.Current = Cursors.Default;

            if (opened.Count > 0)
            {
                this.LoadTreeView(select);
                this.ToggleControlsAfterLoad(true);
                if (Configurations.Default.AutoBackups) this.CreateBackupsForFiles(false);
                this.WriteLineToEndCommandPrompt(opened.Select(_ => $"new negate {Quote(_.Filename)}"));
                this.EditorStatusLabel.Text = $"Opened {String.Join(", ", opened.Select(_ => _.Filename))}";
            }
            else if (alreadyOpen != null)
            {
                this.LoadTreeView(alreadyOpen);
            }

            if (problems.Count > 0)
            {
                this.ShowMessage("Open Game Files", String.Join(Environment.NewLine + Environment.NewLine, problems));
            }

            if (opened.Count > 0)
            {
                this.WarnEmptyFiles(opened);
                this.AskAddToLauncher(opened.Select(_ => _.Filename).ToArray());
            }
        }

        /// <summary>
        /// Tells which of the files were empty on disk when they were opened.
        /// </summary>
        internal void WarnEmptyFiles(IEnumerable<SynchronizedDatabase> files)
        {
            var empty = files.Where(_ => IsEmptyOnDisk(_.Filename)).Select(_ => _.Filename).ToList();
            if (empty.Count == 0) return;

            var list = String.Join(Environment.NewLine, empty.Select(_ => "  " + _));
            this.ShowMessage("Empty File", (empty.Count == 1 ? $"{empty[0]} is empty." : $"These files are empty:{Environment.NewLine}{list}") +
                Environment.NewLine + Environment.NewLine +
                "They are open with nothing in them. Check that this is the file you meant: the game may need its data. " +
                "Saving writes whatever you add to them.");
        }

        private bool IsEmptyOnDisk(string rel)
        {
            try
            {
                var full = SaveRedirect.Read(Path.Combine(this.Profile.Directory, rel));
                return File.Exists(full) && new FileInfo(full).Length == 0;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
            {
                return false;
            }
        }

        // Puts a 'new' command for the user to name the file and run, which creates the file and opens it
        private void FileCreateItem_Click(object sender, EventArgs e)
        {
            if (this.Profile is null) return;

            // In the folder of the file the selection is in
            var root = this.EditorTreeView.SelectedNode;
            while (root?.Parent != null) root = root.Parent;
            var folder = root != null && this.Profile.Find(root.Text) is SynchronizedDatabase selected
                ? Path.GetDirectoryName(selected.Filename) : String.Empty;
            var name = String.IsNullOrEmpty(folder) ? "NEW_FILE.BIN" : Path.Combine(folder, "NEW_FILE.BIN");
            var line = $"new negate \"{name}\"";

            this.WriteLineToEndCommandPrompt(line);

            // Select the name, so typing replaces it
            var prompt = this.EditorCommandPrompt;
            var at = prompt.Text.LastIndexOf(line, StringComparison.Ordinal) + "new negate \"".Length;
            prompt.Focus();
            prompt.Select(at, name.Length);
            this.EditorStatusLabel.Text = "Type the name of the new file (a path in the game folder) and run the line. 'negate' leaves a file that already exists as it is.";
        }

        private void AskAddToLauncher(string[] rels)
        {
            var missing = rels.Where(_ => !this.LauncherContains(_)).ToArray();
            if (missing.Length == 0) return;

            var list = String.Join(Environment.NewLine, missing.Select(_ => "  " + _));
            var text = missing.Length == 1
                ? $"Add {missing[0]} to the launcher, so it opens automatically next time?"
                : $"Add these files to the launcher, so they open automatically next time?{Environment.NewLine}{list}";

            using var prompt = new Choice("Add to Launcher", text, new[] { "Add to launcher", "Not now" }, 0, 1);
            prompt.ShowDialog(this);

            if (prompt.Value == 0) this.EditLauncher(files =>
            {
                foreach (var rel in missing) files.Add(rel);
                return true;
            });
        }

        /// <summary>
        /// Loads a game file into a new <see cref="SynchronizedDatabase"/> without adding it to the profile.
        /// Rejects files Binarius cannot read.
        /// </summary>
        private bool TryLoadGameFile(string rel, out SynchronizedDatabase sdb, out string error)
        {
            sdb = null;
            error = null;
            var full = SaveRedirect.Read(Path.Combine(this.Profile.Directory, rel));

            if (!File.Exists(full))
            {
                error = $"{rel}: the file does not exist.";
                return false;
            }

            try
            {
                if (!IsGameDataFile(full, out var reason))
                {
                    error = $"{rel}: Binarius cannot read this file ({reason}).";
                    return false;
                }

                var result = new SynchronizedDatabase(this.Profile.GameINT, this.Profile.Directory, rel);
                result.Load();
                sdb = result;
                return true;
            }
            catch (Exception ex)
            {
                error = $"{rel}: Binarius cannot read this file ({ex.GetLowestMessage()}).";
                return false;
            }
        }

        /// <summary>
        /// Quick structure check so files Binarius cannot edit (text files, vault/attribute files)
        /// are not loaded and later overwritten on save. Empty files pass. Valid files are either
        /// compressed, or consist of data blocks that exactly fill the file.
        /// </summary>
        internal static bool IsGameDataFile(string path, out string reason)
        {
            reason = null;
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var length = fs.Length;

            // Empty files are opened, with a warning (see WarnEmptyFiles)
            if (length == 0) return true;

            using var br = new BinaryReader(fs);

            if (length >= 4 && Enum.IsDefined(typeof(LZCompressionType), br.ReadInt32())) return true;

            long position = 0;

            while (position + 8 <= length)
            {
                fs.Position = position + 4;
                var size = br.ReadInt32();

                if (size < 0 || position + 8 + size > length)
                {
                    reason = "it is not a game data file Binarius can edit";
                    return false;
                }

                position += 8 + size;
            }

            if (position != length)
            {
                reason = "it is not a game data file Binarius can edit";
                return false;
            }

            return true;
        }

        #endregion

        #region Rename / Copy / Move

        private void FileRenameItem_Click(object sender, EventArgs e)
        {
            var sdb = this.GetSelectedFile();
            if (sdb is null) return;

            if (!this.EnsureSavedBefore("rename or copy a file")) return;

            var oldRel = sdb.Filename;
            var oldFull = sdb.FullPath;

            using (var input = new Input("Enter the new file name, relative to the game folder:",
                _ => this.ResolveNewName(_, oldRel, out _, out _) is null,
                "The new name must be a path inside the game folder that is not already used, for example GLOBAL\\GLOBALB_NEW.LZC.",
                oldRel))
            {
                input.Text = "Rename File";
                if (input.ShowDialog(this) != DialogResult.OK || String.IsNullOrEmpty(input.Value)) return;
                this.ResolveNewName(input.Value, oldRel, out var newRel, out var newFull);
                this.RenameGameFile(sdb, oldRel, oldFull, newRel, newFull);
            }
        }

        private void RenameGameFile(SynchronizedDatabase sdb, string oldRel, string oldFull, string newRel, string newFull)
        {
            var text = $"How do you want to rename {oldRel} to {newRel}?" + Environment.NewLine + Environment.NewLine +
                "Copy: Keeps the original file and copies it with the given name." + Environment.NewLine +
                "Move: Renames the original file." + Environment.NewLine + Environment.NewLine +
                "Warning: Moving a game file may break the game, if the game or other mods still expect it under its original name.";

            using var how = new Choice("Rename File", text, new[] { "Copy", "Move", "Cancel" }, 0, 2);
            how.ShowDialog(this);

            try
            {
                if (how.Value == 0) this.CopyGameFile(sdb, oldRel, oldFull, newRel, newFull);
                else if (how.Value == 1) this.MoveGameFile(sdb, oldRel, oldFull, newRel, newFull);
            }
            catch (Exception ex)
            {
                this.ShowMessage("Rename File", $"The operation failed: {ex.GetLowestMessage()}");
            }
        }

        private void CopyGameFile(SynchronizedDatabase sdb, string oldRel, string oldFull, string newRel, string newFull)
        {
            var text = $"{newRel} will be created as a copy of {oldRel}." + Environment.NewLine +
                "What do you want to do with the copy?" + Environment.NewLine + Environment.NewLine +
                "Open the copy too: Both files stay open." + Environment.NewLine +
                "Switch to the copy: Closes the original one and opens the copy instead." + Environment.NewLine +
                "Copy on disk only: The original one will stay open.";

            using var what = new Choice("Copy File", text,
                new[] { "Open the copy too", "Switch to the copy", "Copy on disk only", "Cancel" }, 0, 3);
            what.ShowDialog(this);
            if (what.Value < 0 || what.Value == 3) return;

            SaveRedirect.CreateDirectory(Path.GetDirectoryName(newFull));
            if (SaveRedirect.FileExists(newFull)) throw new IOException($"{newRel} already exists");
            SafeSave.Copy(oldFull, newFull);

            var commands = new List<string>() { $"move_file override absolute absolute {Quote(oldRel)} {Quote(newRel)}" };
            var select = oldRel;

            if (what.Value == 0 || what.Value == 1)
            {
                if (!this.TryLoadGameFile(newRel, out var copy, out var error))
                {
                    this.WriteLineToEndCommandPrompt(commands);
                    this.ShowMessage("Copy File", $"The copy was created, but it could not be opened. {error}");
                    return;
                }

                if (what.Value == 1)
                {
                    this.Profile.Remove(sdb);
                    commands.Add($"delete {Quote(oldRel)}");
                    this.EditLauncher(files => ReplaceInList(files, oldRel, newRel));
                }

                this.Profile.Add(copy);
                commands.Add($"new negate {Quote(copy.Filename)}");
                select = copy.Filename;
            }

            this.LoadTreeView(select);
            this.WriteLineToEndCommandPrompt(commands);
            this.EditorStatusLabel.Text = $"Copied {oldRel} to {newRel}";

            if (what.Value == 0) this.AskAddToLauncher(new[] { newRel.ToUpperInvariant() });
        }

        private void MoveGameFile(SynchronizedDatabase sdb, string oldRel, string oldFull, string newRel, string newFull)
        {
            SaveRedirect.CreateDirectory(Path.GetDirectoryName(newFull));
            SaveRedirect.MoveFile(oldFull, newFull);

            // Already saved before renaming, so the old database can be dropped without saving
            this.Profile.Remove(sdb);
            this.EditLauncher(files => ReplaceInList(files, oldRel, newRel));

            var commands = new List<string>()
            {
                $"delete {Quote(oldRel)}",
                $"move_file override absolute absolute {Quote(oldRel)} {Quote(newRel)}",
                $"erase_file absolute {Quote(oldRel)}",
            };

            var select = (string)null;

            if (this.TryLoadGameFile(newRel, out var moved, out var error))
            {
                this.Profile.Add(moved);
                commands.Add($"new negate {Quote(moved.Filename)}");
                select = moved.Filename;
            }
            else
            {
                this.ShowMessage("Move File", $"The file was moved, but it could not be opened again. {error}");
            }

            this.LoadTreeView(select);
            this.WriteLineToEndCommandPrompt(commands);
            this.EditorStatusLabel.Text = $"Moved {oldRel} to {newRel}";
        }

        /// <summary>
        /// Validates a new file name; returns an error message, or null when valid.
        /// </summary>
        private string ResolveNewName(string input, string oldRel, out string newRel, out string newFull)
        {
            newRel = null;
            newFull = null;
            var value = input?.Trim().Trim('"') ?? String.Empty;

            if (value.Length == 0) return "The name cannot be empty.";
            if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0) return "The name contains invalid characters.";
            if (Path.GetFileName(value).IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return "The name contains invalid characters.";
            if (value.EndsWith('\\') || value.EndsWith('/')) return "Enter a file name, not a folder.";

            try
            {
                var full = SaveRedirect.ToOriginal(Path.GetFullPath(Path.IsPathRooted(value) ? value : Path.Combine(this.Profile.Directory, value)));
                var rel = this.GetRelativeGamePath(full);

                if (rel is null) return "The new file must be inside the game folder.";
                if (String.Equals(rel, oldRel, StringComparison.OrdinalIgnoreCase)) return "The new name is the same as the current one.";
                if (SaveRedirect.FileExists(full) || SaveRedirect.DirectoryExists(full)) return "A file or folder with this name already exists.";
                if (this.Profile.Find(rel) != null) return "A file with this name is already open.";

                newRel = rel;
                newFull = full;
                return null;
            }
            catch (Exception ex)
            {
                return ex.GetLowestMessage();
            }
        }

        #endregion

        #region Close / Delete

        private void FileCloseItem_Click(object sender, EventArgs e)
        {
            var sdb = this.GetSelectedFile();
            if (sdb is null) return;

            var inLauncher = this.LauncherContains(sdb.Filename);
            var text = $"Close {sdb.Filename}?" + Environment.NewLine + Environment.NewLine +
                "Save and close: Saves the file first and then unloads it." + Environment.NewLine +
                "Close without saving: Discards the changes made to it since it was last saved.";

            using var prompt = new Choice("Close File", text, new[] { "Save and close", "Close without saving", "Cancel" }, 0, 2,
                inLauncher ? "Also remove it from the launcher" : null, true);
            prompt.ShowDialog(this);
            if (prompt.Value != 0 && prompt.Value != 1) return;

            var save = prompt.Value == 0;
            string warning = null;

            try
            {
                if (save) warning = this.Profile.Delete(sdb.Filename); // saves, then removes from the profile
                else this.Profile.Discard(sdb.Filename);
            }
            catch (Exception ex)
            {
                this.ShowMessage("Close File", $"The file could not be saved and was not closed: {ex.GetLowestMessage()}");
                return;
            }

            if (inLauncher && prompt.Checked) this.EditLauncher(files => RemoveFromList(files, sdb.Filename));

            this.LoadTreeView();
            this.WriteLineToEndCommandPrompt($"{(save ? "delete" : "discard")} {Quote(sdb.Filename)}");
            this.EditorStatusLabel.Text = save ? $"Closed {sdb.Filename}" : $"Closed {sdb.Filename} without saving";

            if (warning != null) this.ShowMessage("Warning", $"The file was saved and closed, with a warning: {warning}");
        }

        private void FileDeleteItem_Click(object sender, EventArgs e)
        {
            var sdb = this.GetSelectedFile();
            if (sdb is null) return;

            var text = (SaveRedirect.Active
                ? $"Delete {sdb.Filename}? Binarius cannot write to the game folder, so it removes its copy on the Desktop and lists the original as one to delete."
                : $"Permanently delete {sdb.Filename} from the game folder?") + Environment.NewLine + Environment.NewLine +
                "Warning: this cannot be undone, and deleting a game file may break the game. " +
                "Unsaved changes to this file are lost, and it is removed from the launcher.";

            using var prompt = new Choice("Delete File", text, new[] { "Delete from disk", "Cancel" }, 1, 1);
            prompt.ShowDialog(this);
            if (prompt.Value != 0) return;

            try
            {
                if (SaveRedirect.FileExists(sdb.FullPath))
                {
                    if (!SaveRedirect.Active) File.SetAttributes(sdb.FullPath, FileAttributes.Normal);
                    SaveRedirect.DeleteFile(sdb.FullPath);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("Delete File", $"The file could not be deleted: {ex.GetLowestMessage()}");
                return;
            }

            this.Profile.Remove(sdb);
            this.EditLauncher(files => RemoveFromList(files, sdb.Filename));

            this.LoadTreeView();
            this.WriteLineToEndCommandPrompt(new[] { $"delete {Quote(sdb.Filename)}", $"erase_file absolute {Quote(sdb.Filename)}" });
            this.EditorStatusLabel.Text = $"Deleted {sdb.Filename}";
        }

        #endregion

        #region Helpers

        private SynchronizedDatabase GetSelectedFile()
        {
            var node = this.EditorTreeView.SelectedNode;
            if (this.Profile is null || node is null || node.Parent != null) return null;
            return this.Profile.Find(node.Text);
        }

        private bool EnsureSavedBefore(string action)
        {
            if (!this._edited) return true;

            using var prompt = new Choice("Unsaved Changes",
                $"You have unsaved changes. Binarius needs to save all files before it can {action}.",
                new[] { "Save and continue", "Cancel" }, 0, 1);
            prompt.ShowDialog(this);
            if (prompt.Value != 0) return false;

            this.SaveProfile();
            return !this._edited;
        }

        /// <summary>
        /// Returns the path relative to the game directory, or null if the path is outside of it.
        /// </summary>
        private string GetRelativeGamePath(string path)
        {
            try
            {
                var root = Path.GetFullPath(this.Profile.Directory).TrimEnd('\\', '/') + "\\";
                var full = Path.GetFullPath(SaveRedirect.ToOriginal(path));
                if (!full.StartsWith(root, StringComparison.OrdinalIgnoreCase) || full.Length == root.Length) return null;
                return full[root.Length..];
            }
            catch
            {
                return null;
            }
        }

        private static string NormalizeEntry(string path) => path.Replace('/', '\\').Trim().TrimStart('\\');

        private bool LauncherContains(string rel)
        {
            try
            {
                Launch.Deserialize(Configurations.Default.LaunchFile, out var launch);
                return launch.Files.Any(_ => String.Equals(NormalizeEntry(_), NormalizeEntry(rel), StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads the current launcher, lets <paramref name="edit"/> change its file list, and saves
        /// it back when <paramref name="edit"/> returns true.
        /// </summary>
        private void EditLauncher(Func<List<string>, bool> edit)
        {
            var path = Configurations.Default.LaunchFile;

            try
            {
                Launch.Deserialize(path, out var launch);
                if (edit(launch.Files)) Launch.Serialize(path, launch);
            }
            catch (Exception ex)
            {
                this.ShowMessage("Launcher", $"The launcher {path} could not be updated: {ex.GetLowestMessage()}");
            }
        }

        private static bool ReplaceInList(List<string> files, string oldRel, string newRel)
        {
            var index = files.FindIndex(_ => String.Equals(NormalizeEntry(_), NormalizeEntry(oldRel), StringComparison.OrdinalIgnoreCase));
            if (index < 0) return false;
            files[index] = newRel;
            return true;
        }

        private static bool RemoveFromList(List<string> files, string rel) =>
            files.RemoveAll(_ => String.Equals(NormalizeEntry(_), NormalizeEntry(rel), StringComparison.OrdinalIgnoreCase)) > 0;

        private static string Quote(string value) => EndArguments.Quote(value, false) ?? value;

        private void ShowMessage(string title, string text)
        {
            using var info = new Info(text, title);
            info.ShowDialog(this);
        }

        #endregion
    }
}
