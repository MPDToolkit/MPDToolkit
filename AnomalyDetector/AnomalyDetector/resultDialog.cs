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
                listBox1.Items.Add(System.IO.Path.GetFileName(directories[i]));
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void button1_Click(object sender, EventArgs e)
        {
            
            String temp = listBox1.GetItemText(listBox1.SelectedItem);
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
