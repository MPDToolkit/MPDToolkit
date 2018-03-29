using AnomalyDetector;
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

namespace AnamolyDetector
{

    //Global Settings
    public struct Settings
    {
        bool FirstRun;
        string PythonPath;
        string BatchesPath;
        string BinPath;
    }


    public partial class MainForm : Form
    {
        public Settings settings = new Settings();     //Values read from the 'settings.ini'
        public string settingsPath = @"settings.ini";
        public string selectResultsFolder = "";
        public List<String> currentImages = new List<String>();

        public MainForm()
        {
            InitializeComponent();
        }




        private void ReadSettings()
        {
            string[] entries = File.ReadAllLines(settingsPath);

            foreach (string str in entries)
            {
                string[] opt = str.Split(':');
                switch (opt[0])
                {
                    case "PythonPath":
                        {

                            break;
                        }

                    default:
                        {
                            break;
                        }
                }




            }


        }

        //Update the settings.ini
        private void UpdateSettings()
        {



        }


        private void menuBtnNewAnalysis_Click(object sender, EventArgs e)
        {
            ProcessForm proc_form = new ProcessForm();
            proc_form.Show();
        }

        private void menuBtnSelectResults_Click(object sender, EventArgs e)
        {
            resultDialog results = new resultDialog();
            results.ShowDialog();

            selectResultsFolder = results.getSelected();
            this.loadImages();

            pictureBox1.ImageLocation = "";
            pictureBox2.ImageLocation = "";
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //find image with selected checkbox item and show it in pictureBoxes 
            if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 1) == true)
                pictureBox2.ImageLocation = Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString());

            if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 0)==true)
                pictureBox1.ImageLocation = Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString());

        }

        private bool checkImages(string image, int val)
        {
            if(val == 0)
                return (File.Exists(Path.Combine(selectResultsFolder, "Copy", image)));
            if(val == 1)
                return (File.Exists(Path.Combine(selectResultsFolder, "Detected", image)));
            return false;
        }

        private void loadImages()
        {
            String combined = System.IO.Path.Combine(selectResultsFolder, "Detected");
            string[] temp = Directory.GetFiles(combined);
            currentImages.Clear();
            checkedListBox.Items.Clear();
           

            for (int i = 0; i < temp.Length; i++)  //Ensure images have results and are of right format
                 if (temp[i].ToLower().EndsWith(".jpg") || temp[i].EndsWith(".jpeg") || temp[i].EndsWith(".png"))
                    currentImages.Add(Path.GetFileName(temp[i]));

            for(int i=0; i<currentImages.Count; i++)
                checkedListBox.Items.Add(currentImages[i]);
        }


    }
}
