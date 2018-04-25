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
    public partial class batchName : Form
    {
        private string batch_name;
        private string invalid_msg = "Invalid character";
        private string valid_msg = "Please name this batch";
        private string approved_characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789()_- ";

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public batchName(string default_batch_name)
        {
            InitializeComponent();

            batch_name = default_batch_name;

        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void batchName_Load(object sender, EventArgs e)
        {
            textBox1.Text = batch_name;
            batchLabel.Text = valid_msg;
            button1.Enabled = true;
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void button1_Click(object sender, EventArgs e)
        {
            batch_name = textBox1.Text;
            this.Close();
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public String getText()
        {
            return batch_name;
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Check user input
            if(textBox1.Text.All(approved_characters.Contains))
            {
                batchLabel.Text = valid_msg;
                button1.Enabled = true;
            }
            else
            {
                batchLabel.Text = invalid_msg;
                button1.Enabled = false;
            }
            
            
        }
    }
}
