using System;
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

namespace AnamolyDetector
{


    public partial class ProcessForm : Form
    {
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


        private List<string> batch_names = new List<string>();

        public ProcessForm(Settings settings)
        {
            InitializeComponent();

            workingDirectory = Environment.CurrentDirectory;
            //batchesDirectory = Path.Combine(workingDirectory, "..\\Batches");     //Use this if we are using an installer
            batchesDirectory = Path.Combine(workingDirectory, "Batches");

            pythonPath = settings.PythonPath;

        }


        private string getNextBatchName()
        {
            string[] batch_paths = Directory.GetDirectories(batchesDirectory);
            int ct = 0;

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

            ct++;

            return "Batch_" + ct.ToString();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            batchName bn = new batchName(getNextBatchName());
            if (bn.ShowDialog() == DialogResult.OK)
            {
                string batchName = bn.getText();
                
                currentBatch = batchesDirectory + "\\" + batchName;

                copyDir = batchesDirectory + "\\" + batchName + "\\Copy";
                detDir = batchesDirectory + "\\" + batchName + "\\Detected";
                othDir = batchesDirectory + "\\" + batchName + "\\Other";

                //Update the window title
                this.Text = batchName;

                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog1.Filter = "image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Multiselect = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog1.FileNames.Length > 1000)
                    {
                        if (MessageBox.Show("It is not recommended to run more than 1000 files. Speed is not guaranteed.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;          
                    }

                    int i = 0;
                    if (Directory.Exists(currentBatch))
                    {
                        i++;
                        while (Directory.Exists(currentBatch + " (" + i.ToString() + ")"))
                        {
                            i++;
                        }         
                         currentBatch = currentBatch + " (" + i.ToString() + ")";
                    }


                    //Only create the directories when a folder has been selected
                    Directory.CreateDirectory(currentBatch);
                    Directory.CreateDirectory(copyDir);
                    Directory.CreateDirectory(detDir);
                    Directory.CreateDirectory(othDir);

                    File.Create(currentBatch + @"\batch_log.txt").Close();
                    File.Create(currentBatch + @"\checkbox.ini").Close();

                    string path;
                    foreach (string file in openFileDialog1.FileNames)
                    {
                        if (file.ToLower().EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png"))
                        {
                            path = Path.Combine(copyDir, Path.GetFileName(file));
                            File.Copy(file, path, true);
                            fileCt++;
                        }
                    }

                    filesSelected.Text = "Files Selected: " + fileCt;
                    lblProgressBar.Text = "Ready to analyze...";
                    lblProgressBar.Update();
                }
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            batchName bn = new batchName(getNextBatchName());
            if (bn.ShowDialog() == DialogResult.OK)
            {

                String batchName = bn.getText();
                currentBatch = batchesDirectory + "\\" + batchName;

                copyDir = batchesDirectory + "\\" + batchName + "\\Copy";
                detDir = batchesDirectory + "\\" + batchName + "\\Detected";
                othDir = batchesDirectory + "\\" + batchName + "\\Other";

                //Update the window title
                this.Text = batchName;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    String[] FileNames = Directory.GetFiles(folderBrowserDialog1.SelectedPath);

                    if (FileNames.Length > 1000)
                    {
                        if (MessageBox.Show("It is not recommended to run more than 1000 files. Speed is not guaranteed.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                    }

                    int i = 0;
                    if (Directory.Exists(currentBatch))
                    {
                        i++;
                        while (Directory.Exists(currentBatch + " (" + i.ToString() + ")"))
                        {
                            i++;
                        }
                        currentBatch = currentBatch + " (" + i.ToString() + ")";
                    }

                    //Only create the directories when a folder has been selected
                    Directory.CreateDirectory(currentBatch);
                    Directory.CreateDirectory(copyDir);
                    Directory.CreateDirectory(detDir);
                    Directory.CreateDirectory(othDir);

                    File.Create(currentBatch + @"\batch_log.txt").Close();
                    File.Create(currentBatch + @"\checkbox.ini").Close();

                    string path;
                    foreach (string file in FileNames)
                    {
                        if (file.ToLower().EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png"))
                        {
                            path = Path.Combine(copyDir, Path.GetFileName(file));
                            File.Copy(file, path, true);
                            fileCt++;
                        }
                    }

                    //Update labels on the form
                    filesSelected.Text = "Files Selected: " + fileCt;
                    lblProgressBar.Text = "Ready to analyze...";
                    lblProgressBar.Update();
                }
            }
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            //Disable buttons
            btnSelectFolder.Enabled = false;
            btnSelectFile.Enabled = false;
            btnAnalyze.Enabled = false;

            //Inform user that images are being analyzed
            lblProgressBar.Text = "Analyzing...";
            lblProgressBar.Update();

            int num_threads = 1;
            string pythonArgs = workingDirectory + @"\bin\analyze.py -F " + currentBatch + " -p " + num_threads.ToString();

            if(currentBatch != null)
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
                //backendProcess.EnableRaisingEvents = true;

                //Create a background thread for the progress bar 
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(run_analyze);
                worker.RunWorkerAsync(this);
                
            }
        }

        private void run_analyze(object sender, DoWorkEventArgs e)
        {
            //Start the python process
            backendProcess.Start();
            backendProcess.BeginOutputReadLine();
            backendProcess.BeginErrorReadLine();

            //Updates progress bar 
            while (!backendProcess.HasExited)
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Change(OnChange), "Analyzing...", completed_files_ct, fileCt);
                    }

                    //Wait a bit to reduce cpu load
                    Thread.Sleep(100);
                }
                catch
                {
                    Console.Out.WriteLine("Error");
                    return;
                }
                
            }

            //Finalize the progress bar and percentage
            try
            {
                Invoke(new Change(OnChange), null, completed_files_ct, fileCt);
            }
            catch
            {
                Console.Out.WriteLine("Error");
                return;
            }
            
            
        }

        // this code updates the status while a background thread works
        private delegate void Change(string status, int complete, int total);
        private void OnChange(string status, int complete, int total)
        {
            if (status == null)
            {
                
                lblProgressBar.Text = "Finished...";
                //progressBar1.Visible = false;
                //progressBar1.Value = 0;
            }
            else
            {
                progressBar1.Visible = true;
                progressBar1.Minimum = 0;
                //progressBar1.Maximum = total;
                progressBar1.Value = Convert.ToInt32(((double)completed_files_ct / (double)fileCt) * 100);
                progressBar1.Update();
                lblPercent.Text = Convert.ToInt32(((double)completed_files_ct / (double)fileCt) * 100).ToString() + " %";
                lblProgressBar.Text = status;

                //Update infoLog
                infoLog.Text = infoLogStr;

            }
        }

        public void redirectHandler(object sendingProcess, DataReceivedEventArgs line)
        {
            // Collect the sort command output. 
            if (!string.IsNullOrEmpty(line.Data))
            {
                completed_files_ct++;
                infoLogStr += line.Data + "\r\n";
            }
        }

        public string getCurrentBatchPath()
        {
            return currentBatch;
        }
    }
}
