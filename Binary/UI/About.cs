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
            Binary.UI.DialogLayout.Attach(this); // re-flow for the current DPI before showing
            this.ToggleTheme();
            this.labelProductName.Text = "Binarius";
            this.labelVersion.Text = String.Format("v{0}", AssemblyVersion);
        #if DEBUG
            this.labelVersion.Text += " (Debug)";
        #endif
            this.labelCopyright.UseMnemonic = false; // shows the & in the copyright
            this.labelCopyright.Text = AssemblyCopyright;
            this.textBoxDescription.Text = String.Join("\r\n", Credits);
        }

        private static readonly string[] Credits =
        {
            "Tool for editing Need for Speed binary files (.BIN, .BUN, .LZC).",
            "Based on Binary by MaxHwoy and also implements various fixes and improvements done by the NFS Unlimiter Community.",
            "Powered with Endscript and Darius.",
            "",
            "==== Binarius ====",
            "Main developer:",
            "- nlgxzef",
            "",
            "Thanks to:",
            "- MaxHwoy for creating Binary in the first place.",
            "- rx and avail for unofficial v2.9.0 (NFSCO-Binary), where this fork is started from.",
            "- Sh2dow for their fork with some QoL improvements.",
            "- NI240SX for their fork (Binercover) with QoL improvements and better NFS Undercover support.",
            "- Felipe379 for NFS Carbon preset ride wheel paint support.",
            "- NFS Unlimiter Community for great help; mods, ideas, requests and motivation.",
            "",
            "==== Original credits from Binary ====",
            "Coded by:",
            "- MaxHwoy",
            "",
            "Undercover support by:",
            "- nlgxzef",
            "",
            "Biggest thanks to:",
            "- 379Felipe for testing and resources.",
            "- heyitsleo for great help.",
            "- avail for great help.",
            "",
            "Testers:",
            "379Felipe, ARCHIE, Aceofsquares3, Avalanche, CheapAssVideos, Elven, GhostRider, NFSBOI, rx, konigsseg, elaymm4, Lexal, Miles Dipperion, Miner, MrAdam, nlgxzef, RaTT, Red_Fire, RedCarDriver, rnz, trackmaniamatt, Neon, Zyr3X",
        };

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
