using Lab_Archive;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigInfo.ConnectionStr = ConfigurationManager.AppSettings["ConnectionStr"];
            ConfigInfo.FarzinUrl = ConfigurationManager.AppSettings["FarzinUrl"];
            ConfigInfo.FarzinUsername = ConfigurationManager.AppSettings["FarzinUsername"];
            ConfigInfo.FarzinPassword = ConfigurationManager.AppSettings["FarzinPassword"];
        }
    }
}
