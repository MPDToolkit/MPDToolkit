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

namespace conceptTest
{
    public partial class MainForm : Form
    {
        public static string user_profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string working_directory_path = Environment.CurrentDirectory;
        public static string analyze_path = Path.Combine(working_directory_path, "dist\\analyze\\analyze.exe");
        public List<Process> process_list = new List<Process>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            //Clear the txtbxLog text
           // txtbxLog.Clear();

            //Send txtbxArgv value to the executable
            Console.Out.WriteLine(user_profile);
            Console.Out.WriteLine(working_directory_path);
            Console.Out.WriteLine(analyze_path);

            //Create the process
            ProcessStartInfo start_config = new ProcessStartInfo(analyze_path, txtbxArgv.Text);
            start_config.UseShellExecute = false;
            start_config.RedirectStandardOutput = true;     //Need to disable 'UseShellExecute' first, before enabling this option
            start_config.RedirectStandardError = true;
            start_config.CreateNoWindow = true;     //Don't open command line
            
            process_list.Add(Process.Start(start_config));

            //Print the last element
            var t_start = process_list.Last<Process>().StartTime;      //Begin async read of redirected output stream

            txtbxLog.Text += process_list.Last<Process>().StandardOutput.ReadToEnd();   //Async read of redirected output stream

            var t_end = process_list.Last<Process>().ExitTime;

            //txtbxLog.Text += "Elapsed Time: " + ((float)t_end.Millisecond - (float)t_start.Millisecond).ToString() + " ms";
            txtbxLog.Text += String.Format("Process ID: {0}", process_list.Last<Process>().Id) + System.Environment.NewLine;
            txtbxLog.Text += String.Format("Elapsed time: {0} ms", ((float)t_end.Millisecond - (float)t_start.Millisecond)) + System.Environment.NewLine;
            txtbxLog.Text += String.Format("Process count: {0}", process_list.Count()) + System.Environment.NewLine + System.Environment.NewLine;

        }
    }
}
