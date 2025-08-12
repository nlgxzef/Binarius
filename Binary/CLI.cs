﻿using Binary.Prompt;

using CoreExtensions.Management;

using Endscript.Commands;
using Endscript.Core;
using Endscript.Enums;
using Endscript.Profiles;

using Nikki.Core;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Binary
{
    public class CLI
    {
        BaseProfile m_profile;

        public CLI()
        {
        }

        void PrintExceptions(string[] exceptions)
        {
            string print = "";

            foreach (string exception in exceptions)
            {

                print += $"Exception: {exception}\n";

            }

            Console.WriteLine(print);
        }

        public void LoadProfile(string path)
        {
            Launch.Deserialize(path, out var launch);
            launch.ThisDir = Path.GetDirectoryName(path);

            Editor.FixLaunchDirectory(launch, path);

            if (launch.UsageID != eUsage.Modder)
            {

                throw new Exception($"Usage type of the endscript is stated to be {launch.Usage}, while should be Modder");

            }

            if (launch.GameID == GameINT.None)
            {

                throw new Exception($"Invalid stated game type named {launch.Game}");

            }

            if (!Directory.Exists(launch.Directory))
            {

                throw new DirectoryNotFoundException($"Directory named {launch.Directory} does not exist");

            }

            this.m_profile = BaseProfile.NewProfile(launch.GameID, launch.Directory);

            var watch = new Stopwatch();
            watch.Start();

            string[] exceptions = this.m_profile.Load(launch);

            watch.Stop();

            this.PrintExceptions(exceptions);
            Console.WriteLine($"Completed in {watch.Elapsed.TotalSeconds} seconds");
        }

        public void ImportEndscript(string path)
        {
            var parser = new EndScriptParser(path);

            BaseCommand[] commands;

            try
            {

                commands = parser.Read();

            }
            catch (Exception ex)
            {

                string error = $"Error has occured -> File: {parser.CurrentFile}, Line: {parser.CurrentIndex}" +
                    Environment.NewLine + $"Command: [{parser.CurrentLine}]" + Environment.NewLine +
                    $"Error: {ex.GetLowestMessage()}";

                Console.WriteLine(error);
                return;

            }

            var manager = new EndScriptManager(this.m_profile, commands, path);

            try
            {

                manager.CommandChase();

                while (!manager.ProcessScript())
                {

                    var command = manager.CurrentCommand;

                    if (command is InfoboxCommand infobox)
                    {

                        Console.WriteLine(infobox.Description);

                    }
                    else if (command is CheckboxCommand checkbox)
                    {

                        Console.WriteLine(checkbox.Description);
                        Console.WriteLine("Select one [yes, no]: ");
                        string result = Console.ReadLine();

                        checkbox.Choice = GetCheckboxOptionChosen(result);

                    }
                    else if (command is ComboboxCommand combobox)
                    {

                        Console.WriteLine(combobox.Description);
                        Console.WriteLine($"Select one [{GetInlinedOptions(combobox)}]: ");
                        string result = Console.ReadLine();

                        combobox.Choice = GetComboboxOptionChosen(combobox, result);

                    }

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.GetLowestMessage());
                return;

            }

            string script = Path.GetFileName(path);

            if (manager.Errors.Any())
            {

                Utils.WriteErrorsToLog(manager.Errors, path);
                Console.WriteLine($"Script {script} has been applied, however, {manager.Errors.Count()} errors " +
                    $"have been detected. Check EndError.log for more information.");

            }
            else
            {

                Console.WriteLine($"Script {script} has been successfully applied.");

            }

            string GetInlinedOptions(ComboboxCommand command)
            {
                string result = String.Empty;

                for (int i = 0; i < command.Options.Length - 1; ++i)
                {

                    result += command.Options[i].Name + ", ";

                }

                return result + command.Options[^1].Name;
            }

            int GetCheckboxOptionChosen(string strOption)
            {
                return String.Compare(strOption, "YES", StringComparison.OrdinalIgnoreCase) == 0
                    ? 1
                    : String.Compare(strOption, "NO", StringComparison.OrdinalIgnoreCase) == 0
                    ? 0
                    : throw new Exception("Argument passed is invalid, terminating execution...");
            }

            int GetComboboxOptionChosen(ComboboxCommand command, string strOption)
            {
                for (int i = 0; i < command.Options.Length; ++i)
                {

                    if (String.Compare(strOption, command.Options[i].Name, StringComparison.OrdinalIgnoreCase) == 0)
                    {

                        return i;

                    }

                }

                throw new Exception("Argument passed is invalid, terminating execution...");
            }
        }

        public void Save()
        {
            Console.WriteLine("Saving... Please wait...");

            var watch = new Stopwatch();
            watch.Start();

            string[] exceptions = this.m_profile.Save();

            watch.Stop();

            this.PrintExceptions(exceptions);
            Console.WriteLine($"Complete in {watch.Elapsed.TotalSeconds} seconds.");
        }
    }
}
