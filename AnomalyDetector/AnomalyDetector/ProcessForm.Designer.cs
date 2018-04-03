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
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblPercent = new System.Windows.Forms.Label();
            this.filesSelected = new System.Windows.Forms.Label();
            this.infoLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(436, 159);
            this.btnAnalyze.Margin = new System.Windows.Forms.Padding(2);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(114, 29);
            this.btnAnalyze.TabIndex = 1;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(146, 22);
            this.btnSelectFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(114, 29);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(16, 22);
            this.btnSelectFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(114, 29);
            this.btnSelectFolder.TabIndex = 3;
            this.btnSelectFolder.Text = "Select Folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Location = new System.Drawing.Point(14, 76);
            this.lblProgressBar.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(128, 13);
            this.lblProgressBar.TabIndex = 4;
            this.lblProgressBar.Text = "Waiting for file selection...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 98);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(530, 20);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 5;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(520, 76);
            this.lblPercent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(24, 13);
            this.lblPercent.TabIndex = 6;
            this.lblPercent.Text = "0 %";
            // 
            // filesSelected
            // 
            this.filesSelected.AutoSize = true;
            this.filesSelected.Location = new System.Drawing.Point(433, 30);
            this.filesSelected.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.filesSelected.Name = "filesSelected";
            this.filesSelected.Size = new System.Drawing.Size(90, 13);
            this.filesSelected.TabIndex = 7;
            this.filesSelected.Text = "No Files Selected";
            // 
            // infoLog
            // 
            this.infoLog.Location = new System.Drawing.Point(16, 134);
            this.infoLog.Multiline = true;
            this.infoLog.Name = "infoLog";
            this.infoLog.ReadOnly = true;
            this.infoLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoLog.Size = new System.Drawing.Size(398, 53);
            this.infoLog.TabIndex = 8;
            this.infoLog.TabStop = false;
            // 
            // ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(564, 199);
            this.Controls.Add(this.infoLog);
            this.Controls.Add(this.filesSelected);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblProgressBar);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.btnAnalyze);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(580, 238);
            this.Name = "ProcessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProcessForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Label lblProgressBar;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label filesSelected;
        private System.Windows.Forms.TextBox infoLog;
    }
}