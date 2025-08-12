namespace Binary.Prompt
{
    partial class Info
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Check));
            this.InfoLabel = new System.Windows.Forms.Label();
            this.InfoButtonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.Location = new System.Drawing.Point(12, 12);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(344, 44);
            this.InfoLabel.TabIndex = 0;
            this.InfoLabel.Text = "Custom Text";
            // 
            // ComboButtonOK
            // 
            this.InfoButtonOK.AutoSize = true;
            this.InfoButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.InfoButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoButtonOK.Location = new System.Drawing.Point(162, 62);
            this.InfoButtonOK.Name = "ComboButtonOK";
            this.InfoButtonOK.Size = new System.Drawing.Size(94, 27);
            this.InfoButtonOK.TabIndex = 1;
            this.InfoButtonOK.Text = "OK";
            this.InfoButtonOK.UseVisualStyleBackColor = true;
            this.InfoButtonOK.Click += new System.EventHandler(this.InfoButtonOK_Click);
            // 
            // Check
            // 
            this.AcceptButton = this.InfoButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(368, 99);
            this.Controls.Add(this.InfoButtonOK);
            this.Controls.Add(this.InfoLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Info";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Information";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button InfoButtonOK;
    }
}
