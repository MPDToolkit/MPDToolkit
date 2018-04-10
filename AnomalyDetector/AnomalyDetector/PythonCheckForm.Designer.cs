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
            this.infoLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(82, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(274, 18);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Please wait while we search for python...";
            // 
            // pythonLink
            // 
            this.pythonLink.AutoSize = true;
            this.pythonLink.Location = new System.Drawing.Point(280, 58);
            this.pythonLink.Name = "pythonLink";
            this.pythonLink.Size = new System.Drawing.Size(151, 18);
            this.pythonLink.TabIndex = 1;
            this.pythonLink.TabStop = true;
            this.pythonLink.Text = "Python 3.6 Web Page";
            this.pythonLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.pythonLink_LinkClicked);
            // 
            // PythonCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 85);
            this.Controls.Add(this.pythonLink);
            this.Controls.Add(this.infoLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PythonCheckForm";
            this.Text = "PythonCheckForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.LinkLabel pythonLink;
    }
}