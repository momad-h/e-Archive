using Lab_Archive;
using Lab_Archive.services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace e_Archive
{
    public partial class Form1 : Form
    {
        IPublicServices services;
        public Form1()
        {
            InitializeComponent();
            services = new PublicServices();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigInfo.ConnectionStr = ConfigurationManager.AppSettings["ConnectionStr"];
            ConfigInfo.FarzinUrl = ConfigurationManager.AppSettings["FarzinUrl"];
            ConfigInfo.FarzinUsername = ConfigurationManager.AppSettings["FarzinUsername"];
            ConfigInfo.FarzinPassword = ConfigurationManager.AppSettings["FarzinPassword"];
            ConfigInfo.MaxDegreeOfParallelism = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDegreeOfParallelism"]);
            ConfigInfo.NumberOfRecordsFetched = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfRecordsFetched"]);
            ConfigInfo.Starter = Convert.ToInt32(ConfigurationManager.AppSettings["Starter"]);
            ConfigInfo.WFID = Convert.ToInt32(ConfigurationManager.AppSettings["WFID"]);
            ConfigInfo.SubFormFileFieldName = ConfigurationManager.AppSettings["SubFormFileFieldName"];
        }

        private async void btnAddToArchive_Click(object sender, EventArgs e)
        {
            FileBrowser browser=new FileBrowser();
            try
            {
                lblTotal.Text = services.CountFiles(txtRootPath.Text).ToString();
                await Task.Run(() =>
                {
                    browser.ProcessMainFoldersParallel(txtRootPath.Text, ConfigInfo.MaxDegreeOfParallelism);
                    UpdateResult(browser.Counter.ToString());
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async void btnAddSubDocuments_Click(object sender, EventArgs e)
        {
            SubDocumentManagement subDocumentManagement = new SubDocumentManagement();
            try
            {
                string counter = "0";
                await Task.Run(() =>
                {
                    subDocumentManagement.SubForm_InsertDocument();
                    UpdateResult(counter);
                    MessageBox.Show("پردازش زیرفرم ها پایان یافت");
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UpdateResult(string text)
        {
            if (lblResult.InvokeRequired)
            {
                lblResult.Invoke((Action)(() => lblResult.Text = text));
            }
            else
            {
                lblResult.Text = text;
            }
        }

    }
}
