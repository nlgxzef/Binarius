using Binary.Prompt;

using CoreExtensions.Management;

using Endscript.Core;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace Binary
{
    /// <summary>
    /// Checks if the game folder of a launcher can be written to when it gets loaded. If not, asks to run Binarius
    /// as administrator or to save to a folder on the Desktop instead.
    /// </summary>
    internal static class SaveLocation
    {
        /// <summary>
        /// Argument that opens a launcher in the editor: --editor [launcher]
        /// </summary>
        public const string EditorArgument = "--editor";

        /// <summary>
        /// Argument that installs a launcher: --install [launcher] [game folder]
        /// </summary>
        public const string InstallArgument = "--install";

        private const int ElevationDeclined = 1223; // ERROR_CANCELLED

        public enum Result
        {
            Continue,
            Cancel,
            Restarted,
        }

        // These three are the only things here that reach outside of Binarius
        internal static Func<string> GetDesktop { get; set; } =
            () => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        internal static Func<ProcessStartInfo, Process> StartProcess { get; set; } = Process.Start;

        internal static Func<bool> IsElevated { get; set; } = () =>
        {
            using var identity = WindowsIdentity.GetCurrent();
            return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        };

        /// <summary>
        /// Decides where a launcher saves to. Changes nothing if cancelled.
        /// </summary>
        /// <param name="owner">Window that owns the question.</param>
        /// <param name="launch">Launcher, with its game folder set.</param>
        /// <param name="launcher">Path of the launcher.</param>
        /// <param name="restartArguments">Arguments that bring Binarius back to this launcher after a restart.</param>
        /// <param name="confirmRestart">Returns false to not restart, like when there are unsaved changes.</param>
        public static Result Resolve(IWin32Window owner, Launch launch, string launcher, string[] restartArguments,
            Func<bool> confirmRestart = null)
        {
            if (SaveRedirect.IsFor(launch.Directory, launcher)) return Result.Continue; // already answered

            var denied = SaveRedirect.FindDenied(launch.Directory, launch.Files);

            if (denied is null)
            {
                SaveRedirect.Stop();
                return Result.Continue;
            }

            var output = SaveRedirect.FindFolder(GetDesktop(), launcher, launch.Directory, out var reused);
            var elevated = IsElevated();

            var text = GetReason(denied) + Environment.NewLine + Environment.NewLine +
                GetDesktopText(output, reused);

            if (!elevated)
            {
                text += Environment.NewLine + Environment.NewLine +
                    "Run as administrator: restarts Binarius with administrator rights and loads this launcher again.";
            }

            var buttons = elevated
                ? new[] { "Save to Desktop", "Cancel" }
                : new[] { "Run as administrator", "Save to Desktop", "Cancel" };

            while (true)
            {
                using var prompt = new Choice("No Write Permission", text, buttons, 0, buttons.Length - 1);
                prompt.ShowDialog(owner);

                if (prompt.Value < 0 || prompt.Value == buttons.Length - 1) return Result.Cancel;

                if (prompt.Value == buttons.Length - 2)
                {
                    SaveRedirect.Start(launch.Directory, output, launcher);
                    return Result.Continue;
                }

                if (confirmRestart != null && !confirmRestart()) continue;
                if (TryRestart(owner, restartArguments)) return Result.Restarted;
            }
        }

        /// <summary>
        /// The same question in the console.
        /// </summary>
        /// <returns>False if cancelled.</returns>
        public static bool ResolveInConsole(Launch launch, string launcher)
        {
            var denied = SaveRedirect.FindDenied(launch.Directory, launch.Files);

            if (denied is null)
            {
                SaveRedirect.Stop();
                return true;
            }

            var output = SaveRedirect.FindFolder(GetDesktop(), launcher, launch.Directory, out var reused);

            Console.WriteLine(GetReason(denied));
            Console.WriteLine(GetDesktopText(output, reused));
            Console.WriteLine("To save to the game folder, answer no and run Binarius as administrator.");
            Console.WriteLine("Save to Desktop? [yes, no]: ");

            var answer = Console.ReadLine()?.Trim();

            if (!String.Equals(answer, "yes", StringComparison.OrdinalIgnoreCase) &&
                !String.Equals(answer, "y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Cancelled, nothing was changed.");
                return false;
            }

            SaveRedirect.Start(launch.Directory, output, launcher);
            return true;
        }

        /// <summary>
        /// Tells where the files were saved and what to do with them.
        /// </summary>
        public static string GetSavedText()
        {
            return $"The files are saved to {SaveRedirect.Output}, the game folder was not changed." +
                Environment.NewLine + Environment.NewLine +
                $"To apply them, follow the steps in {SaveRedirect.InfoName} in that folder.";
        }

        private static string GetReason(string denied) =>
            $"Binarius does not have permission to write to {denied}, so it cannot save to the game folder.";

        private static string GetDesktopText(string output, bool reused)
        {
            var text = $"Save to Desktop: saves to {output} instead and leaves the game folder as it is.";

            if (reused) text += " That folder has files saved earlier for this game, which are used instead of the ones in the game folder.";

            return text;
        }

        private static bool TryRestart(IWin32Window owner, string[] arguments)
        {
            var exe = Environment.ProcessPath;

            var info = new ProcessStartInfo(exe)
            {
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = Path.GetDirectoryName(exe),
            };

            foreach (var argument in arguments) info.ArgumentList.Add(argument);

            try
            {
                using var process = StartProcess(info);
                return true;
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == ElevationDeclined)
            {
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner, $"Binarius could not restart as administrator: {ex.GetLowestMessage()}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
