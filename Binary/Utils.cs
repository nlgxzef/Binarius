using CoreExtensions.IO;
using CoreExtensions.Management;
using CoreExtensions.Text;

using Endscript.Core;
using Endscript.Enums;
using Endscript.Helpers;
using Endscript.Profiles;

using Nikki.Core;
using Nikki.Reflection.Abstract;
using Nikki.Reflection.Interface;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Binary.Properties;

namespace Binary
{
    internal static class Utils
    {
        public static IReflective GetReflective(string path, string separator, BaseProfile profile)
        {
            string[] splits = path.Split(separator);

            // splits[0] = Filename
            // splits[1] = IManager
            // splits[2] = Collectable
            // splits[3] = Expandable
            // splits[4] = SubPart

            if (splits.Length is not 3 and not 5)
            {
                return null;
            }

            var db = profile.Find(splits[0]);
            if (db == null)
            {
                return null;
            }

            var manager = db.Database.GetManager(splits[1]);
            if (manager == null)
            {
                return null;
            }

            var collection = manager[manager.IndexOf(splits[2])] as Collectable;

            return splits.Length == 3
                ? collection
                : (IReflective)collection.GetSubPart(splits[4], splits[3]);
        }

        /// <summary>
        /// Opens URL link in default browser.
        /// Credit to: https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
        /// </summary>
        /// <param name="url">URL link to open.</param>
        public static void OpenBrowser(string url)
        {
            try
            {

                _ = Process.Start(url);

            }
            catch
            {

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {

                    url = url.Replace("&", "^&");
                    _ = Process.Start(new ProcessStartInfo("cmd.exe ", $"/c start {url}") { CreateNoWindow = true });

                }
                else
                {
                    _ = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                        ? Process.Start("xdg-open", url)
                        : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? Process.Start("open", url) : throw new PlatformNotSupportedException();

                }

            }
        }

        public static bool IsNullOrComment(this string value) => String.IsNullOrWhiteSpace(value) || value.StartsWith("//") || value.StartsWith("#");

        public static void SendLog()
        {
            try
            {

                // soon

            }
            catch (Exception e)
            {

                Console.WriteLine(e.GetLowestMessage());

            }
        }

        public static Launch GenerateSample() =>
            GenerateSample(String.Empty, GameINT.None.ToString(), eUsage.Invalid.ToString());

        public static Launch GenerateSample(string directory) =>
            GenerateSample(directory, GameINT.None.ToString(), eUsage.Invalid.ToString());

        public static Launch GenerateSample(string directory, string game, string usage)
        {
            var launch = new Launch()
            {

                Game = game,
                Usage = usage,
                Directory = directory,

                Files = new List<string>()
                {
                    @"GLOBAL\GLOBALA.BUN",
                    @"GLOBAL\GLOBALB.LZC",
                },

                Links = new List<SubLoader>()
                {

                    new()
                    {
                        File = @"GLOBAL\attributes.bin",
                        LoadType = eLoaderType.Attributes.ToString(),
                        PathType = ePathType.Absolute.ToString(),
                    },

                    new()
                    {
                        File = @"GLOBAL\fe_attrib.bin",
                        LoadType = eLoaderType.FeAttrib.ToString(),
                        PathType = ePathType.Absolute.ToString(),
                    },

                    new()
                    {
                        File = @"LANGUAGES\Labels_Global.bin",
                        LoadType = eLoaderType.Labels.ToString(),
                        PathType = ePathType.Absolute.ToString(),
                    },

                    new()
                    {
                        File = @"LANGUAGES\Labels.bin",
                        LoadType = eLoaderType.Labels.ToString(),
                        PathType = ePathType.Absolute.ToString(),
                    },

                },

            };

            return launch;
        }

        public static TreeNode GetTreeNodesFromSDB(SynchronizedDatabase sdb)
        {
            var result = new TreeNode(sdb.Filename);

            foreach (var manager in sdb.Database.Managers)
            {
                var managenode = new TreeNode(manager.Name);

                foreach (Collectable collection in manager)
                {

                    _ = managenode.Nodes.Add(GetCollectionNodes(collection));

                }

                _ = result.Nodes.Add(managenode);

            }

            return result;
        }

        public static TreeNode GetCollectionNodes(Collectable collection)
        {
            var result = new TreeNode(collection.CollectionName);

            foreach (var expando in collection.GetAllNodes())
            {

                var expandnode = new TreeNode(expando.NodeName);

                foreach (var subpart in expando.SubNodes)
                {

                    var subnode = new TreeNode(subpart.NodeName);
                    _ = expandnode.Nodes.Add(subnode);

                }

                _ = result.Nodes.Add(expandnode);

            }

            return result;
        }

        public static void toggleEmptyManagersInSDB(SynchronizedDatabase sdb, TreeNode sdbNode)
        {
            foreach (var manager in sdb.Database.Managers)
            {
                if (manager.Count == 0)
                {
                    bool found = false;
                    foreach (TreeNode node in sdbNode.Nodes)
                    {
                        if (node != null && node.GetType() == typeof(TreeNode) && node.FullPath.EndsWith(manager.Name))
                        {
                            if (Configurations.Default.HideEmptyManagers) sdbNode.Nodes.Remove(node);
                            found = true;
                            break;
                        }

                    }

                    if (!found && !Configurations.Default.HideEmptyManagers)
                    {
                        var managenode = new TreeNode(manager.Name);

                        foreach (Collectable collection in manager)
                        {

                            managenode.Nodes.Add(GetCollectionNodes(collection));

                        }

                        sdbNode.Nodes.Insert(sdb.Database.Managers.IndexOf(manager), managenode);

                    }
                }
            }
        }

        public static void MoveUp(TreeNode node)
        {
            TreeNode parent = node.Parent;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index > 0)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index - 1, node);

                    node.TreeView.SelectedNode = node;
                }
            }
        }

        public static void MoveDown(TreeNode node)
        {
            TreeNode parent = node.Parent;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index < parent.Nodes.Count - 1)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index + 1, node);

                    node.TreeView.SelectedNode = node;
                }
            }
        }

        public static string UTF8toISO(string convert)
        {
            byte[] bytes = convert.GetBytes();
            return Encoding.GetEncoding("ISO-8859-1").GetString(bytes);
        }

        public static string ISOtoUTF8(string convert)
        {
            byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(convert);
            return bytes.GetString();
        }

        public static void WriteErrorsToLog(IEnumerable<EndError> errors, string filename)
        {
            using var logger = new Logger("EndError.log", $"Endscript : {filename}", true);

            foreach (var error in errors)
            {

                logger.WriteLine($"File: {error.Filename}, Line: {error.Index}");
                logger.WriteLine($"Command: [{error.Line}]");
                logger.WriteLine($"Error: {error.Error}");
                logger.WriteLine(String.Empty);

            }

        }

        public static string GetTruncatedPath(string path)
        {
            string[] splits = path.Split(new char[] { '/', '\\' });

            return splits.Length > 3 ? Path.Combine("..", splits[^3], splits[^2], splits[^1]) : path;
        }

        public static string GetStatusString(int loadedFiles, long millisecondsToLoad, string path, string addon) => $"Files: {loadedFiles} | {addon} Time: {millisecondsToLoad}ms | Real Time: {DateTime.Now:HH:mm:ss} | Script: {GetTruncatedPath(path)}";

        public static bool PathHasIllegalCharacters(string path)
        {
            try
            {
                _ = Path.Combine(path, String.Empty); // amusement hack
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
