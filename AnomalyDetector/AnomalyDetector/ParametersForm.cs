using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnomalyDetector
{
    public partial class ParametersForm : Form
    {
        public List<string> parameters { get; set; }


        public ParametersForm()
        {
            InitializeComponent();

            //Read the parameters.ini file and load into form
            Read();
        }

        //Read the parameters.ini file
        private void Read()
        {


        }

        //Save to the parameters.ini file
        private void Save()
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
