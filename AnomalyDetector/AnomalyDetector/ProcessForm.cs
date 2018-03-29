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
using System.Windows.Forms;

namespace AnamolyDetector
{
    public partial class ProcessForm : Form
    {
        private string workingDirectory;
        private string batchesDirectory;
        private string currentBatch;
        private string nextBatchName;   //The next default batch name
        private List<string> batch_names = new List<string>();

        public ProcessForm()
        {
            InitializeComponent();
            workingDirectory = Environment.CurrentDirectory;
            batchesDirectory = Path.Combine(workingDirectory, "..\\Batches");     //Use this if we are using an installer
            //batchesDirectory = Path.Combine(workingDirectory, "\\Batches");
            Console.Out.WriteLine(batchesDirectory);
            
            

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
            //nextBatchName = "Batch_" + ct.ToString();

            return "Batch_" + ct.ToString();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            batchName curr = new batchName(getNextBatchName());
            if (curr.ShowDialog() == DialogResult.OK)
            {
                String batchName = curr.getText();
                
                currentBatch = batchesDirectory + "\\" + batchName;

                String copyDir = batchesDirectory + "\\" + batchName + "\\Copy";
                String detDir = batchesDirectory + "\\" + batchName + "\\Detected";
                String othDir = batchesDirectory + "\\" + batchName + "\\Other";

                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Multiselect = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    if (openFileDialog1.FileNames.Length > 1000)
                    {
                        String text = "It is not recommended to run more than 1000 files. Speed is not guaranteed.";
                        DialogResult result = MessageBox.Show(text, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result.Equals(DialogResult.No))
                        {
                            return;
                        }
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
                        

                    //Only create the directories when a file has been selected
                    Directory.CreateDirectory(currentBatch);
                    Directory.CreateDirectory(copyDir);
                    Directory.CreateDirectory(detDir);
                    Directory.CreateDirectory(othDir);

                    String path;
                    int fileCt = 0;


                    foreach (String file in openFileDialog1.FileNames)
                    {
                        if (file.ToLower().EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png"))
                        {
                            path = Path.Combine(copyDir, Path.GetFileName(file));
                            System.IO.File.Copy(file, path, true);
                            fileCt++;
                        }
                    }

                    filesSelected.Text = "Files Selected: " + fileCt;
                }
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            batchName curr = new batchName(getNextBatchName());
            if (curr.ShowDialog() == DialogResult.OK)
            {

                String batchName = curr.getText();
                currentBatch = batchesDirectory + "\\" + batchName;

                String copyDir = batchesDirectory + "\\" + batchName + "\\Copy";
                String detDir = batchesDirectory + "\\" + batchName + "\\Detected";
                String othDir = batchesDirectory + "\\" + batchName + "\\Other";


                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    String[] FileNames = Directory.GetFiles(folderBrowserDialog1.SelectedPath);

                    if (FileNames.Length > 1000)
                    {
                        String text = "It is not recommended to run more than 1000 files. Speed is not guaranteed.";
                        DialogResult result = MessageBox.Show(text, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result.Equals(DialogResult.No))
                        {
                            return;
                        }
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

                    String path;

                    int fileCt = 0;
                    foreach (String file in FileNames)
                    {
                        if (file.ToLower().EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png"))
                        {
                            path = Path.Combine(copyDir, Path.GetFileName(file));
                            System.IO.File.Copy(file, path, true);
                            fileCt++;
                        }
                    }

                    filesSelected.Text = "Files Selected: " + fileCt;
                }
            }
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            String backendPath = workingDirectory + "\\bin\\dist\\analyze\\analyze.exe";
            String backendArgs = "-F " + currentBatch;

            if(currentBatch != null)
            {
                ProcessStartInfo startConfig = new ProcessStartInfo(backendPath, backendArgs);
                startConfig.UseShellExecute = false;
                startConfig.RedirectStandardOutput = true;
                startConfig.RedirectStandardError = false;
                startConfig.CreateNoWindow = true;

                Process backendProcess = new Process();
                backendProcess = Process.Start(startConfig);

               

                //Progess bar updates
                List<String> completedFiles = new List<string>();
                while (!backendProcess.HasExited)
                {
                    
                }



            }
        }

        public String getCurrentBatchPath()
        {
            return currentBatch;
        }
    }
}
