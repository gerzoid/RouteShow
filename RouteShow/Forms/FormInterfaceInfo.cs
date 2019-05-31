using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace RouteShow.Forms
{
    public partial class FormInterfaceInfo : Form
    {
        DataTable table;
        int countRowsInTable=0;
        public FormInterfaceInfo()
        {
            InitializeComponent();
            table = new DataTable();
            table.Columns.Add("Key", Type.GetType("System.String"));
            table.Columns.Add("Value", Type.GetType("System.String"));
            grid.DataSource = table;
            grid.Columns[1].Width = 230;
            grid.Columns[0].Width = 150;
            grid.Columns[1].ReadOnly = true;
            grid.Columns[0].ReadOnly = true;
            grid.Columns[0].DefaultCellStyle.Font = new Font(grid.DefaultCellStyle.Font, FontStyle.Bold);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormInterfaceInfo_Shown(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.route;
            for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
            {
                comboBox1.Items.Add(Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].Address+" - "+Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName);
            }
            if (Helper.interfaceInfo.Count - 1 >= 0)
            {
                comboBox1.Text = Convert.ToString(Helper.interfaceInfo[0].Index) + " - " + Helper.interfaceInfo[0].Address + " - " + Helper.interfaceInfo[0].InterfaceDescription + " " + Helper.interfaceInfo[0].InterfaceName;
                FillTable(0);
            }
        }

        private void AddToTableValue(string name, object value)
        {
            table.Rows.Add();
            table.Rows[table.Rows.Count - 1][0] = name;
            table.Rows[table.Rows.Count - 1][1] = value;
        }

        private void AddToTableIPAddress(string name, IPAddressCollection value)
        {
            for (int x = 0; x <= value.Count - 1; x++)
            {
                if (!value[x].IsIPv6SiteLocal)
                {
                    try
                    {
                        table.Rows.Add();
                        table.Rows[table.Rows.Count - 1][0] = name;
                        table.Rows[table.Rows.Count - 1][1] = new System.Net.IPAddress(value[x].Address);
                    }
                    catch (Exception E)
                    {
                    }
                }
            }
        }

        private void AddToTableIPGatewyAddress(string name, GatewayIPAddressInformationCollection value)
        {
            for (int x = 0; x <= value.Count - 1; x++)
            {
                table.Rows.Add();
                table.Rows[table.Rows.Count - 1][0] = name;
                table.Rows[table.Rows.Count - 1][1] = value[x].Address;
            }
        }


        public void FillTable(int index)
        {
            countRowsInTable = 0;
            table.Rows.Clear();
            AddToTableValue("Index", Helper.interfaceInfo[index].Index);
            AddToTableValue("Interface Name", Helper.interfaceInfo[index].InterfaceName);
            AddToTableValue("Interface Desription", Helper.interfaceInfo[index].InterfaceDescription);
            AddToTableValue("Interface ID",Helper.interfaceInfo[index].RelatedInterface.Id);
            AddToTableValue("MAC", Helper.interfaceInfo[index].RelatedInterface.GetPhysicalAddress());
            AddToTableValue("Address", Helper.interfaceInfo[index].Address);
            AddToTableValue("Mask", Helper.interfaceInfo[index].Mask);
            AddToTableValue("Network Interface Type", Helper.interfaceInfo[index].RelatedInterface.NetworkInterfaceType);
            AddToTableValue("Speed", Helper.interfaceInfo[index].RelatedInterface.Speed);
            AddToTableValue("Flags", Helper.interfaceInfo[index].Flags);
            if (Helper.interfaceInfo[index].RelatedInterface.Supports(NetworkInterfaceComponent.IPv4))
            {
                AddToTableIPAddress("DhcpServerAddresses", Helper.interfaceInfo[index].RelatedInterface.GetIPProperties().DhcpServerAddresses);
                AddToTableIPAddress("DnsAddresses", Helper.interfaceInfo[index].RelatedInterface.GetIPProperties().DnsAddresses);
                AddToTableValue("DnsSuffix", Helper.interfaceInfo[index].RelatedInterface.GetIPProperties().DnsSuffix);
                AddToTableIPGatewyAddress("GatewayAddresses", Helper.interfaceInfo[index].RelatedInterface.GetIPProperties().GatewayAddresses);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillTable(comboBox1.SelectedIndex);
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void FormInterfaceInfo_Resize(object sender, EventArgs e)
        {
            grid.Width = Width - 30;
            grid.Height = Height - 120;
            button1.Location  =  new Point(Convert.ToInt32(Width / 2 - 50), Height -60);
            comboBox1.Width = Width - 85;
        }
    }
}
