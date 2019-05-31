using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace RouteShow.Forms
{
    public partial class FormCheckRoute : Form
    {
        public FormCheckRoute()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grid.Rows.Clear();
            IPAddress addr;
            if (IPAddress.TryParse(textBox1.Text, out addr))
            {
                Height = 260;
                grid.Rows.Add();
                grid[0, 0].Value = 1;
                grid[1, 0].Value = textBox1.Text;
            }
            else
            {
                try
                {
                    IPHostEntry host = Dns.GetHostEntry(textBox1.Text);
                    Height = 260;
                    for (int x = 0; x <= host.AddressList.Length - 1; x++)
                    {
                        grid.Rows.Add();
                        grid[0, x].Value = x + 1;
                        grid[1, x].Value = host.AddressList[x];                     
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Ошибка  - "+ee.Message);
                }
            }
            for (int x = 0; x <= grid.Rows.Count - 1; x++)
            {
                IPAddress addres;
                IPAddress.TryParse(grid[1, x].Value.ToString(), out addres);
                grid[2, x].Value = Helper.GetBestRoute(addres);
            }
        }

        private void FormCheckRoute_Shown(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.route;
            Height = 86;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
