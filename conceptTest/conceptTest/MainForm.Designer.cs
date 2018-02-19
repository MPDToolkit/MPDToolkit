namespace conceptTest
{
    partial class MainForm
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
            this.txtbxArgv = new System.Windows.Forms.TextBox();
            this.txtbxLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyze.Location = new System.Drawing.Point(1127, 56);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(129, 45);
            this.btnAnalyze.TabIndex = 0;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // txtbxArgv
            // 
            this.txtbxArgv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbxArgv.Location = new System.Drawing.Point(95, 60);
            this.txtbxArgv.Name = "txtbxArgv";
            this.txtbxArgv.Size = new System.Drawing.Size(986, 31);
            this.txtbxArgv.TabIndex = 1;
            // 
            // txtbxLog
            // 
            this.txtbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbxLog.Location = new System.Drawing.Point(95, 148);
            this.txtbxLog.Multiline = true;
            this.txtbxLog.Name = "txtbxLog";
            this.txtbxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtbxLog.Size = new System.Drawing.Size(1161, 474);
            this.txtbxLog.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 667);
            this.Controls.Add(this.txtbxLog);
            this.Controls.Add(this.txtbxArgv);
            this.Controls.Add(this.btnAnalyze);
            this.MinimumSize = new System.Drawing.Size(778, 572);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.TextBox txtbxArgv;
        private System.Windows.Forms.TextBox txtbxLog;
    }
}