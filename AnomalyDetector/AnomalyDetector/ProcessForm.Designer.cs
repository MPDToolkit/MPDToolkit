namespace AnamolyDetector
{
    partial class ProcessForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terminateAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblPercent = new System.Windows.Forms.Label();
            this.filesSelected = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(842, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "Menu";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terminateAnalysisToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(69, 29);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // terminateAnalysisToolStripMenuItem
            // 
            this.terminateAnalysisToolStripMenuItem.Name = "terminateAnalysisToolStripMenuItem";
            this.terminateAnalysisToolStripMenuItem.Size = new System.Drawing.Size(241, 30);
            this.terminateAnalysisToolStripMenuItem.Text = "Terminate Analysis";
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(645, 285);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(171, 45);
            this.btnAnalyze.TabIndex = 1;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(219, 46);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(171, 45);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(24, 46);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(171, 45);
            this.btnSelectFolder.TabIndex = 3;
            this.btnSelectFolder.Text = "Select Folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Location = new System.Drawing.Point(21, 126);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(72, 20);
            this.lblProgressBar.TabIndex = 4;
            this.lblProgressBar.Text = "Progress";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(24, 160);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(794, 31);
            this.progressBar1.TabIndex = 5;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(780, 126);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(36, 20);
            this.lblPercent.TabIndex = 6;
            this.lblPercent.Text = "0 %";
            // 
            // filesSelected
            // 
            this.filesSelected.AutoSize = true;
            this.filesSelected.Location = new System.Drawing.Point(522, 58);
            this.filesSelected.Name = "filesSelected";
            this.filesSelected.Size = new System.Drawing.Size(133, 20);
            this.filesSelected.TabIndex = 7;
            this.filesSelected.Text = "No Files Selected";
            // 
            // ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 351);
            this.Controls.Add(this.filesSelected);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblProgressBar);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(864, 407);
            this.MinimumSize = new System.Drawing.Size(864, 407);
            this.Name = "ProcessForm";
            this.Text = "ProcessForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terminateAnalysisToolStripMenuItem;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Label lblProgressBar;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label filesSelected;
    }
}