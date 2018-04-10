﻿using AnomalyDetector;
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

namespace AnomalyDetector
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

    //Global Parameters
    public struct Parameters
    {

    }



    public partial class MainForm : Form
    {
        //Settings
        public Settings settings = new Settings();     //Values read from the 'settings.ini'
        public string settingsPath = @"settings.ini";

        //Parameters


        //Result viewing variables
        public string selectResultsFolder = "";
        public List<String> currentImages = new List<String>();
        public List<string> checked_images = new List<string>();
        public int previousSelectedIndex = 0;
        public bool allow_checked = false;

        public Process proc_img1;
        public Process proc_img2;

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public MainForm()
        {
            InitializeComponent();

            //Read settings.ini
            ReadSettings();

            if( settings.FirstRun || settings.RunPOST ) POST();    //Should be redundant given the new installer

            //Update settings.ini
            UpdateSettings();

        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

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
                            settings.FirstRun = (!string.IsNullOrEmpty(opt[1])) ? Convert.ToBoolean(opt[1]) : true;

                            //TODO***RESET ALL PATH STRINGS

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

        //===================================================================================================================
        //-----------------------------------------------Update Settings File------------------------------------------------
        //===================================================================================================================

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

        //===================================================================================================================
        //-----------------------------------------------Read Checkbox File--------------------------------------------------
        //===================================================================================================================

        //Reads the checkbox.ini found in each batch folder
        private List<string> ReadCheckbox(string batch)
        {
            List<string> str = File.ReadAllLines(batch + @"\checkbox.ini").ToList<string>();
            return str;
        }

        //===================================================================================================================
        //----------------------------------------------Update Checkbox File-------------------------------------------------
        //===================================================================================================================

        //Updates the checkbox.ini found in each batch folder
        private void UpdateCheckbox(string batch, string[] str)
        {
            File.WriteAllLines(batch + @"\checkbox.ini", str);
        }

        //===================================================================================================================
        //-----------------------------------------------------POST----------------------------------------------------------
        //===================================================================================================================

        //Power On Self Test
        private void POST()
        {
            PythonCheckForm py = new PythonCheckForm(settings);
            py.ShowDialog();

        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void displayImages()
        {
            //Display initial image in list
            if (!settings.NewImageWindow)
            {
                //find image with selected checkbox item and show it in pictureBoxes 
                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 1) == true)
                    pictureBox2.ImageLocation = Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString());

                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 0) == true)
                    pictureBox1.ImageLocation = Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString());

                pictureBox1.Update();
                pictureBox2.Update();

            }
            else
            {
                //find image with selected checkbox item and show it in pictureBoxes 
                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 1) == true)
                {
                    Process.Start(Path.Combine(selectResultsFolder, "Detected", checkedListBox.SelectedItem.ToString()));
                }

                if (checkedListBox.SelectedItem != null && checkImages(checkedListBox.SelectedItem.ToString(), 0) == true)
                {
                    Process.Start(Path.Combine(selectResultsFolder, "Copy", checkedListBox.SelectedItem.ToString()));
                }
            }

        }

        //===================================================================================================================
        //-------------------------------------------------Analysis Btn Click------------------------------------------------
        //===================================================================================================================

        private void menuBtnNewAnalysis_Click(object sender, EventArgs e)
        {
            ProcessForm proc_form = new ProcessForm(settings);
            proc_form.Show();
        }

        //===================================================================================================================
        //-------------------------------------------------Results Btn Click-------------------------------------------------
        //===================================================================================================================

        private void menuBtnSelectResults_Click(object sender, EventArgs e)
        {
            resultDialog results = new resultDialog();
            results.ShowDialog();


            if (results.DialogResult == DialogResult.OK && !string.IsNullOrEmpty(results.getSelected()))
            {
                pictureBox1.ImageLocation = "";
                pictureBox2.ImageLocation = "";
                previousSelectedIndex = 0;

                selectResultsFolder = results.getSelected();

                //Update form title to the path of the batch being viewed
                this.Text = selectResultsFolder;

                //Load the detected images
                this.loadImages();

                //Allows the user to view a batch folder that is currently being analyzed
                FileSystemWatcher watcher = new FileSystemWatcher(Path.Combine(selectResultsFolder, "Detected"));
                watcher.EnableRaisingEvents = true;
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                //watcher.Created += Watcher_Created;
                watcher.Changed += Watcher_Changed;

                //Display currently selected images
                displayImages();
                
            }
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            //Calls the method with the thread that owns the UI object
            Invoke((MethodInvoker)(() => loadImages()));
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            
            //Calls the method with the thread that owns the UI object
            Invoke((MethodInvoker)(() => loadImages()));
            
        }

        
        //===================================================================================================================
        //--------------------------------------------Selected Index Changed-------------------------------------------------
        //===================================================================================================================

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Prevents multiple images from being open when checking an image that is currently selected
            if (previousSelectedIndex != checkedListBox.SelectedIndex)
            {
                previousSelectedIndex = checkedListBox.SelectedIndex;
                displayImages();
            }           
        }

        //===================================================================================================================
        //---------------------------------------------------Check Images----------------------------------------------------
        //===================================================================================================================

        private bool checkImages(string image, int val)
        {
            if(val == 0)
                return (File.Exists(Path.Combine(selectResultsFolder, "Copy", image)));
            if(val == 1)
                return (File.Exists(Path.Combine(selectResultsFolder, "Detected", image)));

            return false;
        }

        //===================================================================================================================
        //---------------------------------------------------Load Images-----------------------------------------------------
        //===================================================================================================================

        private void loadImages()
        {
            //Read the checkbox.ini
            checked_images = ReadCheckbox(selectResultsFolder);

            String combined = System.IO.Path.Combine(selectResultsFolder, "Detected");
            string[] temp = Directory.GetFiles(combined);

            //Reset current images and checkbox
            currentImages.Clear();
            checkedListBox.Items.Clear();

            //Ensure images have results and are of right format
            for (int i = 0; i < temp.Length; i++)  
                 if (temp[i].ToLower().EndsWith(".jpg") || temp[i].EndsWith(".jpeg") || temp[i].EndsWith(".png"))
                    currentImages.Add(Path.GetFileName(temp[i]));
            
            for(int i=0; i<currentImages.Count; i++)
            {
                checkedListBox.Items.Add(currentImages[i]);
                
                //Checks previously checked images
                if (checked_images.Contains(checkedListBox.Items[i].ToString()))
                {
                    allow_checked = true;   //Give permission to check image box
                    checkedListBox.SetItemChecked(i, true);
                }
            }

            //Set the initially selected index
            if (checkedListBox.Items.Count > 0)
            {
                if (settings.NewImageWindow)
                {
                    //This section updates the selected item while preventing multiple windows of the same image opening
                    //This is a bug only when viewing a batch while it is still running
                    settings.NewImageWindow = !settings.NewImageWindow;     //Sets to false
                    checkedListBox.SelectedIndex = previousSelectedIndex;
                    settings.NewImageWindow = !settings.NewImageWindow;     //Returns to original state
                }
                else
                {
                    checkedListBox.SelectedIndex = previousSelectedIndex;
                }
                
            }
                

        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //Check permission
            if(allow_checked)
            {
                //User unchecked an image
                if (checkedListBox.GetItemChecked(e.Index))     
                {
                    checked_images.Remove(checkedListBox.Items[e.Index].ToString());

                    //Update the checkbox.ini
                    UpdateCheckbox(selectResultsFolder, checked_images.ToArray());
                }

                //User checked an image
                else
                {
                    if (!checked_images.Contains(checkedListBox.Items[e.Index].ToString()))
                        checked_images.Add(checkedListBox.Items[e.Index].ToString());

                    //Update the checkbox.ini
                    UpdateCheckbox(selectResultsFolder, checked_images.ToArray());
                }
            }
            else
            {
                //Revert checkbox state if permission was denied
                e.NewValue = e.CurrentValue;
            }

            //Revoke permission
            allow_checked = false;
        }

        //===================================================================================================================
        //----------------------------------------------New Window Checked Changed-------------------------------------------
        //===================================================================================================================

        private void checkBoxNewWindow_CheckedChanged(object sender, EventArgs e)
        {
            settings.NewImageWindow = checkBoxNewWindow.Checked;
            UpdateSettings();

            displayImages();
            checkedListBox.Focus();     //Set focus back to the checkbox
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void checkedListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                allow_checked = true;   
        }
    }
}
