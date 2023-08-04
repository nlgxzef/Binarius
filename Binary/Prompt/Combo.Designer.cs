namespace Binary.Prompt
{
    partial class Combo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Combo));
            ComboButtonCancel = new System.Windows.Forms.Button();
            ComboButtonOK = new System.Windows.Forms.Button();
            ComboBoxSelection = new System.Windows.Forms.ComboBox();
            DescriptionLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // ComboButtonCancel
            // 
            ComboButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            ComboButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ComboButtonCancel.Location = new System.Drawing.Point(260, 78);
            ComboButtonCancel.Name = "ComboButtonCancel";
            ComboButtonCancel.Size = new System.Drawing.Size(96, 25);
            ComboButtonCancel.TabIndex = 1;
            ComboButtonCancel.Text = "Cancel";
            ComboButtonCancel.UseVisualStyleBackColor = true;
            ComboButtonCancel.Click += ComboButtonCancel_Click;
            // 
            // ComboButtonOK
            // 
            ComboButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            ComboButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ComboButtonOK.Location = new System.Drawing.Point(158, 78);
            ComboButtonOK.Name = "ComboButtonOK";
            ComboButtonOK.Size = new System.Drawing.Size(96, 25);
            ComboButtonOK.TabIndex = 1;
            ComboButtonOK.Text = "OK";
            ComboButtonOK.UseVisualStyleBackColor = true;
            ComboButtonOK.Click += ComboButtonOK_Click;
            // 
            // ComboBoxSelection
            // 
            ComboBoxSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ComboBoxSelection.FormattingEnabled = true;
            ComboBoxSelection.Location = new System.Drawing.Point(12, 47);
            ComboBoxSelection.Name = "ComboBoxSelection";
            ComboBoxSelection.Size = new System.Drawing.Size(344, 23);
            ComboBoxSelection.TabIndex = 2;
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new System.Drawing.Point(12, 19);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new System.Drawing.Size(73, 15);
            DescriptionLabel.TabIndex = 3;
            DescriptionLabel.Text = "Custom Text";
            // 
            // Combo
            // 
            AcceptButton = ComboButtonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            CancelButton = ComboButtonCancel;
            ClientSize = new System.Drawing.Size(368, 115);
            Controls.Add(DescriptionLabel);
            Controls.Add(ComboBoxSelection);
            Controls.Add(ComboButtonOK);
            Controls.Add(ComboButtonCancel);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Combo";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Prompt Selection";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button ComboButtonCancel;
        private System.Windows.Forms.Button ComboButtonOK;
        private System.Windows.Forms.ComboBox ComboBoxSelection;
        private System.Windows.Forms.Label DescriptionLabel;
    }
}