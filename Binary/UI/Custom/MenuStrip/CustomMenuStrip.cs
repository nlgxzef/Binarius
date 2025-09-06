using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class CustomMenuStrip : MenuStrip
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of System.Windows.Forms.CustomMenuStrip.
        /// </summary>
        public CustomMenuStrip()
        {
            InitializeComponent();

            this.RenderMode = ToolStripRenderMode.Professional;
            this.Renderer = new ToolStripProfessionalRenderer(new CustomMenuStripColorTable());
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ForeColor of the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuStripForeColor")]
        public Color MenuStripForeColor
        {
            get { return Properties.Settings.Default.MenuStripForeColor; }
            set
            {
                Properties.Settings.Default.MenuStripForeColor = value;
                this.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the start color of the gradient used in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuStripGradientBegin")]
        public Color MenuStripGradientBegin
        {
            get { return Properties.Settings.Default.MenuStripGradientBegin; }
            set { Properties.Settings.Default.MenuStripGradientBegin = value; }
        }

        /// <summary>
        /// Gets or sets the end color of the gradient used in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuStripGradientEnd")]
        public Color MenuStripGradientEnd
        {
            get { return Properties.Settings.Default.MenuStripGradientEnd; }
            set { Properties.Settings.Default.MenuStripGradientEnd = value; }
        }

        /// <summary>
        /// Gets or sets the start color of the gradient used when the top-level menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuItemPressedGradientBegin")]
        public Color MenuItemPressedGradientBegin
        {
            get { return Properties.Settings.Default.MenuStripGradientEnd; }
            set { Properties.Settings.Default.MenuStripGradientEnd = value; }
        }

        /// <summary>
        /// Gets or sets the middle color of the gradient used when the top-level menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuItemPressedGradientMiddle")]
        public Color MenuItemPressedGradientMiddle
        {
            get { return Properties.Settings.Default.MenuStripGradientEnd; }
            set { Properties.Settings.Default.MenuStripGradientEnd = value; }
        }

        /// <summary>
        /// Gets or sets the end color of the gradient used when the top-level menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuItemPressedGradientEnd")]
        public Color MenuItemPressedGradientEnd
        {
            get { return Properties.Settings.Default.MenuStripGradientEnd; }
            set { Properties.Settings.Default.MenuStripGradientEnd = value; }
        }

        /// <summary>
        /// Gets or sets the end color of the gradient used when the menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuItemSelectedGradientBegin")]
        public Color MenuItemSelectedGradientBegin
        {
            get { return Properties.Settings.Default.MenuItemSelectedGradientBegin; }
            set { Properties.Settings.Default.MenuItemSelectedGradientBegin = value; }
        }

        /// <summary>
        /// Gets or sets the end color of the gradient used when the menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuItemSelectedGradientEnd")]
        public Color MenuItemSelectedGradientEnd
        {
            get { return Properties.Settings.Default.MenuItemSelectedGradientEnd; }
            set { Properties.Settings.Default.MenuItemSelectedGradientEnd = value; }
        }

        /// <summary>
        /// Gets or sets the color used when the menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuItemSelected")]
        [Description("The color used when the menu item is selected in the System.Windows.Forms.MenuStrip control.")]
        public Color MenuItemSelected
        {
            get { return Properties.Settings.Default.MenuItemSelected; }
            set { Properties.Settings.Default.MenuItemSelected = value; }
        }

        /// <summary>
        /// Gets or sets the color used when the menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuBorder")]
        public Color MenuBorder
        {
            get { return Properties.Settings.Default.MenuBorder; }
            set { Properties.Settings.Default.MenuBorder = value; }
        }

        /// <summary>
        /// Gets or sets the color used when the menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("MenuItemBorder")]
        public Color MenuItemBorder
        {
            get { return Properties.Settings.Default.MenuItemBorder; }
            set { Properties.Settings.Default.MenuItemBorder = value; }
        }

        /// <summary>
        /// Gets or sets the starting color of the gradient used in the image margin of the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ImageMarginGradientBegin")]
        public Color ImageMarginGradientBegin
        {
            get { return Properties.Settings.Default.ImageMarginGradientBegin; }
            set { Properties.Settings.Default.ImageMarginGradientBegin = value; }
        }

        /// <summary>
        /// Gets or sets the middle color of the gradient used in the image margin of the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ImageMarginGradientMiddle")]
        public Color ImageMarginGradientMiddle
        {
            get { return Properties.Settings.Default.ImageMarginGradientMiddle; }
            set { Properties.Settings.Default.ImageMarginGradientMiddle = value; }
        }

        /// <summary>
        /// Gets or sets the ending color of the gradient used in the image margin of the System.Windows.Forms.MenuStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ImageMarginGradientEnd")]
        public Color ImageMarginGradientEnd
        {
            get { return Properties.Settings.Default.ImageMarginGradientEnd; }
            set { Properties.Settings.Default.ImageMarginGradientEnd = value; }
        }
        #endregion
    }
}
