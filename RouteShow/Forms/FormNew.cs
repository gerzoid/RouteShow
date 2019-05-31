using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using NetworkPortsLib;
using System.Net.NetworkInformation;

namespace RouteShow.Forms
{
    public partial class FormNew : Form
    {        
        int[] indexArray;
        int[] indexArrayInterface;

        public FormNew()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress dest, mask, hop;
            if (!IPAddress.TryParse(tbDest.Text, out dest))
            {
                MessageBox.Show("Не корреткное значение IP адреса. Корректный формат xxx.xxx.xxx.xxx");
                tbDest.Focus();
                return;
            }
            else
                if (!IPAddress.TryParse(tbMask.Text, out mask))
                {
                    MessageBox.Show("Не корреткное значение IP адреса. Корректный формат xxx.xxx.xxx.xxx");
                    tbMask.Focus();
                    return;
                }
                else
                    if (!IPAddress.TryParse(tbHop.Text, out hop))
                    {
                        MessageBox.Show("Не корректное значение IP адреса. Корректный формат xxx.xxx.xxx.xxx");
                        tbHop.Focus();
                        return;
                    }
                    else
                        if (cbInterfaces.Text == "")
                        {
                            MessageBox.Show("необходимо выбрать интерфейс");
                            cbInterfaces.Focus();
                            return;
                        }

            
            RouteEntry routeTmp = new RouteEntry((uint)dest.Address, (uint)mask.Address, 0, (uint)hop.Address, Helper.interfaceInfo[cbInterfaces.SelectedIndex].RelatedInterface, NetworkPortsLib.Type.ForwardType.Direct, NetworkPortsLib.Type.ForwardProtocol.Static, 0, 0, Convert.ToInt32(nMetric.Value), 0, 0, 0, 0, indexArray[cbInterfaces.SelectedIndex]);
            int iret = Helper.iph.AddRouteEntry(routeTmp);
            if (iret != 0)
            {
                MessageBox.Show("Ошибка - "+Convert.ToString(iret));
            }
 
        }

        private void FormNew_Shown(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.route;
            indexArray = new int[Helper.interfaceInfo.Count];
            for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
            {
                cbInterfaces.Items.Add(Helper.interfaceInfo[y].Address + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName);
                indexArray[y] = Helper.interfaceInfo[y].Index;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {            
        }
    }
}
