namespace Binary.Interact
{
	partial class Options
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
            this.components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.OptionsButtonOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OptionsCheckDisableAdminWarning = new System.Windows.Forms.CheckBox();
            this.OptionsCheckSoonFeature = new System.Windows.Forms.CheckBox();
            this.OptionsCheckStartMaximized = new System.Windows.Forms.CheckBox();
            this.OptionsCheckAutoBackups = new System.Windows.Forms.CheckBox();
            this.OptionsLabelWatermark = new System.Windows.Forms.Label();
            this.OptionsTextWatermark = new System.Windows.Forms.TextBox();
            this.OptionsButtonCancel = new System.Windows.Forms.Button();
            this.OptionsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionsButtonOK
            // 
            this.OptionsButtonOK.AutoSize = true;
            this.OptionsButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OptionsButtonOK.Location = new System.Drawing.Point(243, 233);
            this.OptionsButtonOK.Name = "OptionsButtonOK";
            this.OptionsButtonOK.Size = new System.Drawing.Size(112, 35);
            this.OptionsButtonOK.TabIndex = 3;
            this.OptionsButtonOK.Text = "OK";
            this.OptionsButtonOK.UseVisualStyleBackColor = false;
            this.OptionsButtonOK.Click += this.OptionsButtonOK_Click;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.OptionsCheckDisableAdminWarning);
            this.panel1.Controls.Add(this.OptionsCheckSoonFeature);
            this.panel1.Controls.Add(this.OptionsCheckStartMaximized);
            this.panel1.Controls.Add(this.OptionsCheckAutoBackups);
            this.panel1.Controls.Add(this.OptionsLabelWatermark);
            this.panel1.Controls.Add(this.OptionsTextWatermark);
            this.panel1.Location = new System.Drawing.Point(12, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 216);
            this.panel1.TabIndex = 4;
            // 
            // OptionsCheckDisableAdminWarning
            // 
            this.OptionsCheckDisableAdminWarning.AutoSize = true;
            this.OptionsCheckDisableAdminWarning.Location = new System.Drawing.Point(3, 117);
            this.OptionsCheckDisableAdminWarning.Name = "OptionsCheckDisableAdminWarning";
            this.OptionsCheckDisableAdminWarning.Size = new System.Drawing.Size(220, 19);
            this.OptionsCheckDisableAdminWarning.TabIndex = 2;
            this.OptionsCheckDisableAdminWarning.Text = "Disable Administrator mode warning";
            this.OptionsToolTip.SetToolTip(this.OptionsCheckDisableAdminWarning, "Disables the warning that gets shown when Binarius is not ran in Administrator mode.");
            this.OptionsCheckDisableAdminWarning.UseVisualStyleBackColor = true;
            // 
            // OptionsCheckSoonFeature
            // 
            this.OptionsCheckSoonFeature.AutoSize = true;
            this.OptionsCheckSoonFeature.Location = new System.Drawing.Point(3, 92);
            this.OptionsCheckSoonFeature.Name = "OptionsCheckSoonFeature";
            this.OptionsCheckSoonFeature.Size = new System.Drawing.Size(178, 19);
            this.OptionsCheckSoonFeature.TabIndex = 2;
            this.OptionsCheckSoonFeature.Text = "Enable experimental features";
            this.OptionsToolTip.SetToolTip(this.OptionsCheckSoonFeature, "Enables experimental features.");
            this.OptionsCheckSoonFeature.UseVisualStyleBackColor = true;
            // 
            // OptionsCheckStartMaximized
            // 
            this.OptionsCheckStartMaximized.AutoSize = true;
            this.OptionsCheckStartMaximized.Location = new System.Drawing.Point(3, 67);
            this.OptionsCheckStartMaximized.Name = "OptionsCheckStartMaximized";
            this.OptionsCheckStartMaximized.Size = new System.Drawing.Size(192, 19);
            this.OptionsCheckStartMaximized.TabIndex = 2;
            this.OptionsCheckStartMaximized.Text = "Start editor in maximized mode";
            this.OptionsToolTip.SetToolTip(this.OptionsCheckStartMaximized, "Starts editor window in maximized mode.");
            this.OptionsCheckStartMaximized.UseVisualStyleBackColor = true;
            // 
            // OptionsCheckAutoBackups
            // 
            this.OptionsCheckAutoBackups.AutoSize = true;
            this.OptionsCheckAutoBackups.Location = new System.Drawing.Point(3, 42);
            this.OptionsCheckAutoBackups.Name = "OptionsCheckAutoBackups";
            this.OptionsCheckAutoBackups.Size = new System.Drawing.Size(179, 19);
            this.OptionsCheckAutoBackups.TabIndex = 2;
            this.OptionsCheckAutoBackups.Text = "Automatically make backups";
            this.OptionsToolTip.SetToolTip(this.OptionsCheckAutoBackups, "Automatically makes backups of the files modified and saved using Binarius if they didn't get backed up before.\r\nBackup files have .bacc file format.");
            this.OptionsCheckAutoBackups.UseVisualStyleBackColor = true;
            // 
            // OptionsLabelWatermark
            // 
            this.OptionsLabelWatermark.AutoSize = true;
            this.OptionsLabelWatermark.Location = new System.Drawing.Point(3, 6);
            this.OptionsLabelWatermark.Name = "OptionsLabelWatermark";
            this.OptionsLabelWatermark.Size = new System.Drawing.Size(65, 15);
            this.OptionsLabelWatermark.TabIndex = 1;
            this.OptionsLabelWatermark.Text = "Watermark";
            this.OptionsToolTip.SetToolTip(this.OptionsLabelWatermark, "Adds this watermark into the files modified and saved using Binarius.");
            // 
            // OptionsTextWatermark
            // 
            this.OptionsTextWatermark.Location = new System.Drawing.Point(74, 3);
            this.OptionsTextWatermark.Name = "OptionsTextWatermark";
            this.OptionsTextWatermark.Size = new System.Drawing.Size(384, 23);
            this.OptionsTextWatermark.TabIndex = 0;
            // 
            // OptionsButtonCancel
            // 
            this.OptionsButtonCancel.AutoSize = true;
            this.OptionsButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OptionsButtonCancel.Location = new System.Drawing.Point(361, 233);
            this.OptionsButtonCancel.Name = "OptionsButtonCancel";
            this.OptionsButtonCancel.Size = new System.Drawing.Size(112, 35);
            this.OptionsButtonCancel.TabIndex = 3;
            this.OptionsButtonCancel.Text = "Cancel";
            this.OptionsButtonCancel.UseVisualStyleBackColor = false;
            this.OptionsButtonCancel.Click += this.OptionsButtonCancel_Click;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(485, 282);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.OptionsButtonCancel);
            this.Controls.Add(this.OptionsButtonOK);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OptionsButtonOK;
		private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OptionsButtonCancel;
        private System.Windows.Forms.TextBox OptionsTextWatermark;
        private System.Windows.Forms.Label OptionsLabelWatermark;
        private System.Windows.Forms.CheckBox OptionsCheckDisableAdminWarning;
        private System.Windows.Forms.CheckBox OptionsCheckSoonFeature;
        private System.Windows.Forms.CheckBox OptionsCheckStartMaximized;
        private System.Windows.Forms.CheckBox OptionsCheckAutoBackups;
        private System.Windows.Forms.ToolTip OptionsToolTip;
    }
}
