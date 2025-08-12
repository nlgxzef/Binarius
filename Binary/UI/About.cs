using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
            this.labelCopyright.Text = "© 2025 MaxHwoy, rx, Sh2dow and nlgxzef.\r\nNo rights reserved.";
            this.textBoxDescription.Text = AssemblyDescription + "\r\n"
                + "Powered with Endscript and Darius.";
        }

        #region Theme

        private void ToggleTheme()
        {
            this.BackColor = Theme.MainBackColor;
            this.ForeColor = Theme.MainForeColor;

            this.okButton.BackColor = Theme.ButtonBackColor;
            this.okButton.ForeColor = Theme.ButtonForeColor;
            this.okButton.FlatAppearance.BorderColor = Theme.ButtonFlatColor;
            this.textBoxDescription.BackColor = Theme.TextBoxBackColor;
            this.textBoxDescription.ForeColor = Theme.TextBoxForeColor;
            this.labelProductName.ForeColor = Theme.LabelTextColor;
            this.labelVersion.ForeColor = Theme.LabelTextColor;
            this.labelCopyright.ForeColor = Theme.LabelTextColor;
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
