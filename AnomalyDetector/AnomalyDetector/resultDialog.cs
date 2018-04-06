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
    public partial class resultDialog : Form
    {
        public String Selection;

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public resultDialog()
        {
            InitializeComponent();
            LoadBatches();
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public void LoadBatches()
        {
            String Path = System.IO.Path.Combine(Environment.CurrentDirectory, "Batches");
            String[] directories = System.IO.Directory.GetDirectories(Path);

            for (int i = 0; i < directories.Length; i++)
            {
                int num_checked = System.IO.File.ReadAllLines(directories[i] + @"\checkbox.ini").Count<string>();
                int num_total = System.IO.Directory.GetFiles(directories[i] + @"\Copy").Count<string>();

                //listBox1.Items.Add( num_checked.ToString() + "/" + num_total.ToString() + "\t" + System.IO.Path.GetFileName(directories[i]));

                dataGridView1.Rows.Add( System.IO.Path.GetFileName(directories[i]), num_checked, num_total );

            }

        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void button1_Click(object sender, EventArgs e)
        {
            
            //String temp = listBox1.GetItemText( listBox1.SelectedItem ).Split('\t')[1];
            String temp = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            Selection = System.IO.Path.Combine(Environment.CurrentDirectory, "Batches", temp);
            this.Close();
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public String getSelected()
        {
            return Selection;
        }

       
    }
}
