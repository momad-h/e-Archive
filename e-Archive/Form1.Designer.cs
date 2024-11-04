namespace e_Archive
{
    partial class Form1
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
            this.txtRootPath = new System.Windows.Forms.TextBox();
            this.btnAddToArchive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRootPath
            // 
            this.txtRootPath.Location = new System.Drawing.Point(48, 24);
            this.txtRootPath.Name = "txtRootPath";
            this.txtRootPath.Size = new System.Drawing.Size(298, 20);
            this.txtRootPath.TabIndex = 0;
            // 
            // btnAddToArchive
            // 
            this.btnAddToArchive.Location = new System.Drawing.Point(48, 147);
            this.btnAddToArchive.Name = "btnAddToArchive";
            this.btnAddToArchive.Size = new System.Drawing.Size(174, 23);
            this.btnAddToArchive.TabIndex = 1;
            this.btnAddToArchive.Text = "Add To Archive";
            this.btnAddToArchive.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 214);
            this.Controls.Add(this.btnAddToArchive);
            this.Controls.Add(this.txtRootPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRootPath;
        private System.Windows.Forms.Button btnAddToArchive;
    }
}

