namespace Binary.Prompt
{
    partial class Input
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Input));
            InputTextBox = new System.Windows.Forms.TextBox();
            InputLabel = new System.Windows.Forms.Label();
            InputButtonOK = new System.Windows.Forms.Button();
            InputButtonCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // InputTextBox
            // 
            InputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            InputTextBox.Location = new System.Drawing.Point(12, 32);
            InputTextBox.Name = "InputTextBox";
            InputTextBox.Size = new System.Drawing.Size(325, 23);
            InputTextBox.TabIndex = 0;
            // 
            // InputLabel
            // 
            InputLabel.AutoSize = true;
            InputLabel.Location = new System.Drawing.Point(12, 14);
            InputLabel.Name = "InputLabel";
            InputLabel.Size = new System.Drawing.Size(66, 15);
            InputLabel.TabIndex = 1;
            InputLabel.Text = "Input value";
            // 
            // InputButtonOK
            // 
            InputButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            InputButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            InputButtonOK.Location = new System.Drawing.Point(139, 61);
            InputButtonOK.Name = "InputButtonOK";
            InputButtonOK.Size = new System.Drawing.Size(96, 25);
            InputButtonOK.TabIndex = 2;
            InputButtonOK.Text = "OK";
            InputButtonOK.UseVisualStyleBackColor = true;
            InputButtonOK.Click += InputButtonOK_Click;
            // 
            // InputButtonCancel
            // 
            InputButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            InputButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            InputButtonCancel.Location = new System.Drawing.Point(241, 61);
            InputButtonCancel.Name = "InputButtonCancel";
            InputButtonCancel.Size = new System.Drawing.Size(96, 25);
            InputButtonCancel.TabIndex = 2;
            InputButtonCancel.Text = "Cancel";
            InputButtonCancel.UseVisualStyleBackColor = true;
            InputButtonCancel.Click += InputButtonCancel_Click;
            // 
            // Input
            // 
            AcceptButton = InputButtonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            CancelButton = InputButtonCancel;
            ClientSize = new System.Drawing.Size(350, 97);
            Controls.Add(InputButtonCancel);
            Controls.Add(InputButtonOK);
            Controls.Add(InputLabel);
            Controls.Add(InputTextBox);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Input";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Editor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Button InputButtonOK;
        private System.Windows.Forms.Button InputButtonCancel;
    }
}