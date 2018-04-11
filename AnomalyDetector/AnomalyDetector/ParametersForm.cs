using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnomalyDetector
{
    public partial class ParametersForm : Form
    {
        public List<string> paramList { get; set; }
        public bool saved_changes = false;


        public ParametersForm()
        {
            InitializeComponent();

            //Read the parameters.ini file and load into form
            Read();
            saveStatus.Text = "Not Saved...";
        }

        //Read the parameters.ini file
        private void Read()
        {
            string[] entries = File.ReadAllLines(@"bin\parameters.ini");
            foreach (string str in entries)
            {
                //Skip comment lines
                if (str.StartsWith("#")) continue;

                //Remove any remaining comment sections
                string p_str = str.Split('#')[0];

                string[] opt = p_str.Split('=');      //name=default=value
                paramData.Rows.Add( Convert.ToBoolean(opt[1]), opt[0], Convert.ToDouble(opt[2]) );
            }

        }

        //Save to the parameters.ini file
        private void Save()
        {
            if (paramData.Rows.Count > 0)
            {
                //Create array of strings
                foreach (DataGridViewRow row in paramData.Rows)
                {
                    paramList.Add(row.Cells[1].Value.ToString() + "=" + row.Cells[0].Value.ToString() + "=" + row.Cells[2].Value.ToString());
                }

                //Write string array to file
                File.WriteAllLines(@"bin\parameters.ini", paramList.ToArray());
            }

            saved_changes = true;
            saveStatus.Text = "Saved...";

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(!saved_changes)
            {
                DialogResult msg = MessageBox.Show("There are unsaved changes. Would you like to save before exitting?", "Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (msg == DialogResult.Yes)
                    Save();
                else if (msg == DialogResult.No)
                    this.Close();
            }
            else
            {
                this.Close();
            }
           
        }

        private void paramData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Something was changed
            saved_changes = false;
            saveStatus.Text = "Not Saved...";
        }

        private void ParametersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Last chance to save
            if (!saved_changes)
            {
                if (MessageBox.Show("Any unsaved changes will be lost. Would you like to save before exitting?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Save();
            }
        }
    }
}
