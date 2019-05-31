using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RouteShow
{
    public partial class FormEdit : Form
    {
        public bool goEdit;

        public FormEdit()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            goEdit = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            goEdit = false;
            Close();
        }
    }
}
