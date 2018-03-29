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
    public partial class MainForm : Form
    {
        public string selectResultsFolder = "";
        public List<String> currentImages = new List<String>();

        public MainForm()
        {
            InitializeComponent();
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
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //find image with selected checkbox item and show it in pictureBoxes 

            string itemFullPath = Path.Combine(selectResultsFolder, checkedListBox.SelectedItem.ToString());
            
           
            pictureBox1.ImageLocation = itemFullPath;
            //TODO set pictureBox2.ImageLocation to the item's detected path
        }

        private Boolean checkImages(String image)
        {
            String Path = System.IO.Path.Combine(Environment.CurrentDirectory, "..\\Batches");
            if (File.Exists(System.IO.Path.Combine(Path, "Results", image)))
                return true;

            return false;
        }

        private void loadImages()
        {
            String combined = System.IO.Path.Combine(selectResultsFolder, "Copy");
            string[] temp = Directory.GetFiles(combined);

            for (int i = 0; i < temp.Length; i++)  //Ensure images have results and are of right format
               if (checkImages(temp[i]))
                    if (temp[i].ToLower().EndsWith(".jpg") || temp[i].EndsWith(".jpeg") || temp[i].EndsWith(".png"))
                        currentImages.Add(Path.GetFileName(temp[i]));

            checkedListBox.DataSource = currentImages;
        }


    }
}
