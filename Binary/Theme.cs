using Binary.Properties;

using Endscript.Core;
using Endscript.Enums;
using Endscript.Exceptions;
using Endscript.Helpers;

using Nikki.Core;
using Nikki.Reflection.Abstract;

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Xml.Linq;

using static System.Resources.ResXFileRef;

namespace Binary
{
    public class Theme
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public bool DarkTheme { get; set; }
        public ColorSet Colors { get; set; } = new ColorSet();
        public class ColorSet
        {
            public Color MainBackColor { get; set; }
            public Color MainForeColor { get; set; }
            public Color ButtonBackColor { get; set; }
            public Color ButtonForeColor { get; set; }
            public Color ButtonFlatColor { get; set; }
            public Color TextBoxBackColor { get; set; }
            public Color TextBoxForeColor { get; set; }
            public Color PrimBackColor { get; set; }
            public Color PrimForeColor { get; set; }
            public Color MenuItemBackColor { get; set; }
            public Color MenuItemForeColor { get; set; }
            public Color LabelTextColor { get; set; }
            public Color RegBorderColor { get; set; }
            public Color FocusedBackColor { get; set; }
            public Color FocusedForeColor { get; set; }
            public Color StatusStripGradientBegin { get; set; }
            public Color StatusStripGradientEnd { get; set; }
            public Color MenuStripGradientBegin { get; set; }
            public Color MenuStripGradientEnd { get; set; }
            public Color MenuBorder { get; set; }
            public Color MenuItemBorder { get; set; }
            public Color MenuItemPressedGradientBegin { get; set; }
            public Color MenuItemPressedGradientMiddle { get; set; }
            public Color MenuItemPressedGradientEnd { get; set; }
            public Color MenuItemSelected { get; set; }
            public Color MenuItemSelectedGradientBegin { get; set; }
            public Color MenuItemSelectedGradientEnd { get; set; }

            // Highlight colors (were hardcoded per dark/light theme)
            public Color SearchHighlightBackColor { get; set; }
            public Color SelectedNodeForeColor { get; set; }
            public Color FirstSelectedNodeForeColor { get; set; }
            public Color ModifiedItemBackColor { get; set; }

            // Border of text boxes and combo boxes (empty = native border)
            public Color TextBoxBorderColor { get; set; }

            public ColorSet Clone() => (ColorSet)this.MemberwiseClone();
        }

        /// <summary>
        /// Theme used to fill in colors that a theme file does not define (the default ThemeFile setting).
        /// </summary>
        public const string DefaultThemeFile = "StackedDeck.json";

        private static readonly Dictionary<string, (DateTime Stamp, Theme Theme)> s_cache = new(StringComparer.OrdinalIgnoreCase);
        private static readonly object s_lock = new();

        private static readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            IgnoreReadOnlyProperties = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = true,
        };

        public class ColorJsonConverter : JsonConverter<Color>
        {
            private static ColorConverter converter = new();

            public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return (Color)converter.ConvertFromInvariantString(reader.GetString() ?? "")!;
            }
            public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(converter.ConvertToInvariantString(value));
            }
        }

        public static void AddColorJsonConverter()
        {
            foreach (JsonConverter i in options.Converters)
            {
                if (i is ColorJsonConverter) return;
            }

            options.Converters.Add(new ColorJsonConverter());
        }

        public static string GetThemeFile()
        {
            string filename = Configurations.Default.ThemeFile;

            if (Configurations.Default.ThemeFile == "Automatic")
                filename = GetAutomaticThemeFile((GameINT)Configurations.Default.CurrentGame);

            return Path.Combine("Themes", filename);
        }

        public static string GetAutomaticThemeFile(GameINT game)
        {
            string theme = game switch
            {
                GameINT.Carbon => "Carbon.json",
                GameINT.MostWanted => "MostWanted.json",
                GameINT.Prostreet => "Prostreet.json",
                GameINT.Undercover => "Undercover.json",
                GameINT.Underground1 => "Underground1.json",
                GameINT.Underground2 => "Underground2.json",
                _ => "System"
            };

            return theme;
        }

        public Theme() : this(string.Empty, string.Empty, string.Empty, false) { }

        public Theme(string name, string author, string version, bool isdark)
        {
            this.Name = name;
            this.Author = author;
            this.Version = version;
            this.DarkTheme = isdark;
            //this.Colors = new ColorSet();

            this.Colors.MainBackColor = SystemColors.Control;
            this.Colors.MainForeColor = SystemColors.ControlText;
            this.Colors.ButtonBackColor = SystemColors.Control;
            this.Colors.ButtonForeColor = SystemColors.ControlText;
            this.Colors.ButtonFlatColor = SystemColors.ButtonShadow;
            this.Colors.TextBoxBackColor = SystemColors.Control;
            this.Colors.TextBoxForeColor = SystemColors.ControlText;
            this.Colors.PrimBackColor = SystemColors.Control;
            this.Colors.PrimForeColor = SystemColors.ControlText;
            this.Colors.MenuItemBackColor = SystemColors.Control;
            this.Colors.MenuItemForeColor = SystemColors.ControlText;
            this.Colors.LabelTextColor = SystemColors.ControlText;
            this.Colors.RegBorderColor = SystemColors.Control;
            this.Colors.FocusedBackColor = SystemColors.Control;  
            this.Colors.FocusedForeColor = SystemColors.ControlText;
            this.Colors.StatusStripGradientBegin = SystemColors.Control;
            this.Colors.StatusStripGradientEnd = SystemColors.Control;
            this.Colors.MenuStripGradientBegin = SystemColors.Control;
            this.Colors.MenuStripGradientEnd = SystemColors.Control;
            this.Colors.MenuBorder = SystemColors.Control;
            this.Colors.MenuItemBorder = SystemColors.Control;
            this.Colors.MenuItemPressedGradientBegin = SystemColors.Control;
            this.Colors.MenuItemPressedGradientMiddle = SystemColors.Control;
            this.Colors.MenuItemPressedGradientEnd = SystemColors.Control;
            this.Colors.MenuItemSelected = SystemColors.Control;
            this.Colors.MenuItemSelectedGradientBegin = SystemColors.Control;
            this.Colors.MenuItemSelectedGradientEnd = SystemColors.Control;
            this.Colors.SearchHighlightBackColor = Color.FromArgb(60, 255, 60);
            this.Colors.SelectedNodeForeColor = Color.FromArgb(255, 20, 20);
            this.Colors.FirstSelectedNodeForeColor = Color.FromArgb(255, 90, 0);
            this.Colors.ModifiedItemBackColor = Color.FromArgb(60, 60, 255);
            this.Colors.TextBoxBorderColor = Color.Empty; // System theme: keep native borders
        }

        public static void Serialize(string filename, Theme theme)
        {
            if (filename == "System") return;

            AddColorJsonConverter();
            var settings = JsonSerializer.Serialize(theme, options);

            Endscript.Core.SafeSave.Write(filename, target =>
            {
                using var sw = new StreamWriter(File.Open(target, FileMode.Create));
                sw.Write(settings);
                sw.WriteLine();
            });
        }

        /// <summary>
        /// Loads a theme. Colors that are missing or invalid in the file are taken from the default theme
        /// (<see cref="DefaultThemeFile"/>); if the default theme lacks them too, system colors are used.
        /// A missing theme file loads the default theme; "System" loads system colors.
        /// </summary>
        public static void Deserialize(string filename, out Theme theme)
        {
            if (String.IsNullOrEmpty(filename) || Path.GetFileName(filename) == "System")
            {
                theme = new Theme("System", "N/A", "1.0", false);
                return;
            }

            var defaultPath = Path.Combine("Themes", DefaultThemeFile);
            var isDefault = String.Equals(Path.GetFullPath(filename), Path.GetFullPath(defaultPath), StringComparison.OrdinalIgnoreCase);
            var fallback = isDefault ? new Theme("System", "N/A", "1.0", false) : LoadCached(defaultPath, null);

            theme = LoadCached(filename, fallback) ?? Copy(fallback);
        }

        private static Theme LoadCached(string filename, Theme fallback)
        {
            if (!File.Exists(filename)) return fallback is null ? new Theme("System", "N/A", "1.0", false) : null;

            var key = Path.GetFullPath(filename);
            var stamp = File.GetLastWriteTimeUtc(key);

            lock (s_lock)
            {
                if (s_cache.TryGetValue(key, out var cached) && cached.Stamp == stamp && fallback is null) return Copy(cached.Theme);
            }

            var result = Parse(key, fallback ?? new Theme("System", "N/A", "1.0", false));

            if (fallback is null)
            {
                lock (s_lock) s_cache[key] = (stamp, Copy(result));
            }

            return result;
        }

        private static Theme Parse(string filename, Theme fallback)
        {
            var result = Copy(fallback);

            try
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(filename), new JsonDocumentOptions()
                {
                    AllowTrailingCommas = true,
                    CommentHandling = JsonCommentHandling.Skip,
                });

                var root = doc.RootElement;
                if (root.TryGetProperty("Name", out var name) && name.ValueKind == JsonValueKind.String) result.Name = name.GetString();
                if (root.TryGetProperty("Author", out var author) && author.ValueKind == JsonValueKind.String) result.Author = author.GetString();
                if (root.TryGetProperty("Version", out var version) && version.ValueKind == JsonValueKind.String) result.Version = version.GetString();
                if (root.TryGetProperty("DarkTheme", out var dark) && (dark.ValueKind == JsonValueKind.True || dark.ValueKind == JsonValueKind.False)) result.DarkTheme = dark.GetBoolean();

                if (root.TryGetProperty("Colors", out var colors) && colors.ValueKind == JsonValueKind.Object)
                {
                    var converter = new ColorConverter();

                    foreach (var property in typeof(ColorSet).GetProperties())
                    {
                        if (property.PropertyType != typeof(Color)) continue;

                        foreach (var entry in colors.EnumerateObject())
                        {
                            if (!String.Equals(entry.Name, property.Name, StringComparison.OrdinalIgnoreCase)) continue;
                            if (entry.Value.ValueKind != JsonValueKind.String) break;

                            try { property.SetValue(result.Colors, (Color)converter.ConvertFromInvariantString(entry.Value.GetString())); }
                            catch { } // invalid value: keep the fallback color

                            break;
                        }
                    }
                }
            }
            catch
            {
                return Copy(fallback); // unreadable file: use the fallback theme
            }

            return result;
        }

        private static Theme Copy(Theme theme) => new Theme()
        {
            Name = theme.Name,
            Author = theme.Author,
            Version = theme.Version,
            DarkTheme = theme.DarkTheme,
            Colors = theme.Colors.Clone(),
        };
    }
}
