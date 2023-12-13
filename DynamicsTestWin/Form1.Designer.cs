namespace DynamicsTestWin
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
            this.dgTest = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkMultiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getFromWCFServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgTest)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgTest
            // 
            this.dgTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTest.Location = new System.Drawing.Point(0, 28);
            this.dgTest.Name = "dgTest";
            this.dgTest.RowHeadersWidth = 51;
            this.dgTest.RowTemplate.Height = 24;
            this.dgTest.Size = new System.Drawing.Size(800, 422);
            this.dgTest.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkMultiToolStripMenuItem,
            this.getFromWCFServiceToolStripMenuItem,
            this.openItemToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // checkMultiToolStripMenuItem
            // 
            this.checkMultiToolStripMenuItem.Name = "checkMultiToolStripMenuItem";
            this.checkMultiToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.checkMultiToolStripMenuItem.Text = "Get From Dynamics";
            this.checkMultiToolStripMenuItem.Click += new System.EventHandler(this.checkMultiToolStripMenuItem_Click);
            // 
            // getFromWCFServiceToolStripMenuItem
            // 
            this.getFromWCFServiceToolStripMenuItem.Name = "getFromWCFServiceToolStripMenuItem";
            this.getFromWCFServiceToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.getFromWCFServiceToolStripMenuItem.Text = "Get From WCF Service";
            this.getFromWCFServiceToolStripMenuItem.Click += new System.EventHandler(this.getFromWCFServiceToolStripMenuItem_Click);
            // 
            // openItemToolStripMenuItem
            // 
            this.openItemToolStripMenuItem.Name = "openItemToolStripMenuItem";
            this.openItemToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.openItemToolStripMenuItem.Text = "Open Item";
            this.openItemToolStripMenuItem.Click += new System.EventHandler(this.openItemToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgTest);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgTest)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgTest;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkMultiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getFromWCFServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openItemToolStripMenuItem;
    }
}

