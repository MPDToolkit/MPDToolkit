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
        }

        private void checkForPython()
        {

            Console.Out.WriteLine("-------------Running POST-------------");

            //Check for Python
            if (!string.IsNullOrEmpty(settings.PythonPath) && File.Exists(settings.PythonPath))
            {
                //Check/Install required packages
                Console.WriteLine("...Checking Python Version...");
                string python_version = Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\checkPythonVersion.bat"), CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }).StandardOutput.ReadToEnd();
                if (python_version.Contains("Python 3.6.4")) Console.WriteLine("Python Version --> OK");
                else Console.WriteLine("Python Version 3.6.4 Required!");

                Console.WriteLine("...Checking Python Packages...");
                string pip_list = Process.Start(new ProcessStartInfo { FileName = Path.Combine(Environment.CurrentDirectory, "bin\\Setup\\checkPythonPackages.bat"), CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true }).StandardOutput.ReadToEnd();
                string[] required_pkgs = { "opencv-python", "numpy", "scipy", "scikit-learn", "spectral", "pyparsing", "matplotlib" };
                foreach (string str in required_pkgs)
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
                settings.FirstRun = false;
                UpdateSettings();
            }
            else
            {
                //Error: No python installation found
                MessageBox.Show("Image analysis requires Python 3 to be installed.", "Missing Python", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 0, "https://www.google.com");
                //this.menuBtnNewAnalysis.Enabled = false;
            }

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
