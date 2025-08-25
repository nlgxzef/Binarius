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
        }

        public class StatStripRenderer : ToolStripProfessionalRenderer
        {
            public StatStripRenderer() : base(new StatStripColors()) { }
        }

        public class MenuStripRenderer : ToolStripProfessionalRenderer
        {
            public MenuStripRenderer() : base(new MenuStripColors()) { }
        }

        private class StatStripColors : ProfessionalColorTable
        {
            //public override Color StatusStripGradientBegin => this.StatusStripGradientBegin;

            //public override Color StatusStripGradientEnd => this.StatusStripGradientEnd;
        }

        private class MenuStripColors : ProfessionalColorTable
        {
            //public override Color MenuStripGradientBegin => this.MenuStripGradientBegin;
            //public override Color MenuStripGradientEnd => this.MenuStripGradientEnd;
            //public override Color MenuBorder => this.MenuBorder;
            //
            //public override Color MenuItemBorder => this.MenuItemBorder;
            //
            //public override Color MenuItemPressedGradientBegin => this.MenuItemPressedGradientBegin;
            //
            //public override Color MenuItemPressedGradientMiddle => this.MenuItemPressedGradientMiddle;
            //
            //public override Color MenuItemPressedGradientEnd => this.MenuItemPressedGradientEnd;
            //
            //public override Color MenuItemSelected => this.MenuItemSelected;
            //
            //public override Color MenuItemSelectedGradientBegin => this.MenuItemSelectedGradientBegin;
            //
            //public override Color MenuItemSelectedGradientEnd => this.MenuItemSelectedGradientEnd;
        }

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
        }

        public static void Serialize(string filename, Theme theme)
        {
            if (filename == "System") return;

            AddColorJsonConverter();
            var settings = JsonSerializer.Serialize(theme, options);

            using var sw = new StreamWriter(File.Open(filename, FileMode.Create));
            sw.Write(settings);
            sw.WriteLine();
        }

        public static void Deserialize(string filename, out Theme theme)
        {
            if (filename == "System" || !File.Exists(filename))
            {
                theme = new Theme("System", "N/A", "1.0", false);
                return;
            }

            var settings = File.ReadAllText(filename);
            AddColorJsonConverter();
            theme = JsonSerializer.Deserialize<Theme>(settings, options);
        }
    }
}
