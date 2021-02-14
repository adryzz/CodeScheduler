using CodeScheduler.Logging;
using CodeScheduler.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeScheduler
{
    public partial class MainWindow : Form
    {
        LogSeverity LocalVerbosity;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            foreach(Plugin p in Program.Manager.Plugins)
            {
                listBox1.Items.Add(p.AssemblyName);
            }
            comboBox1.SelectedIndex = (int)Logger.Verbosity;
            LocalVerbosity = Logger.Verbosity;
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
                groupBox1.Visible = true;
            }
            else
            {
                groupBox1.Visible = false;
            }
            listBox1_SelectedIndexChanged(sender, e);
            ReloadLog();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                groupBox1.Visible = false;
                return;
            }
            Plugin selected = Program.Manager.Plugins[listBox1.SelectedIndex];

            label1.Text = $"Name: {selected.AssemblyName}\n\nDescription: {selected.AssemblyDescription}\n\nVersion: {selected.AssemblyVersion}\n\nLicense: {selected.AssemblyLicense}\n\nLoaded: {selected.Loaded}";

            button1.Enabled = !string.IsNullOrEmpty(selected.ConfigName);

            button2.Enabled = !string.IsNullOrEmpty(selected.AssemblyRepositoryURL);

            button3.Enabled = !string.IsNullOrEmpty(selected.AssemblyWikiURL);

            checkBox1.Checked = selected.Enabled;
            checkBox1.Enabled = !string.IsNullOrEmpty(selected.ConfigName);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Program.Manager.Plugins[listBox1.SelectedIndex].ConfigName))
            {
                Program.Manager.Plugins[listBox1.SelectedIndex].Enabled = checkBox1.Checked;
                Program.Manager.Plugins[listBox1.SelectedIndex].Save();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("This process is irreversible. are you sure you wanna uninstall the plugin?", "CodeScheduler", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                System.IO.File.Delete(Utils.GetAbsolutePath(Program.PluginFolder, Program.Manager.Plugins[listBox1.SelectedIndex].ConfigName));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented yet", "CodeScheduler", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Program.Manager.Plugins[listBox1.SelectedIndex].AssemblyWikiURL);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Are you sure you wanna reload the plugin?", "CodeScheduler", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Program.Manager.ReloadPlugin(Program.Manager.Plugins[listBox1.SelectedIndex]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocalVerbosity = (LogSeverity)comboBox1.SelectedIndex;
            ReloadLog();
        }

        void ReloadLog()
        {
            listView1.SuspendLayout();
            listView1.Items.Clear();
            foreach(LogData d in Logger.LoggedData)
            {
                if ((int)d.Severity >= (int)LocalVerbosity)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { d.Severity.ToString(), d.Time.ToString(), d.Category, d.Message }));
                }
            }
            listView1.ResumeLayout();
        }
    }
}
