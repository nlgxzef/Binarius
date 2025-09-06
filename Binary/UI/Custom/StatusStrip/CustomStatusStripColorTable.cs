using System;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    internal class CustomStatusStripColorTable : ProfessionalColorTable
    {
        /// <summary>
        /// Gets the starting color of the gradient used on the System.Windows.Forms.StatusStrip control.
        /// </summary>
        public override Color StatusStripGradientBegin
        {
            get
            {
                return Properties.Settings.Default.StatusStripGradientBegin;
            }
        }

        /// <summary>
        /// Gets the ending color of the gradient used on the System.Windows.Forms.StatusStrip control.
        /// </summary>
        public override Color StatusStripGradientEnd
        {
            get
            {
                return Properties.Settings.Default.StatusStripGradientEnd;
            }
        }
    }
}
