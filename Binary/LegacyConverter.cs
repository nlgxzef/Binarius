using Binary.Prompt;

using CoreExtensions.Management;

using Endscript.Core;

using Nikki.Core;

using System;
using System.IO;
using System.Windows.Forms;

namespace Binary
{
    /// <summary>
    /// Convert Legacy Endscript: rewrites the CarTypeInfo commands of an endscript made with legacy Binary/Nikki for
    /// Underground 1 or 2. Used from the editor, for the game loaded, and from the intro window, which asks which game.
    /// </summary>
    internal static class LegacyConverter
    {
        private const string Title = "Convert Legacy Endscript";
        private const string ScriptFilter = "Binary/ius Endscript Files|*.end;*.endscript|All Files|*.*";

        /// <summary>
        /// True if the converter works for this game.
        /// </summary>
        public static bool Supports(GameINT game) => game == GameINT.Underground1 || game == GameINT.Underground2;

        /// <summary>
        /// Asks for an endscript, converts it and saves the result to a new file.
        /// </summary>
        /// <param name="owner">Window the dialogs belong to.</param>
        /// <param name="game">Game of the loaded launcher, or null to ask which game the endscript is for.</param>
        public static void Run(IWin32Window owner, GameINT? game)
        {
            using var open = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = ScriptFilter,
                Multiselect = false,
                Title = Title,
            };

            if (open.ShowDialog(owner) != DialogResult.OK) return;

            var intro = "This converter is built to convert endscripts made with legacy Binary/Nikki to work with Binarius which has the proper CarTypeInfo structure.";
            const string keep = "Keep mistakes from legacy Binary/Nikki (exact old result)";
            GameINT chosen;
            bool keepMistakes;

            if (game is null)
            {

                using var which = new Choice(Title,
                    intro + Environment.NewLine + Environment.NewLine +
                    $"Please choose for which game this endscript ({Path.GetFileName(open.FileName)}) is made.",
                    new[] { "Underground 1", "Underground 2", "Cancel" }, 1, 2, keep, false);
                which.ShowDialog(owner);
                if (which.Value < 0 || which.Value > 1) return;

                chosen = which.Value == 0 ? GameINT.Underground1 : GameINT.Underground2;
                keepMistakes = which.Checked;

            }
            else
            {

                // The game of the loaded launcher
                var name = game == GameINT.Underground1 ? "Underground 1" : "Underground 2";
                using var confirm = new Choice(Title,
                    intro + Environment.NewLine + Environment.NewLine +
                    $"{Path.GetFileName(open.FileName)} is converted for {name}, the game of the loaded launcher.",
                    new[] { "Convert", "Cancel" }, 0, 1, keep, false);
                confirm.ShowDialog(owner);
                if (confirm.Value != 0) return;

                chosen = game.Value;
                keepMistakes = confirm.Checked;

            }

            using var save = new SaveFileDialog()
            {
                Filter = open.FilterIndex == 1 ? ScriptFilter : "All Files|*.*",
                InitialDirectory = Path.GetDirectoryName(open.FileName),
                FileName = Path.GetFileNameWithoutExtension(open.FileName) + "_converted" + Path.GetExtension(open.FileName),
                OverwritePrompt = true,
                Title = "Save Converted Script",
            };

            if (save.ShowDialog(owner) != DialogResult.OK) return;

            if (String.Equals(Path.GetFullPath(save.FileName), Path.GetFullPath(open.FileName), StringComparison.OrdinalIgnoreCase))
            {
                _ = MessageBox.Show(owner, "Save the converted script under another name, so the original stays as it is.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                var result = Endscript.Converters.CarTypeInfoScriptConverter.ConvertFile(open.FileName, save.FileName, chosen, keepMistakes);
                var report = Path.ChangeExtension(save.FileName, null) + "_report.txt";
                SafeSave.Write(report, target => File.WriteAllText(target, result.Report()));

                var text = $"{result.Converted} lines converted, {result.Removed} removed, {result.NeedsReview} to check by hand." +
                    Environment.NewLine + Environment.NewLine + $"Report: {report}";

                if (result.AppendedFiles.Count > 0)
                {
                    text += Environment.NewLine + Environment.NewLine + "This script appends other files, which were not converted: " +
                        String.Join(", ", result.AppendedFiles);
                }

                _ = MessageBox.Show(owner, text, Title, MessageBoxButtons.OK,
                    result.NeedsReview > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {

                _ = MessageBox.Show(owner, ex.GetLowestMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
