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
            this.OptionsCheckSafeSave = new System.Windows.Forms.CheckBox();
            this.OptionsLabelTemp = new System.Windows.Forms.Label();
            this.OptionsComboTemp = new System.Windows.Forms.ComboBox();
            this.OptionsTextTempPath = new System.Windows.Forms.TextBox();
            this.OptionsButtonTempBrowse = new System.Windows.Forms.Button();
            this.OptionsCheckKeepTemp = new System.Windows.Forms.CheckBox();
            this.OptionsCheckStartMaximized = new System.Windows.Forms.CheckBox();
            this.OptionsCheckAutoBackups = new System.Windows.Forms.CheckBox();
            this.OptionsLabelWatermark = new System.Windows.Forms.Label();
            this.OptionsTextWatermark = new System.Windows.Forms.TextBox();
            this.OptionsButtonCancel = new System.Windows.Forms.Button();
            this.OptionsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.OptionsCheckHideEmptyManagers = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionsButtonOK
            // 
            this.OptionsButtonOK.AutoSize = true;
            this.OptionsButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OptionsButtonOK.Location = new System.Drawing.Point(243, 275);
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
            this.panel1.Controls.Add(this.OptionsCheckHideEmptyManagers);
            this.panel1.Controls.Add(this.OptionsCheckDisableAdminWarning);
            this.panel1.Controls.Add(this.OptionsCheckSafeSave);
            this.panel1.Controls.Add(this.OptionsLabelTemp);
            this.panel1.Controls.Add(this.OptionsComboTemp);
            this.panel1.Controls.Add(this.OptionsTextTempPath);
            this.panel1.Controls.Add(this.OptionsButtonTempBrowse);
            this.panel1.Controls.Add(this.OptionsCheckKeepTemp);
            this.panel1.Controls.Add(this.OptionsCheckStartMaximized);
            this.panel1.Controls.Add(this.OptionsCheckAutoBackups);
            this.panel1.Controls.Add(this.OptionsLabelWatermark);
            this.panel1.Controls.Add(this.OptionsTextWatermark);
            this.panel1.Location = new System.Drawing.Point(12, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 258);
            this.panel1.TabIndex = 4;
            // 
            // OptionsCheckDisableAdminWarning
            // 
            this.OptionsCheckDisableAdminWarning.AutoSize = true;
            this.OptionsCheckDisableAdminWarning.Location = new System.Drawing.Point(3, 92);
            this.OptionsCheckDisableAdminWarning.Name = "OptionsCheckDisableAdminWarning";
            this.OptionsCheckDisableAdminWarning.Size = new System.Drawing.Size(220, 19);
            this.OptionsCheckDisableAdminWarning.TabIndex = 2;
            this.OptionsCheckDisableAdminWarning.Text = "Disable Administrator mode warning";
            this.OptionsToolTip.SetToolTip(this.OptionsCheckDisableAdminWarning, "Disables the warning that gets shown when Binarius is not ran in Administrator mode.");
            this.OptionsCheckDisableAdminWarning.UseVisualStyleBackColor = true;
            // 
            // OptionsCheckSafeSave
            //
            this.OptionsCheckSafeSave.AutoSize = true;
            this.OptionsCheckSafeSave.Location = new System.Drawing.Point(3, 142);
            this.OptionsCheckSafeSave.Name = "OptionsCheckSafeSave";
            this.OptionsCheckSafeSave.Size = new System.Drawing.Size(247, 19);
            this.OptionsCheckSafeSave.TabIndex = 6;
            this.OptionsCheckSafeSave.Text = "Save to temporary files first (experimental)";
            this.OptionsToolTip.SetToolTip(this.OptionsCheckSafeSave, "Writes to temporary files and replaces the actual files only if saving succeeds.\r\nIf saving fails, the actual files stay as they were.\r\nNeeds free space for a second copy of the files being saved.");
            this.OptionsCheckSafeSave.UseVisualStyleBackColor = true;
            this.OptionsCheckSafeSave.CheckedChanged += this.OptionsTempSetting_Changed;
            //
            // OptionsLabelTemp
            //
            this.OptionsLabelTemp.AutoSize = true;
            this.OptionsLabelTemp.Location = new System.Drawing.Point(20, 173);
            this.OptionsLabelTemp.Name = "OptionsLabelTemp";
            this.OptionsLabelTemp.Size = new System.Drawing.Size(101, 15);
            this.OptionsLabelTemp.TabIndex = 7;
            this.OptionsLabelTemp.Text = "Temporary folder";
            //
            // OptionsComboTemp
            //
            this.OptionsComboTemp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OptionsComboTemp.Items.AddRange(new object[] { "Game folder (_BinariusTemp)", "Launcher folder (_BinariusTemp)", "Custom folder" });
            this.OptionsComboTemp.Location = new System.Drawing.Point(136, 170);
            this.OptionsComboTemp.Name = "OptionsComboTemp";
            this.OptionsComboTemp.Size = new System.Drawing.Size(322, 23);
            this.OptionsComboTemp.TabIndex = 8;
            this.OptionsToolTip.SetToolTip(this.OptionsComboTemp, "Where the temporary files are written.\r\nA folder on the drive of the game is the fastest.\r\nFiles saved while no launcher is loaded use the folder they are saved in, unless a custom folder is chosen.");
            this.OptionsComboTemp.SelectedIndexChanged += this.OptionsTempSetting_Changed;
            //
            // OptionsTextTempPath
            //
            this.OptionsTextTempPath.Location = new System.Drawing.Point(136, 201);
            this.OptionsTextTempPath.Name = "OptionsTextTempPath";
            this.OptionsTextTempPath.Size = new System.Drawing.Size(232, 23);
            this.OptionsTextTempPath.TabIndex = 9;
            //
            // OptionsButtonTempBrowse
            //
            this.OptionsButtonTempBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OptionsButtonTempBrowse.Location = new System.Drawing.Point(374, 200);
            this.OptionsButtonTempBrowse.Name = "OptionsButtonTempBrowse";
            this.OptionsButtonTempBrowse.Size = new System.Drawing.Size(84, 25);
            this.OptionsButtonTempBrowse.TabIndex = 10;
            this.OptionsButtonTempBrowse.Text = "Browse...";
            this.OptionsButtonTempBrowse.UseVisualStyleBackColor = false;
            this.OptionsButtonTempBrowse.Click += this.OptionsButtonTempBrowse_Click;
            //
            // OptionsCheckKeepTemp
            //
            this.OptionsCheckKeepTemp.AutoSize = true;
            this.OptionsCheckKeepTemp.Location = new System.Drawing.Point(23, 232);
            this.OptionsCheckKeepTemp.Name = "OptionsCheckKeepTemp";
            this.OptionsCheckKeepTemp.Size = new System.Drawing.Size(230, 19);
            this.OptionsCheckKeepTemp.TabIndex = 11;
            this.OptionsCheckKeepTemp.Text = "Keep the temporary files after saving";
            this.OptionsToolTip.SetToolTip(this.OptionsCheckKeepTemp, "Keeps a folder for each save, with the files written in the same folders as in the game, and a list of where each one belongs.\r\nThey take up space until you delete them.");
            this.OptionsCheckKeepTemp.UseVisualStyleBackColor = true;
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
            this.OptionsButtonCancel.Location = new System.Drawing.Point(361, 275);
            this.OptionsButtonCancel.Name = "OptionsButtonCancel";
            this.OptionsButtonCancel.Size = new System.Drawing.Size(112, 35);
            this.OptionsButtonCancel.TabIndex = 3;
            this.OptionsButtonCancel.Text = "Cancel";
            this.OptionsButtonCancel.UseVisualStyleBackColor = false;
            this.OptionsButtonCancel.Click += this.OptionsButtonCancel_Click;
            // 
            // OptionsCheckHideEmptyManagers
            // 
            this.OptionsCheckHideEmptyManagers.AutoSize = true;
            this.OptionsCheckHideEmptyManagers.Location = new System.Drawing.Point(3, 117);
            this.OptionsCheckHideEmptyManagers.Name = "OptionsCheckHideEmptyManagers";
            this.OptionsCheckHideEmptyManagers.Size = new System.Drawing.Size(143, 19);
            this.OptionsCheckHideEmptyManagers.TabIndex = 2;
            this.OptionsCheckHideEmptyManagers.Text = "Hide empty managers";
            this.OptionsCheckHideEmptyManagers.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(485, 324);
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
        private System.Windows.Forms.CheckBox OptionsCheckSafeSave;
        private System.Windows.Forms.Label OptionsLabelTemp;
        private System.Windows.Forms.ComboBox OptionsComboTemp;
        private System.Windows.Forms.TextBox OptionsTextTempPath;
        private System.Windows.Forms.Button OptionsButtonTempBrowse;
        private System.Windows.Forms.CheckBox OptionsCheckKeepTemp;
        private System.Windows.Forms.CheckBox OptionsCheckStartMaximized;
        private System.Windows.Forms.CheckBox OptionsCheckAutoBackups;
        private System.Windows.Forms.ToolTip OptionsToolTip;
        private System.Windows.Forms.CheckBox OptionsCheckHideEmptyManagers;
    }
}
