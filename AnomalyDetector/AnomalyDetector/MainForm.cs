using AnomalyDetector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnamolyDetector
{

    //Global Settings
    public struct Settings
    {
        public bool FirstRun;
        public bool RunPOST;
        public string PythonPath;
        public string BatchesPath;
        public string BinPath;
        public bool NewImageWindow;
    }


    public partial class MainForm : Form
    {
        public Settings settings = new Settings();     //Values read from the 'settings.ini'
        public string settingsPath = @"settings.ini";

        public string selectResultsFolder = "";
        public List<String> currentImages = new List<String>();
        public List<string> checked_images = new List<string>();

        public Process proc_orig_image;
        public Process proc_detect_image;

        public MainForm()
        {
            InitializeComponent();


            //Read settings.ini
            ReadSettings();

            if(settings.RunPOST) POST();

            //Update settings.ini
            UpdateSettings();

        }




        private void ReadSettings()
        {
            string[] entries = File.ReadAllLines(settingsPath);

            foreach (string str in entries)
            {
                string[] opt = str.Split('=');
                switch (opt[0])
                {
                    case "FirstRun":
                        {
                            settings.RunPOST = (!string.IsNullOrEmpty(opt[1])) ? Convert.ToBoolean(opt[1]) : true;
                            break;
                        }

                    case "RunPOST":
                        {
                            settings.RunPOST = (!string.IsNullOrEmpty(opt[1])) ? Convert.ToBoolean(opt[1]) : false;
                            break;
                        }

                    case "PythonPath":
                        {
                            settings.PythonPath = opt[1];
                           
                            if (string.IsNullOrEmpty(settings.PythonPath))
                            {
                                //Check the default python3 install location
                                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Python\Python36\python.exe")))
                                {
                                    settings.PythonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Python\Python36\python.exe");
                                }
                            }
                            break;
                        }

                    case "BatchesPath":
                        {
                            settings.BatchesPath = opt[1];
                            if (string.IsNullOrEmpty(settings.BatchesPath))
                            {
                                settings.BatchesPath = Path.Combine(Environment.CurrentDirectory, "Batches");
                            }
                            break;
                        }
                    case "BinPath":
                        {
                            settings.BinPath = opt[1];
                            if (string.IsNullOrEmpty(settings.BinPath))
                            {
                                settings.BinPath = Path.Combine(Environment.CurrentDirectory, "bin");
                            }
                            break;
                        }

                    case "NewImageWindow":
                        {
                            settings.NewImageWindow = (!string.IsNullOrEmpty(opt[1])) ? Convert.ToBoolean(opt[1]) : false;
                            this.checkBoxNewWindow.Checked = settings.NewImageWindow;

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
            //string[] output = { settings.FirstRun.ToString(), settings.PythonPath, settings.BatchesPath, settings.BinPath };
            File.WriteAllLines(settingsPath, 
                new string[]{
                    "FirstRun=" + settings.FirstRun.ToString(),
                    "RunPOST=" + settings.RunPOST.ToString(),
                    "PythonPath=" + settings.PythonPath,
                    "BatchesPath=" + settings.BatchesPath,
                    "BinPath=" + settings.BinPath,
                    "NewImageWindow=" + settings.NewImageWindow
                });
        }

        //Reads the checkbox.ini found in each batch folder
        private List<string> ReadCheckbox(string batch)
        {
            List<string> str = File.ReadAllLines(batch + @"\checkbox.ini").ToList<string>();
            return str;
        }

        //Updates the checkbox.ini found in each batch folder
        private void UpdateCheckbox(string batch, string[] str)
        {
            File.WriteAllLines(batch + @"\checkbox.ini", str);
        }

        //Power On Self Test
        private int POST()
        {
            Console.Out.WriteLine("-------------Running POST-------------");

            //Check for Python
            if( !string.IsNullOrEmpty(settings.PythonPath) && File.Exists(settings.PythonPath) )
            {
                //Check/Install required packages
                Console.WriteLine("...Checking Python Version...");
                string python_version = Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\checkPythonVersion.bat"), CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }).StandardOutput.ReadToEnd();
                if (python_version.Contains("Python 3.6.4")) Console.WriteLine("Python Version --> OK");
                else Console.WriteLine("Python Version 3.6.4 Required!");

                Console.WriteLine("...Checking Python Packages...");
                string pip_list = Process.Start(new ProcessStartInfo { FileName=Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\checkPythonPackages.bat"), CreateNoWindow=true, UseShellExecute=false, RedirectStandardOutput=true }).StandardOutput.ReadToEnd();
                string[] required_pkgs = { "opencv-python", "numpy", "scipy", "scikit-learn", "spectral", "pyparsing", "matplotlib"};
                foreach(string str in required_pkgs)
                {
                    if (!pip_list.Contains(str))
                    {
                        //Console.WriteLine("Installing missing packages...");
                        //Console.WriteLine(str);
                        Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\installPythonPackages.bat"), CreateNoWindow = true, UseShellExecute = false }).WaitForExit();
                        break;
                    }
                }
                Console.Out.WriteLine("Python Packages --> OK"); 
            }
            else
            {
                //Error: No python installation found
                MessageBox.Show("Image analysis requires Python 3 to be installed.", "Missing Python", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.menuBtnNewAnalysis.Enabled = false;
               
            }

            return 0;
        }


        private void menuBtnNewAnalysis_Click(object sender, EventArgs e)
        {
            ProcessForm proc_form = new ProcessForm(settings);
            proc_form.Show();
        }




        private void menuBtnSelectResults_Click(object sender, EventArgs e)
        {
            resultDialog results = new resultDialog();
            results.ShowDialog();


            if (results.DialogResult == DialogResult.OK && !string.IsNullOrEmpty(results.getSelected()))
            {
                selectResultsFolder = results.getSelected();
                this.loadImages();

                pictureBox1.ImageLocation = "";
                pictureBox2.ImageLocation = "";
            }
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!settings.NewImageWindow)
            {
                //find image with selected checkbox item and show it in pictureBoxes 
                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 1) == true)
                    pictureBox2.ImageLocation = Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString());

                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 0) == true)
                    pictureBox1.ImageLocation = Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString());
            }
            else
            {
                //find image with selected checkbox item and show it in pictureBoxes 
                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 1) == true)
                {
                    //pictureBox2.ImageLocation = Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString());
                    Process.Start(Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString()));
                }
                   
                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 0) == true)
                {
                    //pictureBox1.ImageLocation = Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString());
                    Process.Start(Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString()));
                }
                    
            }
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
            //Read the checkbox.ini
            checked_images = ReadCheckbox(selectResultsFolder);

            String combined = System.IO.Path.Combine(selectResultsFolder, "Detected");
            string[] temp = Directory.GetFiles(combined);
            currentImages.Clear();
            checkedListBox.Items.Clear();
           

            for (int i = 0; i < temp.Length; i++)  //Ensure images have results and are of right format
                 if (temp[i].ToLower().EndsWith(".jpg") || temp[i].EndsWith(".jpeg") || temp[i].EndsWith(".png"))
                    currentImages.Add(Path.GetFileName(temp[i]));

            for(int i=0; i<currentImages.Count; i++)
            {
                checkedListBox.Items.Add(currentImages[i]);

                //Checks previously checked images
                if (checked_images.Contains(checkedListBox.Items[i].ToString()))
                {
                    checkedListBox.SetItemChecked(i, true);
                }
            }
               
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBox.GetItemChecked(e.Index))     //User unchecked an image
            {
                checked_images.Remove(checkedListBox.Items[e.Index].ToString());

                //Update the checkbox.ini
                UpdateCheckbox(selectResultsFolder, checked_images.ToArray());
            }
            else    //User checked an image
            {
                if(!checked_images.Contains(checkedListBox.Items[e.Index].ToString()))
                    checked_images.Add(checkedListBox.Items[e.Index].ToString());

                //Update the checkbox.ini
                UpdateCheckbox(selectResultsFolder, checked_images.ToArray());
            }
        }

        private void checkBoxNewWindow_CheckedChanged(object sender, EventArgs e)
        {
            settings.NewImageWindow = checkBoxNewWindow.Checked;
            UpdateSettings();

            if (!settings.NewImageWindow)
            {
                //find image with selected checkbox item and show it in pictureBoxes 
                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 1) == true)
                    pictureBox2.ImageLocation = Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString());

                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 0) == true)
                    pictureBox1.ImageLocation = Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString());
            }
            else
            {
                //find image with selected checkbox item and show it in pictureBoxes 
                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 1) == true)
                {
                    //pictureBox2.ImageLocation = Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString());
                    Process.Start(Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString()));
                }

                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 0) == true)
                {
                    //pictureBox1.ImageLocation = Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString());
                    Process.Start(Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString()));
                }

            }
        }
    }
}
