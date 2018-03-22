namespace AnamolyDetector
{
    partial class batchName
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
            this.batchLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // batchLabel
            // 
            this.batchLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.batchLabel.AutoSize = true;
            this.batchLabel.Location = new System.Drawing.Point(182, 46);
            this.batchLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.batchLabel.MinimumSize = new System.Drawing.Size(236, 25);
            this.batchLabel.Name = "batchLabel";
            this.batchLabel.Size = new System.Drawing.Size(236, 25);
            this.batchLabel.TabIndex = 0;
            this.batchLabel.Text = "Please name this batch";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(64, 96);
            this.textBox1.Margin = new System.Windows.Forms.Padding(6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(494, 31);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(230, 151);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // batchName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 230);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.batchLabel);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "batchName";
            this.Text = "Batch Name";
            this.Load += new System.EventHandler(this.batchName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label batchLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
    }
}