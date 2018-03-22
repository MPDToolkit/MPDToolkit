using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnamolyDetector
{
    public partial class batchName : Form
    {
        private string batch_name;

        public batchName(string default_batch_name)
        {
            InitializeComponent();

            batch_name = default_batch_name;

            textBox1.Text = batch_name;
        }

        private void batchName_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            batch_name = textBox1.Text;
            this.Close();
        }

        public String getText()
        {
            return batch_name;
        }

    }
}
