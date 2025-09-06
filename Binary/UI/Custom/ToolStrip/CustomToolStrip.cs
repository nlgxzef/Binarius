using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class CustomToolStrip : ToolStrip
    {
        #region Constructor
        public CustomToolStrip()
        {
            InitializeComponent();

            this.RenderMode = ToolStripRenderMode.Professional;
            this.Renderer = new ToolStripProfessionalRenderer(new CustomToolStripColorTable());
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ForeColor of the System.Windows.Forms.ToolStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ToolStripForeColor")]
        public Color ToolStripForeColor
        {
            get { return Properties.Settings.Default.ToolStripForeColor; }
            set
            {
                Properties.Settings.Default.ToolStripForeColor = value;
                this.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the border color of the System.Windows.Forms.ToolStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ToolStripBorder")]
        public Color ToolStripBorder
        {
            get { return Properties.Settings.Default.ToolStripBorder; }
            set { Properties.Settings.Default.ToolStripBorder = value; }
        }

        /// <summary>
        /// Gets or sets the starting color of the content panel gradient on System.Windows.Forms.ToolStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ToolStripContentPanelGradientBegin")]
        public Color ToolStripContentPanelGradientBegin
        {
            get { return Properties.Settings.Default.ToolStripContentPanelGradientBegin; }
            set { Properties.Settings.Default.ToolStripContentPanelGradientBegin = value; }
        }

        /// <summary>
        /// Gets or sets the ending color of the content panel gradient on System.Windows.Forms.ToolStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ToolStripContentPanelGradientEnd")]
        public Color ToolStripContentPanelGradientEnd
        {
            get { return Properties.Settings.Default.ToolStripContentPanelGradientEnd; }
            set { Properties.Settings.Default.ToolStripContentPanelGradientEnd = value; }
        }

        /// <summary>
        /// Gets or sets the background color of the drop down on the System.Windows.Forms.ToolStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ToolStripDropDownBackground")]
        public Color ToolStripDropDownBackground
        {
            get { return Properties.Settings.Default.ToolStripDropDownBackground; }
            set { Properties.Settings.Default.ToolStripDropDownBackground = value; }
        }

        /// <summary>
        /// Gets or sets the starting color of the gradient on the System.Windows.Forms.ToolStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ToolStripGradientBegin")]
        public Color ToolStripGradientBegin
        {
            get { return Properties.Settings.Default.ToolStripGradientBegin; }
            set { Properties.Settings.Default.ToolStripGradientBegin = value; }
        }

        /// <summary>
        /// Gets or sets the middle color of the gradient on the System.Windows.Forms.ToolStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ToolStripGradientMiddle")]
        public Color ToolStripGradientMiddle
        {
            get { return Properties.Settings.Default.ToolStripGradientMiddle; }
            set { Properties.Settings.Default.ToolStripGradientMiddle = value; }
        }

        /// <summary>
        /// Gets or sets the ending color of the gradient on the System.Windows.Forms.ToolStrip control.
        /// </summary>
        [Category("Style")]
        [DisplayName("ToolStripGradientEnd")]
        public Color ToolStripGradientEnd
        {
            get { return Properties.Settings.Default.ToolStripGradientEnd; }
            set { Properties.Settings.Default.ToolStripGradientEnd = value; }
        }
        #endregion
    }
}
