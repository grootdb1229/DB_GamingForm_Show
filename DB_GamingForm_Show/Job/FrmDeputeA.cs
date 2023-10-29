using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_GamingForm_Show.Job
{
    public partial class FrmDeputeA : Form
    {
        public FrmDeputeA()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmDeputeA f= new FrmDeputeA();
            f.Close();
        }
    }
}
