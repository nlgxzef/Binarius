using System;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    internal class CustomMenuStripColorTable : ProfessionalColorTable
    {
        /// <summary>
        /// Gets the start color of the gradient used in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuStripGradientBegin
        {
            get
            {
                return Properties.Settings.Default.MenuStripGradientBegin;
            }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuStripGradientEnd
        {
            get
            {
                return Properties.Settings.Default.MenuStripGradientEnd;
            }
        }

        /// <summary>
        /// Gets the start color of the gradient used when the top-level menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return Properties.Settings.Default.MenuItemPressedGradientBegin;
            }
        }

        /// <summary>
        /// Gets the middle color of the gradient used when the top-level menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuItemPressedGradientMiddle
        {
            get
            {
                return Properties.Settings.Default.MenuItemPressedGradientMiddle;
            }
        }

        /// <summary>
        /// Gets the end color of the gradient used when the top-level menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return Properties.Settings.Default.MenuItemPressedGradientEnd;
            }
        }

        /// <summary>
        /// Gets the start color of the gradient used when the menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return Properties.Settings.Default.MenuItemSelectedGradientBegin;
            }
        }

        /// <summary>
        /// Gets the end color of the gradient used when the menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return Properties.Settings.Default.MenuItemSelectedGradientEnd;
            }
        }

        /// <summary>
        /// Gets the color used when the menu item is selected in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuItemSelected
        {
            get
            {
                return Properties.Settings.Default.MenuItemSelected;
            }
        }

        /// <summary>
        /// Gets the color used for the menu border in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuBorder
        {
            get
            {
                return Properties.Settings.Default.MenuBorder;
            }
        }

        /// <summary>
        /// Gets the color used for the menu item border in the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color MenuItemBorder
        {
            get
            {
                return Properties.Settings.Default.MenuItemBorder;
            }
        }

        /// <summary>
        /// Gets the starting color of the gradient used in the image margin of the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color ImageMarginGradientBegin
        {
            get
            {
                return Properties.Settings.Default.ImageMarginGradientBegin;
            }
        }

        /// <summary>
        /// Gets the middle color of the gradient used in the image margin of the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return Properties.Settings.Default.ImageMarginGradientMiddle;
            }
        }

        /// <summary>
        /// Gets the ending color of the gradient used in the image margin of the System.Windows.Forms.MenuStrip control.
        /// </summary>
        public override Color ImageMarginGradientEnd
        {
            get
            {
                return Properties.Settings.Default.ImageMarginGradientEnd;
            }
        }
    }
}
