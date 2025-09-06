namespace Binary.Interact
{
	partial class LanMakerFileSelector
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(LanMakerFileSelector));
            this.LanMakerFileSelectorButtonOK = new System.Windows.Forms.Button();
            this.LanMakerFileSelectorButtonCancel = new System.Windows.Forms.Button();
            this.FileListTreeView = new System.Windows.Forms.CustomTreeView();
            this.EditorImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // LanMakerFileSelectorButtonOK
            // 
            this.LanMakerFileSelectorButtonOK.AutoSize = true;
            this.LanMakerFileSelectorButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LanMakerFileSelectorButtonOK.Location = new System.Drawing.Point(252, 373);
            this.LanMakerFileSelectorButtonOK.Name = "LanMakerFileSelectorButtonOK";
            this.LanMakerFileSelectorButtonOK.Size = new System.Drawing.Size(110, 27);
            this.LanMakerFileSelectorButtonOK.TabIndex = 6;
            this.LanMakerFileSelectorButtonOK.Text = "OK";
            this.LanMakerFileSelectorButtonOK.UseVisualStyleBackColor = true;
            this.LanMakerFileSelectorButtonOK.Click += this.LanMakerFileSelectorButtonOK_Click;
            // 
            // LanMakerFileSelectorButtonCancel
            // 
            this.LanMakerFileSelectorButtonCancel.AutoSize = true;
            this.LanMakerFileSelectorButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LanMakerFileSelectorButtonCancel.Location = new System.Drawing.Point(368, 373);
            this.LanMakerFileSelectorButtonCancel.Name = "LanMakerFileSelectorButtonCancel";
            this.LanMakerFileSelectorButtonCancel.Size = new System.Drawing.Size(110, 27);
            this.LanMakerFileSelectorButtonCancel.TabIndex = 6;
            this.LanMakerFileSelectorButtonCancel.Text = "Cancel";
            this.LanMakerFileSelectorButtonCancel.UseVisualStyleBackColor = true;
            this.LanMakerFileSelectorButtonCancel.Click += this.LanMakerFileSelectorButtonCancel_Click;
            // 
            // FileListTreeView
            // 
            this.FileListTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.FileListTreeView.ImageIndex = 0;
            this.FileListTreeView.ImageList = this.EditorImageList;
            this.FileListTreeView.Location = new System.Drawing.Point(12, 12);
            this.FileListTreeView.Name = "FileListTreeView";
            this.FileListTreeView.SelectedImageIndex = 0;
            this.FileListTreeView.Size = new System.Drawing.Size(466, 355);
            this.FileListTreeView.TabIndex = 10;
            // 
            // EditorImageList
            // 
            this.EditorImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.EditorImageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("EditorImageList.ImageStream");
            this.EditorImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.EditorImageList.Images.SetKeyName(0, "Collection_Black.png");
            this.EditorImageList.Images.SetKeyName(1, "Collection_White.png");
            this.EditorImageList.Images.SetKeyName(2, "Selection_Black.png");
            this.EditorImageList.Images.SetKeyName(3, "Selection_White.png");
            // 
            // LanMakerFileSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(490, 412);
            this.Controls.Add(this.FileListTreeView);
            this.Controls.Add(this.LanMakerFileSelectorButtonCancel);
            this.Controls.Add(this.LanMakerFileSelectorButtonOK);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.Name = "LanMakerFileSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Files";
            this.Load += this.LanMakerFileSelector_Load;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox LanMakerFileSelectorUsage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox LanMakerFileSelectorGame;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button LanMakerFileSelectorButtonOK;
        private System.Windows.Forms.Button LanMakerFileSelectorButtonCancel;
        private System.Windows.Forms.CustomTreeView FileListTreeView;
        private System.Windows.Forms.ImageList EditorImageList;
    }
}
