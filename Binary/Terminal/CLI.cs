using Binary.Args;

using CoreExtensions.Management;

using Endscript.Commands;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;

using Nikki.Core;

using System;
using System.IO;
using System.Linq;

namespace Binary.Terminal
{
    public class CLI
    {
        public void RunCliMode()
        {
            Launch.Deserialize(_options["script"].GetValue<string>(), out Launch launch);
            if(launch.UsageID != _options["mode"].GetValue<eUsage>())
            {
                throw new Exception($"Usage tpe of the endscript is stated to be {launch.Usage}, while should be {_options["mode"].Value}");
            }

            if(launch.GameID == GameINT.None)
            {
                throw new Exception($"Invalid stated game type named {launch.Game}");
            }

            if (!Directory.Exists(_options["game"].GetValue<string>()))
            {
                throw new Exception($"Game directory \"{_options["game"].Value}\" not found");
            }

            launch.Directory = _options["game"].GetValue<string>();
            launch.ThisDir = Path.GetDirectoryName(_options["script"].GetValue<string>());
            launch.CheckEndscript();
            launch.CheckFiles();
            launch.LoadLinks();

            var es = Path.Combine(launch.ThisDir, launch.Endscript);
            var parser = new EndScriptParser(es);

            BaseCommand[] commands;
            try
            {
                commands = parser.Read();
            } catch(Exception ex)
            {
                var error = $"Error has occured -> File: {parser.CurrentFile}, Line: {parser.CurrentIndex}" +
                    Environment.NewLine + $"Command: [{parser.CurrentLine}]" + Environment.NewLine +
                    $"Error: {ex.GetLowestMessage()}";

                throw new Exception(error);
            }

            var profile = BaseProfile.NewProfile(launch.GameID, launch.Directory);
            var exceptions = profile.Load(launch);

            if(exceptions.Length > 0)
            {
                foreach(var ex in exceptions)
                {
                    Console.Error.WriteLine(ex);
                }
                Environment.Exit(-1);
            }

            this.EnsureBackups(profile);
            var manager = new EndScriptManager(profile, commands, es);

            try
            {
                int optIdx = 0;
                manager.CommandChase();
                while(!manager.ProcessScript())
                {
                    var cmd = manager.CurrentCommand;
                    switch(cmd)
                    {
                        case InfoboxCommand info:
                            Console.Error.WriteLine($"[INFO] {info.Description}");
                            break;
                        case CheckboxCommand checkbox:
                            this.AskOption(checkbox, optIdx++);
                            break;
                        case ComboboxCommand combo:
                            this.AskOption(combo, optIdx++);
                            break;
                    }
                }
            } catch(Exception ex)
            {
                Console.Error.WriteLine(ex.GetLowestMessage());
                Environment.Exit(-1);
            }

            var script = Path.GetFileName(_options["script"].GetValue<string>());

            if (manager.Errors.Any())
            {
                Utils.WriteErrorsToLog(manager.Errors, _options["script"].GetValue<string>());
                Console.Error.WriteLine("Script {script} has been applied, however, {manager.Errors.Count()} errors " +
                    $"has been detected. Check EndError.log for more information");
                Environment.Exit(-1);
            } else
            {
                var errors = profile.Save();

                if (errors.Length > 0)
                {

                    foreach (var error in errors)
                        Console.Error.WriteLine(error);

                    Environment.Exit(-1);
                }
            }
        }

        private void AskOption(CheckboxCommand cmd, int idx)
        {
            if(!this.TryGetUserOption(idx, out var value))
            {
                var selection = new CheckBoxSelector(cmd).Process();
                cmd.Choice = selection;
                return;
            }

            switch(value.ToLowerInvariant())
            {
                case "1":
                case "y":
                case "yes":
                case "true":
                    cmd.Choice = 1;
                    return;
                case "0":
                case "n":
                case "no":
                case "false":
                    cmd.Choice = 0;
                    return;
                default:
                    Console.Error.WriteLine($"Invalid option \"{value}\"");
                    break;
            }
        }
        private void AskOption(ComboboxCommand cmd, int idx)
        {
            if(!this.TryGetUserOption(idx, out var value))
            {
                var selection = new ComboBoxSelector(cmd).Process();
                cmd.Choice = selection;
                return;
            }

            if(!int.TryParse(value, out var i32))
            {
                Console.Error.WriteLine($"Invalid option \"{value}\"");
                Environment.Exit(-1);
            }

            cmd.Choice = i32;
        }
        bool TryGetUserOption(int index, out string str)
        {
            if(!_options.TryGetOption("options", out var opt))
            {
                str = null;
                return false;
            }

            var args = opt.GetValue<string>().Split(',');
            if(index >= args.Length)
            {
                str = null;
                return false;
            }

            str = args[index];
            return true;
        }

        private void EnsureBackups(BaseProfile profile)
        {
            foreach (var sdb in profile)
            {

                var orig = sdb.FullPath;
                var back = $"{orig}.bacc";
                if (!File.Exists(back)) File.Copy(orig, back, true);

            }
        }

        public CLI(RuntimeOptions options)
        {
            _options = options;
        }

        BaseProfile m_profile;
        readonly RuntimeOptions _options;
    }
}
