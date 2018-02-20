using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace testGUI
{
    public partial class MainForm : Form
    {

        public static string user_profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string working_directory_path = Environment.CurrentDirectory;

        public static string source_directory = Path.Combine(working_directory_path, "Sources");
        public static string result_directory = Path.Combine(working_directory_path, "Results");

        public static string analyze_path = Path.Combine(working_directory_path, "dist\\analyze\\analyze.exe");

        public List<Process> process_list = new List<Process>();

        string[] dir_files;

        public MainForm()
        {
            InitializeComponent();

        }

        private void menuItem_NewJob_Click(object sender, EventArgs e)
        {
            //Open dialog window to collect job info
            JobForm jf = new JobForm();
            jf.Show();
        }

        private void menuItem_OpenResults_Click(object sender, EventArgs e)
        {
            //Set default folder path
            folderBrowserDialog_Results.SelectedPath = result_directory;

            //Open folder browser dialog
            folderBrowserDialog_Results.ShowDialog();

            //Read the selected folder path
            dir_files = Directory.GetFiles(folderBrowserDialog_Results.SelectedPath);
            foreach(string str in dir_files)
            {
                checkedListBox1.Items.Add(str);
            }
            

        }
    }
}
