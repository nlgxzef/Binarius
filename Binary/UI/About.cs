using Binary.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Binary.UI
{
    partial class About : Form
    {
        public About()
        {
            this.InitializeComponent();
            this.ToggleTheme();
            this.labelProductName.Text = "Binarius";
            this.labelVersion.Text = String.Format("v{0}", AssemblyVersion);
        #if DEBUG
            this.labelVersion.Text += " (Debug)";
        #endif
            this.labelCopyright.Text = "© 2025 MaxHwoy, rx, avail, Sh2dow and nlgxzef.\r\nNo rights reserved.";
            this.textBoxDescription.Text = AssemblyDescription + "\r\n"
                + "Powered with Endscript and Darius.";
        }

        #region Theme

        private void ToggleTheme()
        {
            Theme.Deserialize(Theme.GetThemeFile(), out var theme);

            this.BackColor = theme.Colors.MainBackColor;
            this.ForeColor = theme.Colors.MainForeColor;

            this.okButton.BackColor = theme.Colors.ButtonBackColor;
            this.okButton.ForeColor = theme.Colors.ButtonForeColor;
            this.okButton.FlatAppearance.BorderColor = theme.Colors.ButtonFlatColor;
            this.textBoxDescription.BackColor = theme.Colors.TextBoxBackColor;
            this.textBoxDescription.ForeColor = theme.Colors.TextBoxForeColor;
            this.labelProductName.ForeColor = theme.Colors.LabelTextColor;
            this.labelVersion.ForeColor = theme.Colors.LabelTextColor;
            this.labelCopyright.ForeColor = theme.Colors.LabelTextColor;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
