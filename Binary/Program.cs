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
using System.Threading;
using System.Windows.Forms;

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

            if (debugMode || args.Length > 0)
            {
                NativeCallerX.AllocConsole();
            }

            if (args.Length > 0)
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

                cli.LoadProfile(args[1]);
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
            Application.Run(new IntroUI());

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

            if (!File.Exists(userc)) { _ = File.Create(userc); }
            if (!File.Exists(usermw)) { _ = File.Create(usermw); }
            if (!File.Exists(userps)) { _ = File.Create(userps); }
            if (!File.Exists(useruc)) { _ = File.Create(useruc); }
            if (!File.Exists(userug1)) { _ = File.Create(userug1); }
            if (!File.Exists(userug2)) { _ = File.Create(userug2); }
            if (!File.Exists(usercustattr)) { _ = File.Create(usercustattr); }
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
