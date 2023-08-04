namespace Binary.Prompt
{
    partial class Check
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
            CheckBoxSelection = new System.Windows.Forms.CheckBox();
            CheckButtonCancel = new System.Windows.Forms.Button();
            CheckButtonOK = new System.Windows.Forms.Button();
            LabelDescription = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // CheckBoxSelection
            // 
            CheckBoxSelection.Location = new System.Drawing.Point(12, 63);
            CheckBoxSelection.Name = "CheckBoxSelection";
            CheckBoxSelection.Size = new System.Drawing.Size(344, 30);
            CheckBoxSelection.TabIndex = 0;
            CheckBoxSelection.Text = "Custom Text";
            // 
            // CheckButtonCancel
            // 
            CheckButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CheckButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckButtonCancel.Location = new System.Drawing.Point(262, 99);
            CheckButtonCancel.Name = "CheckButtonCancel";
            CheckButtonCancel.Size = new System.Drawing.Size(96, 25);
            CheckButtonCancel.TabIndex = 1;
            CheckButtonCancel.Text = "Cancel";
            CheckButtonCancel.UseVisualStyleBackColor = true;
            CheckButtonCancel.Click += CheckButtonCancel_Click;
            // 
            // CheckButtonOK
            // 
            CheckButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            CheckButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CheckButtonOK.Location = new System.Drawing.Point(160, 99);
            CheckButtonOK.Name = "CheckButtonOK";
            CheckButtonOK.Size = new System.Drawing.Size(96, 25);
            CheckButtonOK.TabIndex = 1;
            CheckButtonOK.Text = "OK";
            CheckButtonOK.UseVisualStyleBackColor = true;
            CheckButtonOK.Click += CheckButtonOK_Click;
            // 
            // LabelDescription
            // 
            LabelDescription.Location = new System.Drawing.Point(12, 9);
            LabelDescription.MaximumSize = new System.Drawing.Size(344, 0);
            LabelDescription.Name = "LabelDescription";
            LabelDescription.Size = new System.Drawing.Size(344, 51);
            LabelDescription.TabIndex = 2;
            LabelDescription.Text = "Description label";
            // 
            // Check
            // 
            AcceptButton = CheckButtonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            CancelButton = CheckButtonCancel;
            ClientSize = new System.Drawing.Size(368, 138);
            Controls.Add(LabelDescription);
            Controls.Add(CheckButtonOK);
            Controls.Add(CheckButtonCancel);
            Controls.Add(CheckBoxSelection);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Check";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Prompt Selection";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.CheckBox CheckBoxSelection;
        private System.Windows.Forms.Button CheckButtonCancel;
        private System.Windows.Forms.Button CheckButtonOK;
        private System.Windows.Forms.Label LabelDescription;
    }
}