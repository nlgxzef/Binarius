namespace Binary.Prompt
{
    partial class CheckError
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckError));
            this.CheckBoxSelection = new System.Windows.Forms.CheckBox();
            this.CheckButtonCancel = new System.Windows.Forms.Button();
            this.CheckButtonOK = new System.Windows.Forms.Button();
            this.LabelDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CheckBoxSelection
            // 
            this.CheckBoxSelection.Location = new System.Drawing.Point(12, 63);
            this.CheckBoxSelection.Name = "CheckBoxSelection";
            this.CheckBoxSelection.Size = new System.Drawing.Size(344, 30);
            this.CheckBoxSelection.TabIndex = 0;
            this.CheckBoxSelection.Text = "Custom Text";
            // 
            // CheckButtonCancel
            // 
            this.CheckButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CheckButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckButtonCancel.Location = new System.Drawing.Point(262, 99);
            this.CheckButtonCancel.Name = "CheckButtonCancel";
            this.CheckButtonCancel.Size = new System.Drawing.Size(96, 25);
            this.CheckButtonCancel.TabIndex = 1;
            this.CheckButtonCancel.Text = "Cancel";
            this.CheckButtonCancel.UseVisualStyleBackColor = true;
            this.CheckButtonCancel.Click += this.CheckButtonCancel_Click;
            // 
            // CheckButtonOK
            // 
            this.CheckButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CheckButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckButtonOK.Location = new System.Drawing.Point(160, 99);
            this.CheckButtonOK.Name = "CheckButtonOK";
            this.CheckButtonOK.Size = new System.Drawing.Size(96, 25);
            this.CheckButtonOK.TabIndex = 1;
            this.CheckButtonOK.Text = "OK";
            this.CheckButtonOK.UseVisualStyleBackColor = true;
            this.CheckButtonOK.Click += this.CheckButtonOK_Click;
            // 
            // LabelDescription
            // 
            this.LabelDescription.Location = new System.Drawing.Point(12, 9);
            this.LabelDescription.MaximumSize = new System.Drawing.Size(344, 0);
            this.LabelDescription.Name = "LabelDescription";
            this.LabelDescription.Size = new System.Drawing.Size(344, 51);
            this.LabelDescription.TabIndex = 2;
            this.LabelDescription.Text = "Description label";
            // 
            // CheckError
            // 
            this.AcceptButton = this.CheckButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.CheckButtonCancel;
            this.ClientSize = new System.Drawing.Size(368, 138);
            this.Controls.Add(this.LabelDescription);
            this.Controls.Add(this.CheckButtonOK);
            this.Controls.Add(this.CheckButtonCancel);
            this.Controls.Add(this.CheckBoxSelection);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.Name = "CheckError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prompt Selection";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.CheckBox CheckBoxSelection;
        private System.Windows.Forms.Button CheckButtonCancel;
        private System.Windows.Forms.Button CheckButtonOK;
        private System.Windows.Forms.Label LabelDescription;
    }
}
