﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace AnomalyDetector
{


    public partial class ProcessForm : Form
    {
        String batchName;
        string[] selectedFileNames;
        private string workingDirectory;
        private string batchesDirectory;
        private string currentBatch;
        private string pythonPath;
        private int fileCt = 0;
        private Process backendProcess;
        private string copyDir;
        private string detDir;
        private string othDir;
        private int completed_files_ct = 0;
        private string infoLogStr;
        private int num_threads = 1;

        private List<string> batch_names = new List<string>();

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public ProcessForm(Settings settings)
        {
            InitializeComponent();

            workingDirectory = Environment.CurrentDirectory;
            batchesDirectory = Path.Combine(workingDirectory, "Batches");

            pythonPath = settings.PythonPath;

            if (settings.AllowMultiThread)
            {
                num_threads = System.Environment.ProcessorCount;

                //Reserve 2 processes for the rest of the system
                if (num_threads > 2)
                    num_threads -= 2;
            }
            else num_threads = 1;
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        //Computes the next default batch name
        private string getNextBatchName()
        {
            string[] batch_paths = Directory.GetDirectories(batchesDirectory);
            int ct = 0;

            //Finds the latest default batch name
            foreach (string str_path in batch_paths)
            {
                string str = str_path.Split('\\').Last<string>();

                if (str.StartsWith("Batch_") && !str.EndsWith(")"))
                {
                    if (ct < Convert.ToInt32(str.Split('_')[1]))
                    {
                        ct = Convert.ToInt32(str.Split('_')[1]);
                    }
                }
            }
            ct++;   //Increment default name by 1

            return "Batch_" + ct.ToString();
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void createDirectories()
        {
            try
            {

                //Only create the directories when a folder has been selected
                Directory.CreateDirectory(currentBatch);
                Directory.CreateDirectory(copyDir);
                Directory.CreateDirectory(detDir);
                Directory.CreateDirectory(othDir);
                File.Create(currentBatch + @"\batch_log.txt").Close();
                File.Create(currentBatch + @"\checkbox.ini").Close();
            }
            catch
            {

            }
        }

        private void btnBatchName_Click(object sender, EventArgs e)
        {
            try
            {

                //Get the next default batch name
                batchName bn = new batchName(getNextBatchName());

                //Prompt user to select a batch name
                if (bn.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(bn.getText()))
                {
                    batchName = bn.getText();
                    currentBatch = batchesDirectory + "\\" + batchName;

                    //Handles case where batch name already exists
                    int i = 0;
                    if (Directory.Exists(currentBatch))
                    {
                        i++;
                        while (Directory.Exists(currentBatch + " (" + i.ToString() + ")"))
                        {
                            i++;
                        }
                        batchName += " (" + i.ToString() + ")";
                    }

                    //Update the window title
                    this.Text = batchName;

                    btnBatchName.Enabled = false;
                    btnBatchName.Visible = false;
                    btnSelectFolder.Enabled = true;
                    btnSelectFile.Enabled = true;
                    btnAnalyze.Enabled = true;

                    currentBatch = batchesDirectory + "\\" + batchName;
                    copyDir = batchesDirectory + "\\" + batchName + "\\Copy";
                    detDir = batchesDirectory + "\\" + batchName + "\\Detected";
                    othDir = batchesDirectory + "\\" + batchName + "\\Other";

                    createDirectories();


                }
            }
            catch
            {

            }
        }

        private delegate void LabelInfo(string file, string progress);
        private void UpdateLabelInfo(string file, string progress)
        {
            filesSelected.Text = file + fileCt;
            lblProgressBar.Text = progress;
            lblProgressBar.Update();
        }

        private void run_copy(object sender, DoWorkEventArgs e)
        {
            try
            {

                string path;
                foreach (string file in selectedFileNames)
                {
                    if (file.ToLower().EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png"))
                    {
                        path = Path.Combine(copyDir, Path.GetFileName(file));
                        File.Copy(file, path, true);
                        fileCt++;
                    }
                }

                if (fileCt > 0)
                    Invoke(new LabelInfo(UpdateLabelInfo), "Files Selected: ", "Ready to analyze...");

            }
            catch
            {

            }
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            try
            {

                //Prompt user to select a batch name
                if (!String.IsNullOrEmpty(batchName))
                {          
                    //Setup the file selection window
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    openFileDialog1.Filter = "image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png|All files (*.*)|*.*";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.RestoreDirectory = true;
                    openFileDialog1.Multiselect = true;

                    //Prompt user to select an image(s)
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        //Number of selected images exceeds the limit performance is guaranteed
                        if (openFileDialog1.FileNames.Length > 1000)
                        {
                            if (MessageBox.Show("It is not recommended to run more than 1000 files. Speed is not guaranteed.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;          
                        }

                        //Extracts the valid images from the selected image(s)
                        selectedFileNames = openFileDialog1.FileNames;

                        lblProgressBar.Text = "Copying Images...";

                        //Create a background thread for the progress bar 
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(run_copy);
                        worker.RunWorkerAsync(this);

                    }
                }
            }
            catch
            {

            }
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            try
            {

                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

                //Prompt user to select a batch name
                if (!String.IsNullOrEmpty(batchName))
                {
               
                    //Prompt user to select a directory of images
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        String[] FileNames = Directory.GetFiles(folderBrowserDialog1.SelectedPath);

                        //Number of selected images exceeds the limit performance is guaranteed
                        if (FileNames.Length > 1000)
                        {
                            if (MessageBox.Show("It is not recommended to run more than 1000 files. Speed is not guaranteed.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                        }


                        selectedFileNames = FileNames;

                        lblProgressBar.Text = "Copying Images...";

                        //Create a background thread for the progress bar 
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(run_copy);
                        worker.RunWorkerAsync(this);
                        
                    }
                }
            }
            catch
            {

            }
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            try
            {

                if(fileCt > 0)
                {
                    //Disable buttons
                    btnSelectFolder.Enabled = false;
                    btnSelectFile.Enabled = false;
                    btnAnalyze.Enabled = false;

                    //Inform user that images are being analyzed
                    lblProgressBar.Text = "Initializing...";
                    lblProgressBar.Update();

                    string pythonArgs = "\"" + @workingDirectory + @"\bin\analyze.py" + "\" -F \"" + @currentBatch + "\"" + " -p " + num_threads.ToString();

                    if (currentBatch != null)
                    {
                        ProcessStartInfo startConfig = new ProcessStartInfo(pythonPath, pythonArgs);
                        startConfig.UseShellExecute = false;
                        startConfig.RedirectStandardOutput = true;
                        startConfig.RedirectStandardError = true;
                        startConfig.CreateNoWindow = true;

                        backendProcess = new Process { StartInfo = startConfig };

                        //Create output handlers
                        backendProcess.OutputDataReceived += redirectHandler;
                        backendProcess.ErrorDataReceived += redirectHandler;
                        backendProcess.EnableRaisingEvents = true;

                        //Create a background thread for the progress bar 
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(run_analyze);
                        worker.RunWorkerAsync(this);

                    }
                }
            }
            catch
            {

            }
            
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        private void run_analyze(object sender, DoWorkEventArgs e)
        {
            //Start the python process
            backendProcess.Start();
            backendProcess.BeginOutputReadLine();
            backendProcess.BeginErrorReadLine();

            //Wait for backend to finish, then clean up
            backendProcess.WaitForExit();      
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        //Update progress of the backend
        private delegate void Change(string status, int complete, int total);
        private void OnChange(string status, int complete, int total)
        {
            try
            {

                //Update progress form based on the given conditions        
                if (!string.IsNullOrEmpty(status))
                {
                    string[] opt = status.Split(' ');
                    switch (opt[0])
                    {
                        case "-i-":     //Initialization of backend completed
                            {
                                progressBar1.Visible = true;
                                progressBar1.Minimum = 0;
                                progressBar1.Value = 0;
                                progressBar1.Update();
                                lblPercent.Text = Convert.ToInt32(((double)completed_files_ct / (double)fileCt) * 100).ToString() + " %";
                                lblProgressBar.Text = "Analyzing...";
                                break;
                            }

                        case "-d-":     //Image flagged as detected
                            {
                                completed_files_ct++;
                                progressBar1.Visible = true;
                                progressBar1.Minimum = 0;
                                //progressBar1.Maximum = total;
                                progressBar1.Value = Convert.ToInt32(((double)completed_files_ct / (double)fileCt) * 100);
                                progressBar1.Update();
                                lblPercent.Text = Convert.ToInt32(((double)completed_files_ct / (double)fileCt) * 100).ToString() + " %";
                                lblProgressBar.Text = "Analyzing...";
                                break;
                            }

                        case "-o-":     //Image flagged as other
                            {
                                completed_files_ct++;
                                progressBar1.Visible = true;
                                progressBar1.Minimum = 0;
                                //progressBar1.Maximum = total;
                                progressBar1.Value = Convert.ToInt32(((double)completed_files_ct / (double)fileCt) * 100);
                                progressBar1.Update();
                                lblPercent.Text = Convert.ToInt32(((double)completed_files_ct / (double)fileCt) * 100).ToString() + " %";
                                lblProgressBar.Text = "Analyzing...";

                                break;
                            }

                        case "-f-":     //Backend has finished
                            {
                                lblProgressBar.Text = "Finished...";
                                break;
                            }

                        case "-e-":     //An error in the backend has occurred
                            {
                                lblProgressBar.Text = "Error Detected...";
                                break;
                            }

                        default:        //Backend has printed something unexpected/multiple print statements following an above case
                            {
                                //infoLog.Text += "An unexpected string has been detected...";
                                break;
                            }
                    }

                    //Update infoLog
                    //infoLog.Text = infoLogStr;
                    infoLog.AppendText(status + "\r\n");
                }
            }
            catch
            {

            }
           
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public void redirectHandler(object sendingProcess, DataReceivedEventArgs line)
        {
            // Collect the sort command output. 
            if (!string.IsNullOrEmpty(line.Data))
            {
                infoLogStr += line.Data + "\r\n";
                try
                {
                    Invoke(new Change(OnChange), line.Data, completed_files_ct, fileCt);
                }
                catch
                {

                }
            }
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        public string getCurrentBatchPath()
        {
            return currentBatch;
        }

        //===================================================================================================================
        //-------------------------------------------------------------------------------------------------------------------
        //===================================================================================================================

        //Kill backend process if ProcessForm is closed
        private void ProcessForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                if( backendProcess != null && !backendProcess.HasExited )
                    backendProcess.Kill();
            }
            catch
            {

            }
        }


       

    }
}
