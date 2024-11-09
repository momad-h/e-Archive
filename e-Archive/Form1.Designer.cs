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
            this.btnAddToArchive = new System.Windows.Forms.Button();
            this.txtRootPath = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnAddMainDocuments = new System.Windows.Forms.Button();
            this.btnAddSubDocuments = new System.Windows.Forms.Button();
            this.btnAddSubDocToMain = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddToArchive
            // 
            this.btnAddToArchive.Location = new System.Drawing.Point(11, 190);
            this.btnAddToArchive.Name = "btnAddToArchive";
            this.btnAddToArchive.Size = new System.Drawing.Size(129, 23);
            this.btnAddToArchive.TabIndex = 0;
            this.btnAddToArchive.Text = "Read Files";
            this.btnAddToArchive.UseVisualStyleBackColor = true;
            this.btnAddToArchive.Click += new System.EventHandler(this.btnAddToArchive_Click);
            // 
            // txtRootPath
            // 
            this.txtRootPath.Location = new System.Drawing.Point(32, 39);
            this.txtRootPath.Name = "txtRootPath";
            this.txtRootPath.Size = new System.Drawing.Size(260, 20);
            this.txtRootPath.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblProgress);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lblTotal);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lblResult);
            this.panel1.Location = new System.Drawing.Point(11, 98);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(309, 70);
            this.panel1.TabIndex = 20;
            // 
            // lblProgress
            // 
            this.lblProgress.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.Red;
            this.lblProgress.Location = new System.Drawing.Point(206, 10);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblProgress.Size = new System.Drawing.Size(230, 42);
            this.lblProgress.TabIndex = 21;
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(46, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 19);
            this.label5.TabIndex = 19;
            this.label5.Text = "Total :";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(138, 10);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(18, 19);
            this.lblTotal.TabIndex = 20;
            this.lblTotal.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(39, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 19);
            this.label6.TabIndex = 17;
            this.label6.Text = "Result :";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(138, 41);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(18, 19);
            this.lblResult.TabIndex = 18;
            this.lblResult.Text = "0";
            // 
            // btnAddMainDocuments
            // 
            this.btnAddMainDocuments.Location = new System.Drawing.Point(146, 190);
            this.btnAddMainDocuments.Name = "btnAddMainDocuments";
            this.btnAddMainDocuments.Size = new System.Drawing.Size(129, 23);
            this.btnAddMainDocuments.TabIndex = 21;
            this.btnAddMainDocuments.Text = "Add Main Documents";
            this.btnAddMainDocuments.UseVisualStyleBackColor = true;
            // 
            // btnAddSubDocuments
            // 
            this.btnAddSubDocuments.Location = new System.Drawing.Point(281, 190);
            this.btnAddSubDocuments.Name = "btnAddSubDocuments";
            this.btnAddSubDocuments.Size = new System.Drawing.Size(129, 23);
            this.btnAddSubDocuments.TabIndex = 22;
            this.btnAddSubDocuments.Text = "Add Sub Documents";
            this.btnAddSubDocuments.UseVisualStyleBackColor = true;
            // 
            // btnAddSubDocToMain
            // 
            this.btnAddSubDocToMain.Location = new System.Drawing.Point(416, 190);
            this.btnAddSubDocToMain.Name = "btnAddSubDocToMain";
            this.btnAddSubDocToMain.Size = new System.Drawing.Size(178, 23);
            this.btnAddSubDocToMain.TabIndex = 23;
            this.btnAddSubDocToMain.Text = "Add SubDocs To Main Docs";
            this.btnAddSubDocToMain.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 229);
            this.Controls.Add(this.btnAddSubDocToMain);
            this.Controls.Add(this.btnAddSubDocuments);
            this.Controls.Add(this.btnAddMainDocuments);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtRootPath);
            this.Controls.Add(this.btnAddToArchive);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddToArchive;
        private System.Windows.Forms.TextBox txtRootPath;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnAddMainDocuments;
        private System.Windows.Forms.Button btnAddSubDocuments;
        private System.Windows.Forms.Button btnAddSubDocToMain;
    }
}

