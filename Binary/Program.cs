using Binary.Properties;

using CoreExtensions.IO;
using CoreExtensions.Management;
using CoreExtensions.Native;

using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Properties;

namespace Binary
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool debugMode = false;

#if DEBUG
            debugMode = true;
#endif

            // Upgrade settings if necessary
            if (Configurations.Default.UpgradeRequired)
            {
                Configurations.Default.Upgrade();
                Configurations.Default.UpgradeRequired = false;
                Configurations.Default.Save();
            }

            Utils.ApplySaveSettings(); // before anything can save, CLI mode included

            // Context menus draw disabled items from the theme's colors too
            ToolStripManager.Renderer = new ThemedToolStripRenderer();

            // These reopen a launcher after restarting as administrator, everything else is the command line mode
            string editLauncher = args.Length == 2 && args[0] == SaveLocation.EditorArgument ? args[1] : null;
            string installLauncher = args.Length == 3 && args[0] == SaveLocation.InstallArgument ? args[1] : null;
            string installDirectory = installLauncher is null ? null : args[2];
            bool cliMode = args.Length > 0 && editLauncher is null && installLauncher is null;

            if (debugMode || cliMode)
            {
                NativeCallerX.AllocConsole();
            }
            else if (!Debugger.IsAttached && !Configurations.Default.DisableAdminWarning) // Warn the user if not ran in console & admin mode
            {
                using var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);

                if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                {

                    MessageBox.Show("Binarius is currently running in User Mode. To prevent issues while working with games installed in restricted places, it's highly recommended to run Binarius as Administrator.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

            if (cliMode)
            {
                var usage = eUsage.Invalid;

                usage = args[0].ToLowerInvariant() switch
                {
                    "modder" => eUsage.Modder,
                    "user" => eUsage.User,
                    _ => throw new ArgumentException("Invalid argument: {args[0]} - \"user\" or \"modder\" expected."),
                };
                if (args.Length < 2)
                {
                    throw new ArgumentException("Expected argument missing: VERSN1 path missing");
                }

                if (args.Length < 3)
                {
                    throw new ArgumentException("Expected argument missing: VERSN2 path missing");
                }

                if (!File.Exists("MainLog.txt")) { using var str = File.Create("MainLog.txt"); }
                if (!File.Exists("EndError.log")) { using var str = File.Create("EndError.log"); }
                string pp = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                SetDependencyPaths(pp);

                var cli = new CLI();

                if (!cli.LoadProfile(args[1])) return;
                cli.ImportEndscript(args[2]);
                cli.Save();

                return;
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Endscript.Version.Value = version;
            SynchronizedDatabase.Watermark = "Binarius | Automated";

            if (!File.Exists("MainLog.txt")) { using var str = File.Create("MainLog.txt"); }
            if (!File.Exists("EndError.log")) { using var str = File.Create("EndError.log"); }
            string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            SetDependencyPaths(path);

            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionHandler);
            Binary.UI.ThemedBorders.Install();
            Application.Run(new IntroUI(editLauncher, installLauncher, installDirectory));

            if (debugMode)
            {
                NativeCallerX.FreeConsole();
            }
        }

        private static void SetDependencyPaths(string thispath)
        {
            string userdir = Path.Combine(thispath, "userkeys");
            string mainc = Path.Combine(thispath, @"mainkeys\carbon.txt");
            string userc = Path.Combine(thispath, @"userkeys\carbon.txt");
            string mainmw = Path.Combine(thispath, @"mainkeys\mostwanted.txt");
            string usermw = Path.Combine(thispath, @"userkeys\mostwanted.txt");
            string mainps = Path.Combine(thispath, @"mainkeys\prostreet.txt");
            string userps = Path.Combine(thispath, @"userkeys\prostreet.txt");
            string mainuc = Path.Combine(thispath, @"mainkeys\undercover.txt");
            string useruc = Path.Combine(thispath, @"userkeys\undercover.txt");
            string mainug1 = Path.Combine(thispath, @"mainkeys\underground1.txt");
            string userug1 = Path.Combine(thispath, @"userkeys\underground1.txt");
            string mainug2 = Path.Combine(thispath, @"mainkeys\underground2.txt");
            string userug2 = Path.Combine(thispath, @"userkeys\underground2.txt");
            string usercustattr = Path.Combine(thispath, @"userkeys\CustomAttributes.txt");

            // Optional vault (attrib) key lists, loaded only if present
            MostWantedProfile.AttribHashList = Path.Combine(thispath, @"mainkeys\mostwanted_attrib.txt");
            CarbonProfile.AttribHashList = Path.Combine(thispath, @"mainkeys\carbon_attrib.txt");
            ProstreetProfile.AttribHashList = Path.Combine(thispath, @"mainkeys\prostreet_attrib.txt");
            UndercoverProfile.AttribHashList = Path.Combine(thispath, @"mainkeys\undercover_attrib.txt");

            CarbonProfile.MainHashList = mainc;
            CarbonProfile.CustomHashList = userc;
            MostWantedProfile.MainHashList = mainmw;
            MostWantedProfile.CustomHashList = usermw;
            ProstreetProfile.MainHashList = mainps;
            ProstreetProfile.CustomHashList = userps;
            UndercoverProfile.MainHashList = mainuc;
            UndercoverProfile.CustomHashList = useruc;
            Underground1Profile.MainHashList = mainug1;
            Underground1Profile.CustomHashList = userug1;
            Underground2Profile.MainHashList = mainug2;
            Underground2Profile.CustomHashList = userug2;
            Nikki.Core.Map.CustomAttribFile = usercustattr;

            if (!Directory.Exists(userdir))
            {
                _ = Directory.CreateDirectory(userdir);
            }

            if (!File.Exists(userc)) { File.Create(userc).Dispose(); }
            if (!File.Exists(usermw)) { File.Create(usermw).Dispose(); }
            if (!File.Exists(userps)) { File.Create(userps).Dispose(); }
            if (!File.Exists(useruc)) { File.Create(useruc).Dispose(); }
            if (!File.Exists(userug1)) { File.Create(userug1).Dispose(); }
            if (!File.Exists(userug2)) { File.Create(userug2).Dispose(); }
            if (!File.Exists(usercustattr)) { File.Create(usercustattr).Dispose(); }
        }

        public static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            using var logger = new Logger("MainLog.txt", "Binarius : Unknown Exception", true);
            logger.WriteException(e.Exception);

#if DEBUG
            MessageBox.Show("Unexpected error has occured. Please send MainLog.txt " +
                "file to us. Right now Binarius will export all your " +
                "collections to an autogenerated folder so you won't lose your data.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

            try
            {

                var s = new StackTrace(e.Exception);
                var thisasm = Assembly.GetExecutingAssembly();
                var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm);

                if (methodname.DeclaringType == typeof(Editor))
                {

                    var form = Application.OpenForms.Cast<Form>().First(_ => _.GetType() == typeof(Editor));

                    if (form is Editor editor)
                    {

                        //editor.EmergencySaveDatabase();
                        //MessageBox.Show("Database backup up.", "Done");

                    }

                }

            }
            catch { }

#else

            MessageBox.Show($"Unexpected error has occured: {e.Exception.GetLowestMessage()}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

#endif
        }
    }
}
