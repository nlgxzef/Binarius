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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Raider));
            this.ChooseSearchMode = new System.Windows.Forms.ComboBox();
            this.BinHashInput = new System.Windows.Forms.TextBox();
            this.BinFileInput = new System.Windows.Forms.TextBox();
            this.StringGuessed = new System.Windows.Forms.TextBox();
            this.CopyBinHash = new System.Windows.Forms.Button();
            this.CopyBinFile = new System.Windows.Forms.Button();
            this.CopyString = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ChooseSearchMode
            // 
            this.ChooseSearchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChooseSearchMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChooseSearchMode.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ChooseSearchMode.FormattingEnabled = true;
            this.ChooseSearchMode.Items.AddRange(new object[] { "Use Bin Memory Hash search", "Use Bin File Hash search" });
            this.ChooseSearchMode.Location = new System.Drawing.Point(114, 15);
            this.ChooseSearchMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ChooseSearchMode.Name = "ChooseSearchMode";
            this.ChooseSearchMode.Size = new System.Drawing.Size(272, 22);
            this.ChooseSearchMode.TabIndex = 0;
            this.ChooseSearchMode.SelectedIndexChanged += this.ChooseSearchMode_SelectedIndexChanged;
            // 
            // BinHashInput
            // 
            this.BinHashInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BinHashInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BinHashInput.Location = new System.Drawing.Point(114, 50);
            this.BinHashInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BinHashInput.Name = "BinHashInput";
            this.BinHashInput.ReadOnly = true;
            this.BinHashInput.Size = new System.Drawing.Size(272, 22);
            this.BinHashInput.TabIndex = 1;
            this.BinHashInput.TextChanged += this.BinHashInput_TextChanged;
            // 
            // BinFileInput
            // 
            this.BinFileInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BinFileInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BinFileInput.Location = new System.Drawing.Point(114, 85);
            this.BinFileInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BinFileInput.Name = "BinFileInput";
            this.BinFileInput.ReadOnly = true;
            this.BinFileInput.Size = new System.Drawing.Size(272, 22);
            this.BinFileInput.TabIndex = 2;
            this.BinFileInput.TextChanged += this.BinFileInput_TextChanged;
            // 
            // StringGuessed
            // 
            this.StringGuessed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StringGuessed.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StringGuessed.Location = new System.Drawing.Point(114, 120);
            this.StringGuessed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.StringGuessed.Name = "StringGuessed";
            this.StringGuessed.ReadOnly = true;
            this.StringGuessed.Size = new System.Drawing.Size(272, 22);
            this.StringGuessed.TabIndex = 3;
            // 
            // CopyBinHash
            // 
            this.CopyBinHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyBinHash.Location = new System.Drawing.Point(400, 49);
            this.CopyBinHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyBinHash.Name = "CopyBinHash";
            this.CopyBinHash.Size = new System.Drawing.Size(96, 25);
            this.CopyBinHash.TabIndex = 4;
            this.CopyBinHash.Text = "Copy";
            this.CopyBinHash.UseVisualStyleBackColor = false;
            this.CopyBinHash.Click += this.CopyBinHash_Click;
            // 
            // CopyBinFile
            // 
            this.CopyBinFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyBinFile.Location = new System.Drawing.Point(400, 84);
            this.CopyBinFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyBinFile.Name = "CopyBinFile";
            this.CopyBinFile.Size = new System.Drawing.Size(96, 25);
            this.CopyBinFile.TabIndex = 5;
            this.CopyBinFile.Text = "Copy";
            this.CopyBinFile.UseVisualStyleBackColor = false;
            this.CopyBinFile.Click += this.CopyBinFile_Click;
            // 
            // CopyString
            // 
            this.CopyString.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyString.Location = new System.Drawing.Point(400, 119);
            this.CopyString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyString.Name = "CopyString";
            this.CopyString.Size = new System.Drawing.Size(96, 25);
            this.CopyString.TabIndex = 6;
            this.CopyString.Text = "Copy";
            this.CopyString.UseVisualStyleBackColor = false;
            this.CopyString.Click += this.CopyString_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(59, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Lookup";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(34, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Bin Memory";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(61, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Bin File";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(68, 123);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "String";
            // 
            // Raider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(534, 156);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CopyString);
            this.Controls.Add(this.CopyBinFile);
            this.Controls.Add(this.CopyBinHash);
            this.Controls.Add(this.StringGuessed);
            this.Controls.Add(this.BinFileInput);
            this.Controls.Add(this.BinHashInput);
            this.Controls.Add(this.ChooseSearchMode);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "Raider";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NFS-Raider by MaxHwoy";
            this.ResumeLayout(false);
            this.PerformLayout();
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