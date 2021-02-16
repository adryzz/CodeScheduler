using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Octokit;

namespace CodeScheduler
{
    public partial class PluginUpdater : Form
    {
        string URL;

        public PluginUpdater(string url)
        {
            InitializeComponent();
            URL = url;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PluginUpdater_Shown(object sender, EventArgs e)
        {
            
        }

        void UpdatePackage()
        {
            var github = new GitHubClient(new ProductHeaderValue("plugin-updater"));
            string data = URL;
            data.Replace("https://github.com/", "");
            string owner = data.Substring(0, data.IndexOf('/'));
            string repo = data.Substring(data.IndexOf('/'));
            Repository r = github.Repository.Get(owner, repo).Result;
            if (r.HasDownloads)
            {
                //Release re = r.Releases[0];
            }
        }
    }
}
