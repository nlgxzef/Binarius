namespace Binary.Interact
{
    partial class Exporter
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Exporter));
            ExportSerialized = new System.Windows.Forms.CheckBox();
            ExporterButton = new System.Windows.Forms.Button();
            ExporterToolTip = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // ExportSerialized
            // 
            ExportSerialized.Checked = true;
            ExportSerialized.CheckState = System.Windows.Forms.CheckState.Checked;
            ExportSerialized.Location = new System.Drawing.Point(13, 14);
            ExportSerialized.Name = "ExportSerialized";
            ExportSerialized.Size = new System.Drawing.Size(89, 23);
            ExportSerialized.TabIndex = 0;
            ExportSerialized.Text = "Serialized";
            // 
            // ExporterButton
            // 
            ExporterButton.AutoSize = true;
            ExporterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ExporterButton.Location = new System.Drawing.Point(108, 12);
            ExporterButton.Name = "ExporterButton";
            ExporterButton.Size = new System.Drawing.Size(94, 27);
            ExporterButton.TabIndex = 1;
            ExporterButton.Text = "Export";
            ExporterButton.UseVisualStyleBackColor = true;
            ExporterButton.Click += ExporterButton_Click;
            // 
            // Exporter
            // 
            AcceptButton = ExporterButton;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(214, 48);
            Controls.Add(ExporterButton);
            Controls.Add(ExportSerialized);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Exporter";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Exporter";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox ExportSerialized;
        private System.Windows.Forms.Button ExporterButton;
        private System.Windows.Forms.ToolTip ExporterToolTip;
    }
}