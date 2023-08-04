namespace Binary.Tools
{
    partial class Swatcher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Swatcher));
            GroupBoxRGB = new System.Windows.Forms.GroupBox();
            LabelBlue = new System.Windows.Forms.Label();
            LabelGreen = new System.Windows.Forms.Label();
            LabelRed = new System.Windows.Forms.Label();
            TrackBar_Blue = new System.Windows.Forms.TrackBar();
            TrackBar_Green = new System.Windows.Forms.TrackBar();
            TrackBar_Red = new System.Windows.Forms.TrackBar();
            OpenWindowsColorForm = new System.Windows.Forms.Button();
            ColorPreview = new System.Windows.Forms.PictureBox();
            SwatchDialog = new System.Windows.Forms.ColorDialog();
            GroupBoxSwatch = new System.Windows.Forms.GroupBox();
            CopyBrightnessValue = new System.Windows.Forms.Button();
            CopySaturationValue = new System.Windows.Forms.Button();
            CopyPaintSwatchValue = new System.Windows.Forms.Button();
            TextBoxBrightness = new System.Windows.Forms.TextBox();
            TextBoxSaturation = new System.Windows.Forms.TextBox();
            TextBoxPaintSwatch = new System.Windows.Forms.TextBox();
            LabelBrightness = new System.Windows.Forms.Label();
            LabelSaturation = new System.Windows.Forms.Label();
            LabelPaintSwatch = new System.Windows.Forms.Label();
            GroupBoxRGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrackBar_Blue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrackBar_Green).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrackBar_Red).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ColorPreview).BeginInit();
            GroupBoxSwatch.SuspendLayout();
            SuspendLayout();
            // 
            // GroupBoxRGB
            // 
            GroupBoxRGB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GroupBoxRGB.Controls.Add(LabelBlue);
            GroupBoxRGB.Controls.Add(LabelGreen);
            GroupBoxRGB.Controls.Add(LabelRed);
            GroupBoxRGB.Controls.Add(TrackBar_Blue);
            GroupBoxRGB.Controls.Add(TrackBar_Green);
            GroupBoxRGB.Controls.Add(TrackBar_Red);
            GroupBoxRGB.Location = new System.Drawing.Point(29, 21);
            GroupBoxRGB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBoxRGB.Name = "GroupBoxRGB";
            GroupBoxRGB.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBoxRGB.Size = new System.Drawing.Size(416, 228);
            GroupBoxRGB.TabIndex = 1;
            GroupBoxRGB.TabStop = false;
            GroupBoxRGB.Text = "RGB Value Picker";
            GroupBoxRGB.Paint += GroupBoxRGB_Paint;
            // 
            // LabelBlue
            // 
            LabelBlue.AutoSize = true;
            LabelBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LabelBlue.Location = new System.Drawing.Point(22, 151);
            LabelBlue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelBlue.Name = "LabelBlue";
            LabelBlue.Size = new System.Drawing.Size(41, 20);
            LabelBlue.TabIndex = 26;
            LabelBlue.Text = "Blue";
            // 
            // LabelGreen
            // 
            LabelGreen.AutoSize = true;
            LabelGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LabelGreen.Location = new System.Drawing.Point(7, 92);
            LabelGreen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelGreen.Name = "LabelGreen";
            LabelGreen.Size = new System.Drawing.Size(54, 20);
            LabelGreen.TabIndex = 25;
            LabelGreen.Text = "Green";
            // 
            // LabelRed
            // 
            LabelRed.AutoSize = true;
            LabelRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LabelRed.Location = new System.Drawing.Point(24, 39);
            LabelRed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelRed.Name = "LabelRed";
            LabelRed.Size = new System.Drawing.Size(39, 20);
            LabelRed.TabIndex = 24;
            LabelRed.Text = "Red";
            // 
            // TrackBar_Blue
            // 
            TrackBar_Blue.Location = new System.Drawing.Point(77, 157);
            TrackBar_Blue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TrackBar_Blue.Maximum = 255;
            TrackBar_Blue.Name = "TrackBar_Blue";
            TrackBar_Blue.Size = new System.Drawing.Size(326, 45);
            TrackBar_Blue.TabIndex = 16;
            TrackBar_Blue.Scroll += TrackBar_Blue_Scroll;
            // 
            // TrackBar_Green
            // 
            TrackBar_Green.Location = new System.Drawing.Point(77, 98);
            TrackBar_Green.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TrackBar_Green.Maximum = 255;
            TrackBar_Green.Name = "TrackBar_Green";
            TrackBar_Green.Size = new System.Drawing.Size(326, 45);
            TrackBar_Green.TabIndex = 15;
            TrackBar_Green.Scroll += TrackBar_Green_Scroll;
            // 
            // TrackBar_Red
            // 
            TrackBar_Red.Location = new System.Drawing.Point(77, 39);
            TrackBar_Red.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TrackBar_Red.Maximum = 255;
            TrackBar_Red.Name = "TrackBar_Red";
            TrackBar_Red.Size = new System.Drawing.Size(326, 45);
            TrackBar_Red.TabIndex = 14;
            TrackBar_Red.Scroll += TrackBar_Red_Scroll;
            // 
            // OpenWindowsColorForm
            // 
            OpenWindowsColorForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            OpenWindowsColorForm.Location = new System.Drawing.Point(370, 293);
            OpenWindowsColorForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            OpenWindowsColorForm.Name = "OpenWindowsColorForm";
            OpenWindowsColorForm.Size = new System.Drawing.Size(306, 30);
            OpenWindowsColorForm.TabIndex = 27;
            OpenWindowsColorForm.Text = "Use Windows Color Picker";
            OpenWindowsColorForm.UseVisualStyleBackColor = false;
            OpenWindowsColorForm.Click += OpenWindowsColorForm_Click;
            // 
            // ColorPreview
            // 
            ColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ColorPreview.Location = new System.Drawing.Point(182, 276);
            ColorPreview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ColorPreview.Name = "ColorPreview";
            ColorPreview.Size = new System.Drawing.Size(162, 59);
            ColorPreview.TabIndex = 23;
            ColorPreview.TabStop = false;
            // 
            // GroupBoxSwatch
            // 
            GroupBoxSwatch.Controls.Add(CopyBrightnessValue);
            GroupBoxSwatch.Controls.Add(CopySaturationValue);
            GroupBoxSwatch.Controls.Add(CopyPaintSwatchValue);
            GroupBoxSwatch.Controls.Add(TextBoxBrightness);
            GroupBoxSwatch.Controls.Add(TextBoxSaturation);
            GroupBoxSwatch.Controls.Add(TextBoxPaintSwatch);
            GroupBoxSwatch.Controls.Add(LabelBrightness);
            GroupBoxSwatch.Controls.Add(LabelSaturation);
            GroupBoxSwatch.Controls.Add(LabelPaintSwatch);
            GroupBoxSwatch.Location = new System.Drawing.Point(453, 21);
            GroupBoxSwatch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBoxSwatch.Name = "GroupBoxSwatch";
            GroupBoxSwatch.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBoxSwatch.Size = new System.Drawing.Size(346, 228);
            GroupBoxSwatch.TabIndex = 28;
            GroupBoxSwatch.TabStop = false;
            GroupBoxSwatch.Text = "Swatch Color";
            GroupBoxSwatch.Paint += GroupBoxSwatch_Paint;
            // 
            // CopyBrightnessValue
            // 
            CopyBrightnessValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyBrightnessValue.Location = new System.Drawing.Point(230, 159);
            CopyBrightnessValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyBrightnessValue.Name = "CopyBrightnessValue";
            CopyBrightnessValue.Size = new System.Drawing.Size(96, 25);
            CopyBrightnessValue.TabIndex = 35;
            CopyBrightnessValue.Text = "Copy";
            CopyBrightnessValue.UseVisualStyleBackColor = false;
            CopyBrightnessValue.Click += CopyBrightnessValue_Click;
            // 
            // CopySaturationValue
            // 
            CopySaturationValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopySaturationValue.Location = new System.Drawing.Point(230, 106);
            CopySaturationValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopySaturationValue.Name = "CopySaturationValue";
            CopySaturationValue.Size = new System.Drawing.Size(96, 25);
            CopySaturationValue.TabIndex = 34;
            CopySaturationValue.Text = "Copy";
            CopySaturationValue.UseVisualStyleBackColor = false;
            CopySaturationValue.Click += CopySaturationValue_Click;
            // 
            // CopyPaintSwatchValue
            // 
            CopyPaintSwatchValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyPaintSwatchValue.Location = new System.Drawing.Point(230, 50);
            CopyPaintSwatchValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyPaintSwatchValue.Name = "CopyPaintSwatchValue";
            CopyPaintSwatchValue.Size = new System.Drawing.Size(96, 25);
            CopyPaintSwatchValue.TabIndex = 33;
            CopyPaintSwatchValue.Text = "Copy";
            CopyPaintSwatchValue.UseVisualStyleBackColor = false;
            CopyPaintSwatchValue.Click += CopyPaintSwatchValue_Click;
            // 
            // TextBoxBrightness
            // 
            TextBoxBrightness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBoxBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TextBoxBrightness.Location = new System.Drawing.Point(128, 159);
            TextBoxBrightness.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxBrightness.Name = "TextBoxBrightness";
            TextBoxBrightness.ReadOnly = true;
            TextBoxBrightness.Size = new System.Drawing.Size(94, 22);
            TextBoxBrightness.TabIndex = 32;
            TextBoxBrightness.Text = "0";
            // 
            // TextBoxSaturation
            // 
            TextBoxSaturation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBoxSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TextBoxSaturation.Location = new System.Drawing.Point(128, 106);
            TextBoxSaturation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxSaturation.Name = "TextBoxSaturation";
            TextBoxSaturation.ReadOnly = true;
            TextBoxSaturation.Size = new System.Drawing.Size(94, 22);
            TextBoxSaturation.TabIndex = 31;
            TextBoxSaturation.Text = "0";
            // 
            // TextBoxPaintSwatch
            // 
            TextBoxPaintSwatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextBoxPaintSwatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TextBoxPaintSwatch.Location = new System.Drawing.Point(128, 50);
            TextBoxPaintSwatch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxPaintSwatch.Name = "TextBoxPaintSwatch";
            TextBoxPaintSwatch.ReadOnly = true;
            TextBoxPaintSwatch.Size = new System.Drawing.Size(94, 22);
            TextBoxPaintSwatch.TabIndex = 30;
            TextBoxPaintSwatch.Text = "0";
            // 
            // LabelBrightness
            // 
            LabelBrightness.AutoSize = true;
            LabelBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LabelBrightness.Location = new System.Drawing.Point(7, 159);
            LabelBrightness.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelBrightness.Name = "LabelBrightness";
            LabelBrightness.Size = new System.Drawing.Size(85, 20);
            LabelBrightness.TabIndex = 29;
            LabelBrightness.Text = "Brightness";
            // 
            // LabelSaturation
            // 
            LabelSaturation.AutoSize = true;
            LabelSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LabelSaturation.Location = new System.Drawing.Point(7, 106);
            LabelSaturation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelSaturation.Name = "LabelSaturation";
            LabelSaturation.Size = new System.Drawing.Size(83, 20);
            LabelSaturation.TabIndex = 28;
            LabelSaturation.Text = "Saturation";
            // 
            // LabelPaintSwatch
            // 
            LabelPaintSwatch.AutoSize = true;
            LabelPaintSwatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LabelPaintSwatch.Location = new System.Drawing.Point(7, 50);
            LabelPaintSwatch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelPaintSwatch.Name = "LabelPaintSwatch";
            LabelPaintSwatch.Size = new System.Drawing.Size(98, 20);
            LabelPaintSwatch.TabIndex = 27;
            LabelPaintSwatch.Text = "PaintSwatch";
            // 
            // Swatcher
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(830, 361);
            Controls.Add(GroupBoxSwatch);
            Controls.Add(OpenWindowsColorForm);
            Controls.Add(GroupBoxRGB);
            Controls.Add(ColorPreview);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Swatcher";
            Text = "Swatcher by MaxHwoy";
            GroupBoxRGB.ResumeLayout(false);
            GroupBoxRGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TrackBar_Blue).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrackBar_Green).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrackBar_Red).EndInit();
            ((System.ComponentModel.ISupportInitialize)ColorPreview).EndInit();
            GroupBoxSwatch.ResumeLayout(false);
            GroupBoxSwatch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBoxRGB;
        private System.Windows.Forms.Button OpenWindowsColorForm;
        private System.Windows.Forms.Label LabelBlue;
        private System.Windows.Forms.Label LabelGreen;
        private System.Windows.Forms.Label LabelRed;
        private System.Windows.Forms.PictureBox ColorPreview;
        private System.Windows.Forms.TrackBar TrackBar_Blue;
        private System.Windows.Forms.TrackBar TrackBar_Green;
        private System.Windows.Forms.TrackBar TrackBar_Red;
        private System.Windows.Forms.ColorDialog SwatchDialog;
        private System.Windows.Forms.GroupBox GroupBoxSwatch;
        private System.Windows.Forms.TextBox TextBoxPaintSwatch;
        private System.Windows.Forms.Label LabelBrightness;
        private System.Windows.Forms.Label LabelSaturation;
        private System.Windows.Forms.Label LabelPaintSwatch;
        private System.Windows.Forms.Button CopyPaintSwatchValue;
        private System.Windows.Forms.TextBox TextBoxBrightness;
        private System.Windows.Forms.TextBox TextBoxSaturation;
        private System.Windows.Forms.Button CopyBrightnessValue;
        private System.Windows.Forms.Button CopySaturationValue;
    }
}