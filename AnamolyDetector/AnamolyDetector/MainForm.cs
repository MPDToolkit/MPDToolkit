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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void menuBtnNewAnalysis_Click(object sender, EventArgs e)
        {
            ProcessForm proc_form = new ProcessForm();
            proc_form.Show();
        }
    }
}
