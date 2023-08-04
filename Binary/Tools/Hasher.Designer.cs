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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hasher));
            StringTextbox = new System.Windows.Forms.TextBox();
            BinHashTextbox = new System.Windows.Forms.TextBox();
            BinFileTextbox = new System.Windows.Forms.TextBox();
            VltHashTextbox = new System.Windows.Forms.TextBox();
            VltFileTextbox = new System.Windows.Forms.TextBox();
            CopyString = new System.Windows.Forms.Button();
            CopyBinHash = new System.Windows.Forms.Button();
            CopyBinFile = new System.Windows.Forms.Button();
            CopyVltHash = new System.Windows.Forms.Button();
            CopyVltFile = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // StringTextbox
            // 
            StringTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            StringTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            StringTextbox.Location = new System.Drawing.Point(114, 15);
            StringTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StringTextbox.Name = "StringTextbox";
            StringTextbox.Size = new System.Drawing.Size(272, 22);
            StringTextbox.TabIndex = 0;
            StringTextbox.TextChanged += StringTextbox_TextChanged;
            // 
            // BinHashTextbox
            // 
            BinHashTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            BinHashTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BinHashTextbox.Location = new System.Drawing.Point(114, 50);
            BinHashTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BinHashTextbox.Name = "BinHashTextbox";
            BinHashTextbox.ReadOnly = true;
            BinHashTextbox.Size = new System.Drawing.Size(272, 22);
            BinHashTextbox.TabIndex = 1;
            // 
            // BinFileTextbox
            // 
            BinFileTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            BinFileTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BinFileTextbox.Location = new System.Drawing.Point(114, 85);
            BinFileTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BinFileTextbox.Name = "BinFileTextbox";
            BinFileTextbox.ReadOnly = true;
            BinFileTextbox.Size = new System.Drawing.Size(272, 22);
            BinFileTextbox.TabIndex = 2;
            // 
            // VltHashTextbox
            // 
            VltHashTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            VltHashTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            VltHashTextbox.Location = new System.Drawing.Point(114, 120);
            VltHashTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            VltHashTextbox.Name = "VltHashTextbox";
            VltHashTextbox.ReadOnly = true;
            VltHashTextbox.Size = new System.Drawing.Size(272, 22);
            VltHashTextbox.TabIndex = 3;
            // 
            // VltFileTextbox
            // 
            VltFileTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            VltFileTextbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            VltFileTextbox.Location = new System.Drawing.Point(114, 155);
            VltFileTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            VltFileTextbox.Name = "VltFileTextbox";
            VltFileTextbox.ReadOnly = true;
            VltFileTextbox.Size = new System.Drawing.Size(272, 22);
            VltFileTextbox.TabIndex = 4;
            // 
            // CopyString
            // 
            CopyString.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyString.Location = new System.Drawing.Point(400, 14);
            CopyString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyString.Name = "CopyString";
            CopyString.Size = new System.Drawing.Size(96, 25);
            CopyString.TabIndex = 5;
            CopyString.Text = "Copy";
            CopyString.UseVisualStyleBackColor = false;
            CopyString.Click += CopyString_Click;
            // 
            // CopyBinHash
            // 
            CopyBinHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyBinHash.Location = new System.Drawing.Point(400, 49);
            CopyBinHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyBinHash.Name = "CopyBinHash";
            CopyBinHash.Size = new System.Drawing.Size(96, 25);
            CopyBinHash.TabIndex = 6;
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
            CopyBinFile.TabIndex = 7;
            CopyBinFile.Text = "Copy";
            CopyBinFile.UseVisualStyleBackColor = false;
            CopyBinFile.Click += CopyBinFile_Click;
            // 
            // CopyVltHash
            // 
            CopyVltHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyVltHash.Location = new System.Drawing.Point(400, 119);
            CopyVltHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyVltHash.Name = "CopyVltHash";
            CopyVltHash.Size = new System.Drawing.Size(96, 25);
            CopyVltHash.TabIndex = 8;
            CopyVltHash.Text = "Copy";
            CopyVltHash.UseVisualStyleBackColor = false;
            CopyVltHash.Click += CopyVltHash_Click;
            // 
            // CopyVltFile
            // 
            CopyVltFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyVltFile.Location = new System.Drawing.Point(400, 154);
            CopyVltFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CopyVltFile.Name = "CopyVltFile";
            CopyVltFile.Size = new System.Drawing.Size(96, 25);
            CopyVltFile.TabIndex = 9;
            CopyVltFile.Text = "Copy";
            CopyVltFile.UseVisualStyleBackColor = false;
            CopyVltFile.Click += CopyVltFile_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(71, 18);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(35, 15);
            label1.TabIndex = 10;
            label1.Text = "Input";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(34, 53);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 15);
            label2.TabIndex = 11;
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
            label3.TabIndex = 12;
            label3.Text = "Bin File";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(37, 123);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(69, 15);
            label4.TabIndex = 13;
            label4.Text = "Vlt Memory";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label5.Location = new System.Drawing.Point(64, 158);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(42, 15);
            label5.TabIndex = 14;
            label5.Text = "Vlt File";
            // 
            // Hasher
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(534, 191);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(CopyVltFile);
            Controls.Add(CopyVltHash);
            Controls.Add(CopyBinFile);
            Controls.Add(CopyBinHash);
            Controls.Add(CopyString);
            Controls.Add(VltFileTextbox);
            Controls.Add(VltHashTextbox);
            Controls.Add(BinFileTextbox);
            Controls.Add(BinHashTextbox);
            Controls.Add(StringTextbox);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Hasher";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "NFS-Hasher by MaxHwoy";
            ResumeLayout(false);
            PerformLayout();
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
