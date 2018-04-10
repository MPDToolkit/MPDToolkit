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

namespace AnomalyDetector
{
    public partial class PythonCheckForm : Form
    {
        private Settings settings;

        public PythonCheckForm(Settings set)
        {
            InitializeComponent();
            settings = set;
            checkForPython();
        }

        private void checkForPython()
        {

            //Console.Out.WriteLine("-------------Running POST-------------");

            //Check for Python
            if (string.IsNullOrEmpty(settings.PythonPath) || !File.Exists(settings.PythonPath))
            {
                //Check/Install required packages
                infoLabel.Text = "Checking Python version...";
                string python_version = Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\checkPythonVersion.bat"), CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }).StandardOutput.ReadToEnd();
                if (python_version.Contains("Python 3.6.4"))
                {
                    infoLabel.Text = "Checking Python version......OK";
                }
                else
                {
                    infoLabel.Text = "Python 3.6.4 is required. Please install the correct version.";
                    this.DialogResult = DialogResult.No;     
                }

                //Console.WriteLine("...Checking Python Packages...");
                infoLabel.Text = "Checking Python packages...";
                string pip_list = Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\checkPythonPackages.bat"), CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }).StandardOutput.ReadToEnd();
                string[] required_pkgs = { "opencv-python", "numpy", "scipy", "scikit-learn", "spectral", "pyparsing", "matplotlib" };
                foreach (string str in required_pkgs)
                {
                    if (!pip_list.Contains(str))
                    {
                        infoLabel.Text = "Installing Python packages...";
                        Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\installPythonPackages.bat"), CreateNoWindow = true, UseShellExecute = false }).WaitForExit();
                        break;
                    }
                }
                infoLabel.Text = "Python Setup Complete...";
                settings.FirstRun = false;
                UpdateSettings();

                this.DialogResult = DialogResult.OK;   
            }
            else
            {
                infoLabel.Text = "Finished...";
                this.DialogResult = DialogResult.OK;
            }

            //this.Close();
        }

        //===================================================================================================================
        //-----------------------------------------------Update Settings File------------------------------------------------
        //===================================================================================================================

        //Update the settings.ini
        private void UpdateSettings()
        {
            //string[] output = { settings.FirstRun.ToString(), settings.PythonPath, settings.BatchesPath, settings.BinPath };
            File.WriteAllLines(@"settings.ini",
                new string[]{
                    "FirstRun=" + settings.FirstRun.ToString(),
                    "RunPOST=" + settings.RunPOST.ToString(),
                    "PythonPath=" + settings.PythonPath,
                    "BatchesPath=" + settings.BatchesPath,
                    "BinPath=" + settings.BinPath,
                    "NewImageWindow=" + settings.NewImageWindow
                });
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
    }
}
