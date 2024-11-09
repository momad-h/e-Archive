using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab_Archive;

namespace e_Archive
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            ServiceManager sv = new ServiceManager();
            txtRes.Text = sv.Start_Service(1, 2, 3).ToString();


        }
    }
}
