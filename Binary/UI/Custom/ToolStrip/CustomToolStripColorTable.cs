using System;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    internal class CustomToolStripColorTable : ProfessionalColorTable
    {
        /// <summary>
        /// Gets the border color of the System.Windows.Forms.ToolStrip control.
        /// </summary>
        public override Color ToolStripBorder
        {
            get
            {
                return Properties.Settings.Default.ToolStripBorder;
            }
        }

        /// <summary>
        /// Gets the starting color of the content panel gradient on System.Windows.Forms.ToolStrip control.
        /// </summary>
        public override Color ToolStripContentPanelGradientBegin
        {
            get
            {
                return Properties.Settings.Default.ToolStripContentPanelGradientBegin;
            }
        }

        /// <summary>
        /// Gets the ending color of the content panel gradient on System.Windows.Forms.ToolStrip control.
        /// </summary>
        public override Color ToolStripContentPanelGradientEnd
        {
            get
            {
                return Properties.Settings.Default.ToolStripContentPanelGradientEnd;
            }
        }

        /// <summary>
        /// Gets the background color of the drop down on the System.Windows.Forms.ToolStrip control.
        /// </summary>
        public override Color ToolStripDropDownBackground
        {
            get
            {
                return Properties.Settings.Default.ToolStripDropDownBackground;
            }
        }

        /// <summary>
        /// Gets the starting color of the gradient on the System.Windows.Forms.ToolStrip control.
        /// </summary>
        public override Color ToolStripGradientBegin
        {
            get
            {
                return Properties.Settings.Default.ToolStripGradientBegin;
            }
        }

        /// <summary>
        /// Gets the middle color of the gradient on the System.Windows.Forms.ToolStrip control.
        /// </summary>
        public override Color ToolStripGradientMiddle
        {
            get
            {
                return Properties.Settings.Default.ToolStripGradientMiddle;
            }
        }

        /// <summary>
        /// Gets the ending color of the gradient on the System.Windows.Forms.ToolStrip control.
        /// </summary>
        public override Color ToolStripGradientEnd
        {
            get
            {
                return Properties.Settings.Default.ToolStripGradientEnd;
            }
        }
    }
}
