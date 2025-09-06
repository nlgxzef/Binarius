namespace Binary.Interact
{
	partial class ThemeSelector
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemeSelector));
            this.ThemeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ThemeButtonOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ThemeColorsList = new System.Windows.Forms.ListView();
            this.Color = new System.Windows.Forms.ColumnHeader();
            this.ThemeVersionTextBox = new System.Windows.Forms.TextBox();
            this.ThemeAuthorTextBox = new System.Windows.Forms.TextBox();
            this.ThemeNameTextBox = new System.Windows.Forms.TextBox();
            this.ThemeListComboBox = new System.Windows.Forms.ComboBox();
            this.ThemeColorsLabel = new System.Windows.Forms.Label();
            this.ThemeVersionLabel = new System.Windows.Forms.Label();
            this.ThemeAuthorLabel = new System.Windows.Forms.Label();
            this.ThemeNameLabel = new System.Windows.Forms.Label();
            this.ThemeLabel = new System.Windows.Forms.Label();
            this.ThemeButtonSaveAs = new System.Windows.Forms.Button();
            this.ThemeButtonCancel = new System.Windows.Forms.Button();
            this.CheckDarkTheme = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ThemeButtonOK
            // 
            this.ThemeButtonOK.AutoSize = true;
            this.ThemeButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ThemeButtonOK.Location = new System.Drawing.Point(240, 392);
            this.ThemeButtonOK.Name = "ThemeButtonOK";
            this.ThemeButtonOK.Size = new System.Drawing.Size(112, 35);
            this.ThemeButtonOK.TabIndex = 3;
            this.ThemeButtonOK.Text = "OK";
            this.ThemeButtonOK.UseVisualStyleBackColor = false;
            this.ThemeButtonOK.Click += this.ThemeButtonOK_Click;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.CheckDarkTheme);
            this.panel1.Controls.Add(this.ThemeColorsList);
            this.panel1.Controls.Add(this.ThemeVersionTextBox);
            this.panel1.Controls.Add(this.ThemeAuthorTextBox);
            this.panel1.Controls.Add(this.ThemeNameTextBox);
            this.panel1.Controls.Add(this.ThemeListComboBox);
            this.panel1.Controls.Add(this.ThemeColorsLabel);
            this.panel1.Controls.Add(this.ThemeVersionLabel);
            this.panel1.Controls.Add(this.ThemeAuthorLabel);
            this.panel1.Controls.Add(this.ThemeNameLabel);
            this.panel1.Controls.Add(this.ThemeLabel);
            this.panel1.Location = new System.Drawing.Point(12, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 375);
            this.panel1.TabIndex = 4;
            // 
            // ThemeColorsList
            // 
            this.ThemeColorsList.AutoArrange = false;
            this.ThemeColorsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.Color });
            this.ThemeColorsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ThemeColorsList.Location = new System.Drawing.Point(6, 174);
            this.ThemeColorsList.MultiSelect = false;
            this.ThemeColorsList.Name = "ThemeColorsList";
            this.ThemeColorsList.ShowGroups = false;
            this.ThemeColorsList.Size = new System.Drawing.Size(452, 198);
            this.ThemeColorsList.TabIndex = 3;
            this.ThemeColorsList.UseCompatibleStateImageBehavior = false;
            this.ThemeColorsList.View = System.Windows.Forms.View.Details;
            this.ThemeColorsList.MouseDoubleClick += this.ThemeColorsList_MouseDoubleClick;
            // 
            // Color
            // 
            this.Color.Width = 420;
            // 
            // ThemeVersionTextBox
            // 
            this.ThemeVersionTextBox.Location = new System.Drawing.Point(91, 94);
            this.ThemeVersionTextBox.Name = "ThemeVersionTextBox";
            this.ThemeVersionTextBox.Size = new System.Drawing.Size(367, 23);
            this.ThemeVersionTextBox.TabIndex = 2;
            // 
            // ThemeAuthorTextBox
            // 
            this.ThemeAuthorTextBox.Location = new System.Drawing.Point(91, 65);
            this.ThemeAuthorTextBox.Name = "ThemeAuthorTextBox";
            this.ThemeAuthorTextBox.Size = new System.Drawing.Size(367, 23);
            this.ThemeAuthorTextBox.TabIndex = 2;
            // 
            // ThemeNameTextBox
            // 
            this.ThemeNameTextBox.Location = new System.Drawing.Point(91, 38);
            this.ThemeNameTextBox.Name = "ThemeNameTextBox";
            this.ThemeNameTextBox.Size = new System.Drawing.Size(367, 23);
            this.ThemeNameTextBox.TabIndex = 2;
            // 
            // ThemeListComboBox
            // 
            this.ThemeListComboBox.FormattingEnabled = true;
            this.ThemeListComboBox.Location = new System.Drawing.Point(52, 3);
            this.ThemeListComboBox.Name = "ThemeListComboBox";
            this.ThemeListComboBox.Size = new System.Drawing.Size(406, 23);
            this.ThemeListComboBox.TabIndex = 1;
            this.ThemeListComboBox.SelectedIndexChanged += this.ThemeListComboBox_SelectedIndexChanged;
            // 
            // ThemeColorsLabel
            // 
            this.ThemeColorsLabel.AutoSize = true;
            this.ThemeColorsLabel.Location = new System.Drawing.Point(3, 157);
            this.ThemeColorsLabel.Name = "ThemeColorsLabel";
            this.ThemeColorsLabel.Size = new System.Drawing.Size(80, 15);
            this.ThemeColorsLabel.TabIndex = 0;
            this.ThemeColorsLabel.Text = "Theme Colors";
            // 
            // ThemeVersionLabel
            // 
            this.ThemeVersionLabel.AutoSize = true;
            this.ThemeVersionLabel.Location = new System.Drawing.Point(3, 97);
            this.ThemeVersionLabel.Name = "ThemeVersionLabel";
            this.ThemeVersionLabel.Size = new System.Drawing.Size(48, 15);
            this.ThemeVersionLabel.TabIndex = 0;
            this.ThemeVersionLabel.Text = "Version:";
            // 
            // ThemeAuthorLabel
            // 
            this.ThemeAuthorLabel.AutoSize = true;
            this.ThemeAuthorLabel.Location = new System.Drawing.Point(4, 68);
            this.ThemeAuthorLabel.Name = "ThemeAuthorLabel";
            this.ThemeAuthorLabel.Size = new System.Drawing.Size(47, 15);
            this.ThemeAuthorLabel.TabIndex = 0;
            this.ThemeAuthorLabel.Text = "Author:";
            // 
            // ThemeNameLabel
            // 
            this.ThemeNameLabel.AutoSize = true;
            this.ThemeNameLabel.Location = new System.Drawing.Point(4, 41);
            this.ThemeNameLabel.Name = "ThemeNameLabel";
            this.ThemeNameLabel.Size = new System.Drawing.Size(81, 15);
            this.ThemeNameLabel.TabIndex = 0;
            this.ThemeNameLabel.Text = "Theme Name:";
            // 
            // ThemeLabel
            // 
            this.ThemeLabel.AutoSize = true;
            this.ThemeLabel.Location = new System.Drawing.Point(3, 6);
            this.ThemeLabel.Name = "ThemeLabel";
            this.ThemeLabel.Size = new System.Drawing.Size(46, 15);
            this.ThemeLabel.TabIndex = 0;
            this.ThemeLabel.Text = "Theme:";
            // 
            // ThemeButtonSaveAs
            // 
            this.ThemeButtonSaveAs.AutoSize = true;
            this.ThemeButtonSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ThemeButtonSaveAs.Location = new System.Drawing.Point(9, 392);
            this.ThemeButtonSaveAs.Name = "ThemeButtonSaveAs";
            this.ThemeButtonSaveAs.Size = new System.Drawing.Size(112, 35);
            this.ThemeButtonSaveAs.TabIndex = 3;
            this.ThemeButtonSaveAs.Text = "Save...";
            this.ThemeButtonSaveAs.UseVisualStyleBackColor = false;
            this.ThemeButtonSaveAs.Click += this.ThemeButtonSaveAs_Click;
            // 
            // ThemeButtonCancel
            // 
            this.ThemeButtonCancel.AutoSize = true;
            this.ThemeButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ThemeButtonCancel.Location = new System.Drawing.Point(358, 392);
            this.ThemeButtonCancel.Name = "ThemeButtonCancel";
            this.ThemeButtonCancel.Size = new System.Drawing.Size(112, 35);
            this.ThemeButtonCancel.TabIndex = 3;
            this.ThemeButtonCancel.Text = "Cancel";
            this.ThemeButtonCancel.UseVisualStyleBackColor = false;
            this.ThemeButtonCancel.Click += this.ThemeButtonCancel_Click;
            // 
            // CheckDarkTheme
            // 
            this.CheckDarkTheme.AutoSize = true;
            this.CheckDarkTheme.Location = new System.Drawing.Point(6, 128);
            this.CheckDarkTheme.Name = "CheckDarkTheme";
            this.CheckDarkTheme.Size = new System.Drawing.Size(50, 19);
            this.CheckDarkTheme.TabIndex = 4;
            this.CheckDarkTheme.Text = "Dark";
            this.ThemeToolTip.SetToolTip(this.CheckDarkTheme, "Makes the theme use dark image resources.");
            this.CheckDarkTheme.UseVisualStyleBackColor = true;
            this.CheckDarkTheme.CheckedChanged += this.CheckDarkTheme_CheckedChanged;
            // 
            // ThemeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(485, 439);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ThemeButtonSaveAs);
            this.Controls.Add(this.ThemeButtonCancel);
            this.Controls.Add(this.ThemeButtonOK);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.Name = "ThemeSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Theme Selector";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ToolTip ThemeToolTip;
		private System.Windows.Forms.Button ThemeButtonOK;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label ThemeLabel;
        private System.Windows.Forms.ComboBox ThemeListComboBox;
        private System.Windows.Forms.Label ThemeAuthorLabel;
        private System.Windows.Forms.Label ThemeNameLabel;
        private System.Windows.Forms.Label ThemeVersionLabel;
        private System.Windows.Forms.Button ThemeButtonSaveAs;
        private System.Windows.Forms.Button ThemeButtonCancel;
        private System.Windows.Forms.TextBox ThemeVersionTextBox;
        private System.Windows.Forms.TextBox ThemeAuthorTextBox;
        private System.Windows.Forms.TextBox ThemeNameTextBox;
        private System.Windows.Forms.Label ThemeColorsLabel;
        private System.Windows.Forms.ListView ThemeColorsList;
        private System.Windows.Forms.ColumnHeader Color;
        private System.Windows.Forms.CheckBox CheckDarkTheme;
    }
}
