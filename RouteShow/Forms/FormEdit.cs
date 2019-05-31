using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Runtime.InteropServices;

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
            IPAddress addr;            
            if (!IPAddress.TryParse(tbDest.Text, out addr))
            {
                MessageBox.Show("Не корреткное значение IP адреса. Корректный формат xxx.xxx.xxx.xxx");
                tbDest.Focus();
                return;
            }
            else
                if (!IPAddress.TryParse(tbMask.Text, out addr))
                {
                    MessageBox.Show("Не корреткное значение IP адреса. Корректный формат xxx.xxx.xxx.xxx");
                    tbMask.Focus();
                    return;
                }
                else
                    if (!IPAddress.TryParse(tbHop.Text, out addr))
                    {
                        MessageBox.Show("Не корреткное значение IP адреса. Корректный формат xxx.xxx.xxx.xxx");
                        tbHop.Focus();
                        return;
                    }

            goEdit = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            goEdit = false;
            Close();
        }

        private void FormEdit_Shown(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.route;
        }

        private void cbPersistent_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
