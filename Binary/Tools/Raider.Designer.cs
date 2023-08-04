namespace Binary.Tools
{
    partial class Raider
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Raider));
            ChooseSearchMode = new System.Windows.Forms.ComboBox();
            BinHashInput = new System.Windows.Forms.TextBox();
            BinFileInput = new System.Windows.Forms.TextBox();
            StringGuessed = new System.Windows.Forms.TextBox();
            CopyBinHash = new System.Windows.Forms.Button();
            CopyBinFile = new System.Windows.Forms.Button();
            CopyString = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // ChooseSearchMode
            // 
            ChooseSearchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ChooseSearchMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ChooseSearchMode.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ChooseSearchMode.FormattingEnabled = true;
            ChooseSearchMode.Items.AddRange(new object[] { "Use Bin Memory Hash search", "Use Bin File Hash search" });
            ChooseSearchMode.Location = new System.Drawing.Point(114, 15);
            ChooseSearchMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ChooseSearchMode.Name = "ChooseSearchMode";
            ChooseSearchMode.Size = new System.Drawing.Size(272, 22);
            ChooseSearchMode.TabIndex = 0;
            ChooseSearchMode.SelectedIndexChanged += ChooseSearchMode_SelectedIndexChanged;
            // 
            // BinHashInput
            // 
            BinHashInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            BinHashInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BinHashInput.Location = new System.Drawing.Point(114, 50);
            BinHashInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BinHashInput.Name = "BinHashInput";
            BinHashInput.ReadOnly = true;
            BinHashInput.Size = new System.Drawing.Size(272, 22);
            BinHashInput.TabIndex = 1;
            BinHashInput.TextChanged += BinHashInput_TextChanged;
            // 
            // BinFileInput
            // 
            BinFileInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            BinFileInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BinFileInput.Location = new System.Drawing.Point(114, 85);
            BinFileInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BinFileInput.Name = "BinFileInput";
            BinFileInput.ReadOnly = true;
            BinFileInput.Size = new System.Drawing.Size(272, 22);
            BinFileInput.TabIndex = 2;
            BinFileInput.TextChanged += BinFileInput_TextChanged;
            // 
            // StringGuessed
            // 
            StringGuessed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            StringGuessed.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            StringGuessed.Location = new System.Drawing.Point(114, 120);
            StringGuessed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StringGuessed.Name = "StringGuessed";
            StringGuessed.ReadOnly = true;
            StringGuessed.Size = new System.Drawing.Size(272, 22);
            StringGuessed.TabIndex = 3;
            // 
            // CopyBinHash
            // 
            CopyBinHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyBinHash.Location = new System.Drawing.Point(400, 49);
            CopyBinHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyBinHash.Name = "CopyBinHash";
            CopyBinHash.Size = new System.Drawing.Size(96, 25);
            CopyBinHash.TabIndex = 4;
            CopyBinHash.Text = "Copy";
            CopyBinHash.UseVisualStyleBackColor = false;
            CopyBinHash.Click += CopyBinHash_Click;
            // 
            // CopyBinFile
            // 
            CopyBinFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyBinFile.Location = new System.Drawing.Point(400, 84);
            CopyBinFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyBinFile.Name = "CopyBinFile";
            CopyBinFile.Size = new System.Drawing.Size(96, 25);
            CopyBinFile.TabIndex = 5;
            CopyBinFile.Text = "Copy";
            CopyBinFile.UseVisualStyleBackColor = false;
            CopyBinFile.Click += CopyBinFile_Click;
            // 
            // CopyString
            // 
            CopyString.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyString.Location = new System.Drawing.Point(400, 119);
            CopyString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyString.Name = "CopyString";
            CopyString.Size = new System.Drawing.Size(96, 25);
            CopyString.TabIndex = 6;
            CopyString.Text = "Copy";
            CopyString.UseVisualStyleBackColor = false;
            CopyString.Click += CopyString_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(59, 18);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(47, 15);
            label1.TabIndex = 7;
            label1.Text = "Lookup";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(34, 53);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 15);
            label2.TabIndex = 8;
            label2.Text = "Bin Memory";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(61, 88);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(45, 15);
            label3.TabIndex = 9;
            label3.Text = "Bin File";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(68, 123);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(38, 15);
            label4.TabIndex = 10;
            label4.Text = "String";
            // 
            // Raider
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(534, 156);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(CopyString);
            Controls.Add(CopyBinFile);
            Controls.Add(CopyBinHash);
            Controls.Add(StringGuessed);
            Controls.Add(BinFileInput);
            Controls.Add(BinHashInput);
            Controls.Add(ChooseSearchMode);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Raider";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "NFS-Raider by MaxHwoy";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox ChooseSearchMode;
        private System.Windows.Forms.TextBox BinHashInput;
        private System.Windows.Forms.TextBox BinFileInput;
        private System.Windows.Forms.TextBox StringGuessed;
        private System.Windows.Forms.Button CopyBinHash;
        private System.Windows.Forms.Button CopyBinFile;
        private System.Windows.Forms.Button CopyString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}