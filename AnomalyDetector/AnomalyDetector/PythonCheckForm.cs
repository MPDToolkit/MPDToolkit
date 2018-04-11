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
    public partial class PythonCheckForm : Form
    {
        private Settings settings;
        private bool is_python_installed = false;
        private string[] default_python_path = { "%LocalAppData%\\Programs\\Python\\Python36\\python.exe", "%LocalAppData%\\Programs\\Python\\Python36-32\\python.exe", "%ProgramFiles%\\Python 3.6\\python.exe", "%ProgramFiles(x86)%\\Python 3.6\\python.exe" };

        public PythonCheckForm(Settings set)
        {
            InitializeComponent();
            settings = set;     
        }

        private void PythonCheckForm_Load(object sender, EventArgs e)
        {
            //Create a background thread for the progress bar 
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(checkForPython);
            worker.RunWorkerAsync(this);
        }

        private delegate void Bool(bool state);
        private void SetEnabled(bool state)
        {
            btnClose.Enabled = state;
        }

        private delegate void Info(string str);
        private void UpdateInfo(string str)
        {
            infoLabel.Text = str;
        }

        private void checkForPython(object sender, DoWorkEventArgs e)
        {
           
            //Check for Python
            if (string.IsNullOrEmpty(settings.PythonPath) || !File.Exists(settings.PythonPath))
            {
                //Check/Install required packages
                Invoke(new Info(UpdateInfo), "Checking Python version...");
                string python_version = Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\checkPythonVersion.bat"), CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }).StandardOutput.ReadToEnd();
                if (python_version.Contains("Python 3.6"))
                {
                    Invoke(new Info(UpdateInfo), "Checking Python version......OK");

                    //Console.WriteLine("...Checking Python Packages...");
                    Invoke(new Info(UpdateInfo), "Checking Python packages...");
                    string pip_list = Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\checkPythonPackages.bat"), CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }).StandardOutput.ReadToEnd();
                    string[] required_pkgs = { "opencv-python", "numpy", "scipy", "scikit-learn", "spectral", "pyparsing", "matplotlib" };
                    foreach (string str in required_pkgs)
                    {
                        if (!pip_list.Contains(str))
                        {
                            Invoke(new Info(UpdateInfo), "Installing Python packages...");
                            Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\installPythonPackages.bat"), CreateNoWindow = true, UseShellExecute = false }).WaitForExit();
                            break;
                        }
                    }

                    //Search for python path in the default install locations
                    foreach (string path in default_python_path)
                    {
                        if (File.Exists(Environment.ExpandEnvironmentVariables(path)))
                        {
                            settings.PythonPath = Environment.ExpandEnvironmentVariables(path);
                        }
                    }

                    Invoke(new Info(UpdateInfo), "Python Setup Complete...");
                    settings.FirstRun = false;
                    is_python_installed = true;
                }
                else
                {
                    Invoke(new Info(UpdateInfo), "Python 3.6.4 is required.");
                    is_python_installed = false;
                    
                }      
            }
            else
            {
                Invoke(new Info(UpdateInfo), "Finished...");
                is_python_installed = true;
            }

            //Re-enabled the close button
            Invoke(new Bool(SetEnabled), true);
        }

        private void pythonLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                pythonLink.LinkVisited = true; 
                System.Diagnostics.Process.Start("https://www.python.org/downloads/release/python-364/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            this.DialogResult = (is_python_installed) ? DialogResult.Yes : DialogResult.No;
            this.Close();
        }

        public Settings GetSettings()
        {
            return settings;
        }



    }
}
