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
            FormManagement sv = new FormManagement();
            txtRes.Text = sv.SetSlaveFormInMaster(3644, 1, 3645, 4,"PersonnelInfo_C1").ToString();


        }
    }
}
