namespace Binary.Interact
{
    partial class StringCreator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StringCreator));
            StringTextBoxText = new System.Windows.Forms.TextBox();
            StringTextBoxLabel = new System.Windows.Forms.TextBox();
            StringTextBoxKey = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            StringCheckBoxCustom = new System.Windows.Forms.CheckBox();
            StringCheckBoxReversed = new System.Windows.Forms.CheckBox();
            StringButtonCancel = new System.Windows.Forms.Button();
            StringButtonOK = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // StringTextBoxText
            // 
            StringTextBoxText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            StringTextBoxText.Location = new System.Drawing.Point(12, 70);
            StringTextBoxText.Multiline = true;
            StringTextBoxText.Name = "StringTextBoxText";
            StringTextBoxText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            StringTextBoxText.Size = new System.Drawing.Size(501, 132);
            StringTextBoxText.TabIndex = 0;
            // 
            // StringTextBoxLabel
            // 
            StringTextBoxLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            StringTextBoxLabel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            StringTextBoxLabel.Location = new System.Drawing.Point(53, 41);
            StringTextBoxLabel.Name = "StringTextBoxLabel";
            StringTextBoxLabel.Size = new System.Drawing.Size(294, 22);
            StringTextBoxLabel.TabIndex = 1;
            StringTextBoxLabel.TextChanged += StringTextBoxLabel_TextChanged;
            // 
            // StringTextBoxKey
            // 
            StringTextBoxKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            StringTextBoxKey.Enabled = false;
            StringTextBoxKey.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            StringTextBoxKey.Location = new System.Drawing.Point(53, 12);
            StringTextBoxKey.Name = "StringTextBoxKey";
            StringTextBoxKey.Size = new System.Drawing.Size(294, 22);
            StringTextBoxKey.TabIndex = 1;
            StringTextBoxKey.Validating += StringTextBoxKey_Validating;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(21, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(26, 15);
            label1.TabIndex = 2;
            label1.Text = "Key";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 44);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(35, 15);
            label2.TabIndex = 2;
            label2.Text = "Label";
            // 
            // StringCheckBoxCustom
            // 
            StringCheckBoxCustom.Location = new System.Drawing.Point(353, 12);
            StringCheckBoxCustom.Name = "StringCheckBoxCustom";
            StringCheckBoxCustom.Size = new System.Drawing.Size(151, 23);
            StringCheckBoxCustom.TabIndex = 3;
            StringCheckBoxCustom.Text = "Enable Custom Key";
            StringCheckBoxCustom.CheckedChanged += StringCheckBoxCustom_CheckedChanged;
            // 
            // StringCheckBoxReversed
            // 
            StringCheckBoxReversed.Location = new System.Drawing.Point(353, 41);
            StringCheckBoxReversed.Name = "StringCheckBoxReversed";
            StringCheckBoxReversed.Size = new System.Drawing.Size(151, 23);
            StringCheckBoxReversed.TabIndex = 3;
            StringCheckBoxReversed.Text = "Show Reversed Key";
            StringCheckBoxReversed.CheckedChanged += StringCheckBoxReversed_CheckedChanged;
            // 
            // StringButtonCancel
            // 
            StringButtonCancel.AutoSize = true;
            StringButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            StringButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            StringButtonCancel.Location = new System.Drawing.Point(413, 210);
            StringButtonCancel.Name = "StringButtonCancel";
            StringButtonCancel.Size = new System.Drawing.Size(96, 27);
            StringButtonCancel.TabIndex = 4;
            StringButtonCancel.Text = "Cancel";
            StringButtonCancel.UseVisualStyleBackColor = true;
            StringButtonCancel.Click += StringButtonCancel_Click;
            // 
            // StringButtonOK
            // 
            StringButtonOK.AutoSize = true;
            StringButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            StringButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            StringButtonOK.Location = new System.Drawing.Point(307, 210);
            StringButtonOK.Name = "StringButtonOK";
            StringButtonOK.Size = new System.Drawing.Size(96, 27);
            StringButtonOK.TabIndex = 4;
            StringButtonOK.Text = "OK";
            StringButtonOK.UseVisualStyleBackColor = true;
            StringButtonOK.Click += StringButtonOK_Click;
            // 
            // StringCreator
            // 
            AcceptButton = StringButtonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            CancelButton = StringButtonCancel;
            ClientSize = new System.Drawing.Size(529, 244);
            Controls.Add(StringButtonOK);
            Controls.Add(StringButtonCancel);
            Controls.Add(StringCheckBoxReversed);
            Controls.Add(StringCheckBoxCustom);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(StringTextBoxKey);
            Controls.Add(StringTextBoxLabel);
            Controls.Add(StringTextBoxText);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "StringCreator";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "StringCreator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox StringTextBoxText;
        private System.Windows.Forms.TextBox StringTextBoxLabel;
        private System.Windows.Forms.TextBox StringTextBoxKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox StringCheckBoxCustom;
        private System.Windows.Forms.CheckBox StringCheckBoxReversed;
        private System.Windows.Forms.Button StringButtonCancel;
        private System.Windows.Forms.Button StringButtonOK;
    }
}