namespace Binary.Tools
{
    partial class Hasher
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Hasher));
            this.StringTextbox = new System.Windows.Forms.TextBox();
            this.BinHashTextbox = new System.Windows.Forms.TextBox();
            this.BinFileTextbox = new System.Windows.Forms.TextBox();
            this.VltHashTextbox = new System.Windows.Forms.TextBox();
            this.VltFileTextbox = new System.Windows.Forms.TextBox();
            this.CopyString = new System.Windows.Forms.Button();
            this.CopyBinHash = new System.Windows.Forms.Button();
            this.CopyBinFile = new System.Windows.Forms.Button();
            this.CopyVltHash = new System.Windows.Forms.Button();
            this.CopyVltFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StringTextbox
            // 
            this.StringTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StringTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StringTextbox.Location = new System.Drawing.Point(114, 15);
            this.StringTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.StringTextbox.Name = "StringTextbox";
            this.StringTextbox.Size = new System.Drawing.Size(272, 22);
            this.StringTextbox.TabIndex = 0;
            this.StringTextbox.TextChanged += this.StringTextbox_TextChanged;
            // 
            // BinHashTextbox
            // 
            this.BinHashTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BinHashTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BinHashTextbox.Location = new System.Drawing.Point(114, 50);
            this.BinHashTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BinHashTextbox.Name = "BinHashTextbox";
            this.BinHashTextbox.ReadOnly = true;
            this.BinHashTextbox.Size = new System.Drawing.Size(272, 22);
            this.BinHashTextbox.TabIndex = 1;
            // 
            // BinFileTextbox
            // 
            this.BinFileTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BinFileTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BinFileTextbox.Location = new System.Drawing.Point(114, 85);
            this.BinFileTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BinFileTextbox.Name = "BinFileTextbox";
            this.BinFileTextbox.ReadOnly = true;
            this.BinFileTextbox.Size = new System.Drawing.Size(272, 22);
            this.BinFileTextbox.TabIndex = 2;
            // 
            // VltHashTextbox
            // 
            this.VltHashTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VltHashTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VltHashTextbox.Location = new System.Drawing.Point(114, 120);
            this.VltHashTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.VltHashTextbox.Name = "VltHashTextbox";
            this.VltHashTextbox.ReadOnly = true;
            this.VltHashTextbox.Size = new System.Drawing.Size(272, 22);
            this.VltHashTextbox.TabIndex = 3;
            // 
            // VltFileTextbox
            // 
            this.VltFileTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VltFileTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.VltFileTextbox.Location = new System.Drawing.Point(114, 155);
            this.VltFileTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.VltFileTextbox.Name = "VltFileTextbox";
            this.VltFileTextbox.ReadOnly = true;
            this.VltFileTextbox.Size = new System.Drawing.Size(272, 22);
            this.VltFileTextbox.TabIndex = 4;
            // 
            // CopyString
            // 
            this.CopyString.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyString.Location = new System.Drawing.Point(400, 14);
            this.CopyString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyString.Name = "CopyString";
            this.CopyString.Size = new System.Drawing.Size(96, 25);
            this.CopyString.TabIndex = 5;
            this.CopyString.Text = "Copy";
            this.CopyString.UseVisualStyleBackColor = false;
            this.CopyString.Click += this.CopyString_Click;
            // 
            // CopyBinHash
            // 
            this.CopyBinHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyBinHash.Location = new System.Drawing.Point(400, 49);
            this.CopyBinHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyBinHash.Name = "CopyBinHash";
            this.CopyBinHash.Size = new System.Drawing.Size(96, 25);
            this.CopyBinHash.TabIndex = 6;
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
            this.CopyBinFile.TabIndex = 7;
            this.CopyBinFile.Text = "Copy";
            this.CopyBinFile.UseVisualStyleBackColor = false;
            this.CopyBinFile.Click += this.CopyBinFile_Click;
            // 
            // CopyVltHash
            // 
            this.CopyVltHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyVltHash.Location = new System.Drawing.Point(400, 119);
            this.CopyVltHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyVltHash.Name = "CopyVltHash";
            this.CopyVltHash.Size = new System.Drawing.Size(96, 25);
            this.CopyVltHash.TabIndex = 8;
            this.CopyVltHash.Text = "Copy";
            this.CopyVltHash.UseVisualStyleBackColor = false;
            this.CopyVltHash.Click += this.CopyVltHash_Click;
            // 
            // CopyVltFile
            // 
            this.CopyVltFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyVltFile.Location = new System.Drawing.Point(400, 154);
            this.CopyVltFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CopyVltFile.Name = "CopyVltFile";
            this.CopyVltFile.Size = new System.Drawing.Size(96, 25);
            this.CopyVltFile.TabIndex = 9;
            this.CopyVltFile.Text = "Copy";
            this.CopyVltFile.UseVisualStyleBackColor = false;
            this.CopyVltFile.Click += this.CopyVltFile_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(71, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Input";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(34, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 11;
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
            this.label3.TabIndex = 12;
            this.label3.Text = "Bin File";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(37, 123);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Vlt Memory";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(64, 158);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Vlt File";
            // 
            // Hasher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(534, 191);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CopyVltFile);
            this.Controls.Add(this.CopyVltHash);
            this.Controls.Add(this.CopyBinFile);
            this.Controls.Add(this.CopyBinHash);
            this.Controls.Add(this.CopyString);
            this.Controls.Add(this.VltFileTextbox);
            this.Controls.Add(this.VltHashTextbox);
            this.Controls.Add(this.BinFileTextbox);
            this.Controls.Add(this.BinHashTextbox);
            this.Controls.Add(this.StringTextbox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "Hasher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NFS-Hasher by MaxHwoy";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox StringTextbox;
        private System.Windows.Forms.TextBox BinHashTextbox;
        private System.Windows.Forms.TextBox BinFileTextbox;
        private System.Windows.Forms.TextBox VltHashTextbox;
        private System.Windows.Forms.TextBox VltFileTextbox;
        private System.Windows.Forms.Button CopyString;
        private System.Windows.Forms.Button CopyBinHash;
        private System.Windows.Forms.Button CopyBinFile;
        private System.Windows.Forms.Button CopyVltHash;
        private System.Windows.Forms.Button CopyVltFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
