using Binary.Properties;

using Nikki.Core;
using Nikki.Reflection.Enum;
using Nikki.Reflection.Enum.CP;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;



namespace Binary.Interact
{
	public partial class CustomAttribCreator : Form
	{
		private const string Boolean = "Boolean";
		private const string Color = "Color";
		private const string Floating = "Floating";
		private const string Integer = "Integer";
		private const string Key = "Key";
		private const string ModelTable = "ModelTable";
		private const string PartID = "PartID";
		private const string String = "String";
		private const string TwoString = "TwoString";

		public CarPartAttribType Type { get; private set; }
        public string Value { get; private set; }
        public List<string> CustomAttribKeys { get; private set; }

        public CustomAttribCreator(GameINT game)
		{
			this.InitializeComponent();
			this.ToggleTheme();
			this.PopulateAttribTypesBasedOnGame(game);
			this.LoadHashList();
		}

        public void LoadHashList()
        {
            if (File.Exists(Map.CustomAttribFile)) try
            {

                var lines = File.ReadAllLines(Map.CustomAttribFile);

                this.CustomAttribKeys = new List<string>(lines.Length);

                foreach (var line in lines)
                {

                    if (line.StartsWith("//") || line.StartsWith("#")) continue;
                    else this.CustomAttribKeys.Add(line);

                }

            }
            catch { }

            this.AttribKeyComboBox.Items.Clear();
            this.CustomAttribKeys.Sort();
            foreach (var label in this.CustomAttribKeys)
            {
                this.AttribKeyComboBox.Items.Add(label);
            }

        }

        public void SaveHashList()
        {
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Map.CustomAttribFile));

            if (File.Exists(Map.CustomAttribFile))
            {

                var lines = File.ReadAllLines(Map.CustomAttribFile);
                var set = new HashSet<string>(lines.Length + 1);

                foreach (var line in lines)
                {

                    if (line.StartsWith("//") || line.StartsWith("#")) continue;
                    else set.Add(line);

                }

                using var sw = new StreamWriter(File.Open(Map.CustomAttribFile, FileMode.Create));
                foreach (var line in set) sw.WriteLine(line);
                if (!set.Contains(this.Value)) sw.WriteLine(this.Value);

            }
            else
            {

                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Map.CustomAttribFile));
                using var sw = new StreamWriter(File.Open(Map.CustomAttribFile, FileMode.Create));
                sw.WriteLine(this.Value);
            }
        }

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
			this.ForeColor = theme.Colors.MainForeColor;
			this.AttribTypeComboBox.BackColor = theme.Colors.TextBoxBackColor;
			this.AttribTypeComboBox.ForeColor = theme.Colors.TextBoxForeColor;
			this.AttribKeyComboBox.BackColor = theme.Colors.TextBoxBackColor;
			this.AttribKeyComboBox.ForeColor = theme.Colors.TextBoxForeColor;
			this.AttribButtonCreate.BackColor = theme.Colors.ButtonBackColor;
			this.AttribButtonCreate.ForeColor = theme.Colors.ButtonForeColor;
			this.AttribButtonCreate.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
			this.AttribButtonHelp.BackColor = theme.Colors.ButtonBackColor;
			this.AttribButtonHelp.ForeColor = theme.Colors.ButtonForeColor;
			this.AttribButtonHelp.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
			this.label1.BackColor = theme.Colors.MainBackColor;
			this.label1.ForeColor = theme.Colors.LabelTextColor;
			this.label2.BackColor = theme.Colors.MainBackColor;
			this.label2.ForeColor = theme.Colors.LabelTextColor;
		}

		private void PopulateAttribTypesBasedOnGame(GameINT game)
		{
			// Since there is not easy type to just "GET" available attribute types,
			// we use what we have available in each module

			switch (game)
			{
				case GameINT.Carbon:
				case GameINT.Prostreet:
				case GameINT.Undercover:
					this.AttribTypeComboBox.Items.AddRange(new string[]
					{
						Boolean, /// <see cref="Nikki.Support.Carbon.Attributes.BoolAttribute"/>
						         /// <see cref="Nikki.Support.Prostreet.Attributes.BoolAttribute"/>						
						
						
						Color,   /// <see cref="Nikki.Support.Carbon.Attributes.ColorAttribute"/>
						         /// <see cref="Nikki.Support.Prostreet.Attributes.ColorAttribute"/>					
						
						
						Floating, /// <see cref="Nikki.Support.Carbon.Attributes.FloatAttribute"/>
						          /// <see cref="Nikki.Support.Prostreet.Attributes.FloatAttribute"/>
						
						
						Integer, /// <see cref="Nikki.Support.Carbon.Attributes.IntAttribute"/>
						         /// <see cref="Nikki.Support.Prostreet.Attributes.IntAttribute"/>
						
						
						Key, /// <see cref="Nikki.Support.Carbon.Attributes.KeyAttribute"/>
						     /// <see cref="Nikki.Support.Prostreet.Attributes.KeyAttribute"/>
						
						
						ModelTable, /// <see cref="Nikki.Support.Carbon.Attributes.ModelTableAttribute"/>
						            /// <see cref="Nikki.Support.Prostreet.Attributes.ModelTableAttribute"/>
						
						
						PartID, /// <see cref="Nikki.Support.Carbon.Attributes.PartIDAttribute"/>
						        /// <see cref="Nikki.Support.Prostreet.Attributes.PartIDAttribute"/>
						
						
						String, /// <see cref="Nikki.Support.Carbon.Attributes.StringAttribute"/>
						        /// <see cref="Nikki.Support.Prostreet.Attributes.StringAttribute"/>
						
						
						TwoString, /// <see cref="Nikki.Support.Carbon.Attributes.TwoStringAttribute"/>
						           /// <see cref="Nikki.Support.Prostreet.Attributes.TwoStringAttribute"/>
					
					});
					this.AttribTypeComboBox.SelectedIndex = 0;
					break;

				case GameINT.Underground1:
				case GameINT.Underground2:
				case GameINT.MostWanted:
					this.AttribTypeComboBox.Items.AddRange(new string[]
					{
						Boolean, /// <see cref="Nikki.Support.MostWanted.Attributes.BoolAttribute"/>
						         /// <see cref="Nikki.Support.Underground2.Attributes.BoolAttribute"/>
								 /// <see cref="Nikki.Support.Underground1.Attributes.BoolAttribute"/>


						Floating, /// <see cref="Nikki.Support.MostWanted.Attributes.FloatAttribute"/>
						          /// <see cref="Nikki.Support.Underground2.Attributes.FloatAttribute"/>
								  /// <see cref="Nikki.Support.Underground1.Attributes.FloatAttribute"/>


						Integer, /// <see cref="Nikki.Support.MostWanted.Attributes.IntAttribute"/>
						         /// <see cref="Nikki.Support.Underground2.Attributes.IntAttribute"/>
								 /// <see cref="Nikki.Support.Underground1.Attributes.IntAttribute"/>


						Key, /// <see cref="Nikki.Support.MostWanted.Attributes.KeyAttribute"/>
						     /// <see cref="Nikki.Support.Underground2.Attributes.KeyAttribute"/>
							 /// <see cref="Nikki.Support.Underground1.Attributes.KeyAttribute"/>


						String, /// <see cref="Nikki.Support.MostWanted.Attributes.StringAttribute"/>
						        /// <see cref="Nikki.Support.Underground2.Attributes.StringAttribute"/>
								/// <see cref="Nikki.Support.Underground1.Attributes.StringAttribute"/>

					});
					this.AttribTypeComboBox.SelectedIndex = 0;
					break;

				default:
					break;
			}
		}

		private void AttribButtonHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Choose attribute type that is going to be applied to a car part. " +
				"Then, based on the type chosen, type the attribute key that describes what " +
				"attribute is for.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void AttribButtonCreate_Click(object sender, EventArgs e)
		{
			var type = (this.AttribTypeComboBox.SelectedItem.ToString()) switch
			{
				Boolean => CarPartAttribType.Boolean,
				Color => CarPartAttribType.Color,
				Floating => CarPartAttribType.Floating,
				Integer => CarPartAttribType.Integer,
				Key => CarPartAttribType.Key,
                ModelTable => CarPartAttribType.ModelTable,
				PartID => CarPartAttribType.CarPartID,
				String => CarPartAttribType.String,
				TwoString => CarPartAttribType.TwoString,
				_ => CarPartAttribType.Integer,
            };

			this.Value = this.AttribKeyComboBox.Text.ToString();
            this.Type = type;
            this.DialogResult = DialogResult.OK;
            this.SaveHashList();
            this.Close();
		}
	}
}
