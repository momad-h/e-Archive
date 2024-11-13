using Lab_Archive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace e_Archive
{
    public partial class Form1 : Form
    {
        IPublicServices services;
        System.Timers.Timer timer;
        SubDocumentManagement subDocumentManagement;
        FileBrowser browser;
        DocumentManagement documentManagement;
        ProccessByPersonelID proccess;
        ListViewGroup errorGroup;
        string btnEvent = "";
        public Form1()
        {
            InitializeComponent();
            ConfigInfo.ConnectionStr = ConfigurationManager.AppSettings["ConnectionStr"];
            ConfigInfo.FarzinUrl = ConfigurationManager.AppSettings["FarzinUrl"];
            ConfigInfo.FarzinUsername = ConfigurationManager.AppSettings["FarzinUsername"];
            ConfigInfo.FarzinPassword = ConfigurationManager.AppSettings["FarzinPassword"];
            ConfigInfo.MaxDegreeOfParallelism = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDegreeOfParallelism"]);
            ConfigInfo.NumberOfRecordsFetched = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfRecordsFetched"]);
            ConfigInfo.Starter = Convert.ToInt32(ConfigurationManager.AppSettings["Starter"]);
            ConfigInfo.WFID = Convert.ToInt32(ConfigurationManager.AppSettings["WFID"]);
            ConfigInfo.SubFormFileFieldName = ConfigurationManager.AppSettings["SubFormFileFieldName"];
            ConfigInfo.WhereConditionFieldName = ConfigurationManager.AppSettings["WhereConditionFieldName"];
            ConfigInfo.MasterETC = Convert.ToInt32(ConfigurationManager.AppSettings["MasterETC"]);
            ConfigInfo.SlaveETC = Convert.ToInt32(ConfigurationManager.AppSettings["SlaveETC"]);
            services = new PublicServices();
            browser = new FileBrowser();
            subDocumentManagement = new SubDocumentManagement();
            documentManagement = new DocumentManagement();
            proccess = new ProccessByPersonelID();
            timer = new System.Timers.Timer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblLog.Visible=false;
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Tick);

            errorGroup = new ListViewGroup("خطا", HorizontalAlignment.Left);
            lstLogs.Groups.Add(errorGroup);
        }

        private async void btnAddToArchive_Click(object sender, EventArgs e)
        {
            timer.Start();
            btnEvent = "btnAddToArchive";
            try
            {
                lblTotal.Text = services.CountFiles(txtRootPath.Text).ToString();
                await Task.Run(() =>
                {
                    browser.ProcessMainFoldersParallel(txtRootPath.Text, ConfigInfo.MaxDegreeOfParallelism);
                    timer.Stop();
                    MessageBox.Show("انتقال اطلاعات به پایگاه داده به اتمام رسید");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (btnEvent)
                {
                    case "btnAddToArchive": UpdateResult(browser.Counter1.ToString()); break;
                    case "btnAddSubDocuments": UpdateResult(subDocumentManagement.Counter.ToString()); break;
                    case "btnAddMainDocuments": UpdateResult(documentManagement.Counter.ToString()); break;
                    case "btnProccessByPersonelID": UpdateResult(proccess.Counter.ToString()); break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void btnAddSubDocuments_Click(object sender, EventArgs e)
        {

            try
            {
                lblTotal.Text = subDocumentManagement.TotalCounter.ToString();
                btnEvent = "btnAddSubDocuments";
                timer.Start();
                await Task.Run(() =>
                {
                    subDocumentManagement.SubForm_InsertDocument();
                    timer.Stop();
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
        public void UpdateProgress(string text)
        {
            if (lblProgress.InvokeRequired)
            {
                lblProgress.Invoke((Action)(() => lblProgress.Text = text));
            }
            else
            {
                lblProgress.Text = text;
            }
        }
        public void UpdateTotal(string text)
        {
            if (lblTotal.InvokeRequired)
            {
                lblTotal.Invoke((Action)(() => lblTotal.Text = text));
            }
            else
            {
                lblTotal.Text = text;
            }
        }
        public void UpdateList(string text)
        {
            ListViewItem item = new ListViewItem(text);
            item.Group = errorGroup;

            if (lstLogs.InvokeRequired)
            {
                lstLogs.Invoke((Action)(() => lstLogs.Items.Add(item)));
            }
            else
            {
                lstLogs.Items.Add(item);
            }
        }
        private async void btnAddMainDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                lblTotal.Text = documentManagement.TotalCounter.ToString();
                btnEvent = "btnAddMainDocuments";
                timer.Start();
                await Task.Run(() =>
                {
                    documentManagement.MainForm_InsertDocument();
                    timer.Stop();
                    MessageBox.Show("پردازش فرم های اصلی پایان یافت");
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async void btnProccessByPersonelID_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRootPath.Text == "")
                {
                    MessageBox.Show("لطفا مسیر ریشه را وارد کنید");
                }
                else
                {
                    lblLog.Visible = true;
                    btnProccessByPersonelID.Enabled = false;
                    lblProgress.Text = string.Empty;
                    lstLogs.Items.Clear();
                    foreach (DataRow item in services.GetPersonalCodStrFromUsers().Rows)
                    {
                        await Run(item["PersonalID"].ToString());
                    }
                    btnProccessByPersonelID.Enabled = true;
                    lblProgress.Text = "پردازش فایل ها پایان یافت";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                btnProccessByPersonelID.Enabled = true;
            }
        }
        async Task Run(string pid)
        {
            try
            {
                //lblTotal.Text = documentManagement.TotalCounter.ToString();
                lblProgress.Text = "در حال پردازش اطلاعات پرسنل :\"" + pid + "\"";
                btnEvent = "btnProccessByPersonelID";
                timer.Start();
                await Task.Run(() =>
                {
                    proccess.ProcessFolder(txtRootPath.Text, pid);
                    UpdateTotal(proccess.Counter.ToString());
                    timer.Stop();
                });
            }
            catch (Exception ex)
            {
                UpdateList(ex.Message);
            }
        }

        private void lstLogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void lstLogs_DoubleClick(object sender, EventArgs e)
        {
        }

        private void lblLog_Click(object sender, EventArgs e)
        {
            try
            {

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.txt");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                foreach (ListViewItem item in lstLogs.Items)
                {
                    string line = string.Join("\t", item.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(subItem => subItem.Text));
                    File.AppendAllText(filePath, line+Environment.NewLine);
                }

                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
