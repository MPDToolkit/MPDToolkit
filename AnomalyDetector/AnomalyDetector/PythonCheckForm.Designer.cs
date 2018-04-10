namespace AnomalyDetector
{
    partial class PythonCheckForm
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
            this.infoLabel = new System.Windows.Forms.Label();
            this.pythonLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(325, 22);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Please wait while we search for python...";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pythonLink
            // 
            this.pythonLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pythonLink.Location = new System.Drawing.Point(12, 43);
            this.pythonLink.Name = "pythonLink";
            this.pythonLink.Size = new System.Drawing.Size(325, 19);
            this.pythonLink.TabIndex = 1;
            this.pythonLink.TabStop = true;
            this.pythonLink.Text = "Python 3.6.4 Download Page";
            this.pythonLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pythonLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.pythonLink_LinkClicked);
            // 
            // PythonCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 71);
            this.Controls.Add(this.pythonLink);
            this.Controls.Add(this.infoLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(365, 110);
            this.Name = "PythonCheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PythonCheckForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.LinkLabel pythonLink;
    }
}