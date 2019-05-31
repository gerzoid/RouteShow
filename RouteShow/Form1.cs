using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetworkPortsLib;
using System.Net;
using Microsoft.Win32;
using System.Security.Principal;
using RouteShow.Forms;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace RouteShow
{
    public partial class Form1 : Form
    {
        DataTable routeTable;
        bool admin = false;
        bool enableRoute = false;
        Font myFont;

        public Form1()
        {
            InitializeComponent();
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            myFont = new System.Drawing.Font(this.Font, FontStyle.Bold);
            admin = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
            if (admin)
                toolStripPrava.Text = "Администратор";
            else
                toolStripPrava.Text = "Пользователь";

            try
            {
                RegistryKey readKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\");
                if (readKey == null)
                    return;
                object value = readKey.GetValue("IPEnableRouter");
                if (value != null)
                    if (Convert.ToInt32(value) == 1)
                        enableRoute = true;
            }
            catch (Exception e)
            {
                enableRoute = false;
            }                
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            FormInterfaceInfo formInterfaceInfo = new FormInterfaceInfo();
            formInterfaceInfo.ShowDialog();
            int indexInterface = 0;
            //RouteEntry routeBackup = new RouteEntry((uint)dest.Address, (uint)mask.Address, 0, (uint)hop.Address, Helper.interfaceInfo[cbInterfaces.SelectedIndex].RelatedInterface, NetworkPortsLib.Type.ForwardType.Direct, NetworkPortsLib.Type.ForwardProtocol.Static, 0, 0, Convert.ToInt32(nMetric.Value), 0, 0, 0, 0, indexArray[cbInterfaces.SelectedIndex]);
            MessageBox.Show(Helper.interfaceInfo[1].RelatedInterface.Description);
            
            //GetPersistentRoutes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Helper.Init();
            Helper.FillRouteTable();

            if (!admin)
            {
                addButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                редактироватьToolStripMenuItem.Enabled = false;
                удалитьToolStripMenuItem.Enabled = false;
                добавитьМаршрутToolStripMenuItem.Enabled = false;
                редактироватьМаршрутToolStripMenuItem.Enabled = false;
                удалитьМаршрутToolStripMenuItem.Enabled = false;
                включитьToolStripMenuItem.Enabled = false;
                выключитьToolStripMenuItem.Enabled = false;
            }            

            grid.DataSource = Helper.routeTable;

            grid.Columns["DestIp"].HeaderText = "Сетевой адрес";
            grid.Columns["SubnetMask"].HeaderText = "Маска";
            grid.Columns["NextHop"].HeaderText = "Шлюз";
            grid.Columns["ifindex"].Visible = false;
            grid.Columns["type"].Visible = false;
            grid.Columns["proto"].Visible = false;
            grid.Columns["age"].Visible = false;
            grid.Columns["ProtoText"].HeaderText = "Протокол";
            grid.Columns["ProtoText"].Width = 70;
            grid.Columns["TypeText"].Width = 70;
            grid.Columns["TypeText"].HeaderText = "Тип";
            grid.Columns["AgeText"].HeaderText = "Время жизни";
            grid.Columns["Metric1"].HeaderText = "Метрика";
            grid.Columns["Metric1"].Width = 60;
            grid.Columns["iftext"].HeaderText = "Интерфейс";
            grid.Columns["persistent"].HeaderText = "Постоянный";
            grid.Columns["iftext"].DisplayIndex = 4;
            grid.Columns["N"].DisplayIndex = 0;
            grid.Columns["N"].Width = 30;
            grid.Columns["N"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //RouteEntry re = iph.GetBestRoute(System.Net.IPAddress.Parse("192.168.66.2"));
            //MessageBox.Show(string.Format("Route trouvée:\r\n\r\nIndex interface: {0}\r\nNom interface: {1}\r\nAddr saut suivant: {2}\r\nMasque: {3}",
                //re.Index, GetInterfaceByID(re.Index).Address, re.NextHop, re.Mask), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //lblRouteTableNb.Text = string.Format("{0} entrées", forwardEntryBindingSource.Count);
            //grid.DataSource = forwardEntryBindingSource;
//            grid.Columns.Add("interface", "Интерфейс");
        }



        private void EditRecordInEditForm()
        {
            bool isWasPersistent = false;
            FormEdit formEdit = new FormEdit();
            formEdit.tbDest.Text = grid.SelectedRows[0].Cells[0].Value.ToString();
            formEdit.tbMask.Text = grid.SelectedRows[0].Cells[1].Value.ToString();
            formEdit.tbHop.Text = grid.SelectedRows[0].Cells[2].Value.ToString();
            if (grid.SelectedRows[0].Cells["Persistent"].Value.ToString() == "Да")
            {
                formEdit.cbPersistent.Text = "Да";
                isWasPersistent = true;
            }
            formEdit.nMetric.Value = Convert.ToInt32(grid.SelectedRows[0].Cells[9].Value);
            formEdit.ShowDialog();
            if (formEdit.goEdit)
            {
                RouteEntry routeTmp = Helper.GetRoute(Convert.ToInt32(grid.SelectedRows[0].Cells[12].Value) - 1);
                RouteEntry routeBackup = new RouteEntry((uint)routeTmp.Destination.Address, (uint)routeTmp.Mask.Address, routeTmp.Policy, (uint)routeTmp.NextHop.Address, routeTmp.RelatedInterface, routeTmp.ForwardType, NetworkPortsLib.Type.ForwardProtocol.Static/*  routeTmp.Protocol*/, routeTmp.Age, routeTmp.NextHopAS, routeTmp.Metric1, routeTmp.Metric2, routeTmp.Metric3, routeTmp.Metric4, routeTmp.Metric5, routeTmp.Index);

                int iret = Helper.iph.DeleteRouteTableEntry(routeTmp);
                if (iret != 0)
                {                    
                    MessageBox.Show("Ошибка. Код ошибки - " + Convert.ToString(iret), "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string persistentString = routeTmp.Destination + "," + routeTmp.Mask + "," + routeTmp.NextHop + "," + Convert.ToString(routeTmp.Metric1);
                string persistentStringWithoutMetric = routeTmp.Destination + "," + routeTmp.Mask + "," + routeTmp.NextHop;
                //Проверки на постоянность маршрута
                if (formEdit.cbPersistent.Text == "Да")
                {
                    if (!isWasPersistent)   //Если маршрут не был постоянным
                    {
                        try
                        {
                            RegistryKey writeKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\Tcpip\\Parameters\\PersistentRoutes\\", true);
                            writeKey.SetValue(persistentString, "");
                            grid.SelectedRows[0].Cells["Persistent"].Value = "Да";
                        }
                        catch (Exception E)
                        {
                            MessageBox.Show(E.Message+" Ошибка при установке постоянного маршрута, при записи в реестр System\\CurrentControlSet\\Services\\Tcpip\\Parameters\\PersistentRoutes\\ значения - " + persistentString);
                        }
                        //Добавить в реестр
                    }
                }
                else
                {
                    if (isWasPersistent)    //Если маршрут был пстоянным, то удалим его
                    {
                        RegistryKey writeKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\Tcpip\\Parameters\\PersistentRoutes\\", true);
                        try
                        {
                            writeKey.DeleteValue(persistentString, true);
                            grid.SelectedRows[0].Cells["Persistent"].Value = "Нет";
                        }
                        catch (Exception e)
                        {
                            writeKey.DeleteValue(persistentStringWithoutMetric, false);
                            grid.SelectedRows[0].Cells["Persistent"].Value = "Нет";
                        }
                        //Удалить из реестра
                    }
                }


                routeTmp.Destination = IPAddress.Parse(formEdit.tbDest.Text);
                routeTmp.Mask = IPAddress.Parse(formEdit.tbMask.Text);
                routeTmp.NextHop = IPAddress.Parse(formEdit.tbHop.Text);
                routeTmp.Metric1 = Convert.ToInt32(formEdit.nMetric.Value);
                routeTmp.Protocol = NetworkPortsLib.Type.ForwardProtocol.Static;
                iret = Helper.iph.AddRouteEntry(routeTmp);
                if (iret != 0)
                    Helper.iph.AddRouteEntry(routeBackup);
                //                    int iret = Helper.iph.ChangeRouteEntry(ref routeTmp, 2, ,
                //                       IPAddress.Parse(formEdit.tbMask.Text), NetworkPortsLib.Type.ForwardProtocol.Static, IPAddress.Parse(formEdit.tbHop.Text), Convert.ToInt32(formEdit.nMetric.Value));

                //                    int iret = Helper.iph.ChangeRouteEntry(ref routeTmp,1,IPAddress.Parse(formEdit.tbDest.Text),IPAddress.Parse(formEdit.tbMask.Text),routeTmp.Protocol, IPAddress.Parse(formEdit.tbHop.Text));
                if (iret == 0)
                    MessageBox.Show("Маршрут изменен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    if (iret == 87)
                        MessageBox.Show("Ошибка. Параметры указаны не корректно", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("!Ошибка. Код ошибки - " + Convert.ToString(iret), "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void grid_DoubleClick(object sender, EventArgs e)
        {
            if (!admin)
                return;
            if (grid.SelectedRows.Count > 0)
            {
                EditRecordInEditForm();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPAddress addr;
            if (IPAddress.TryParse(textBox1.Text, out addr))
                MessageBox.Show((Helper.iph.GetBestRoute(IPAddress.Parse(textBox1.Text)).NextHop.ToString()));
            else
                MessageBox.Show("Недопустимый адрес");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            //saveIndex = grid.SelectedRows[0].Index;
            Helper.RefreshRouteTable();
            //Helper.FillRouteTable();
            //grid.Rows[saveIndex].Selected = true;
            //grid.Refresh();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int saveIndex = grid.CurrentRow.Index;
            Helper.FillRouteTable();
            grid.Rows[saveIndex].Selected = true;
            grid.FirstDisplayedScrollingRowIndex = saveIndex;            
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!admin)
                return;
            if (grid.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Маршрут будет удален. Вы уверены?", "Внимание", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    RouteEntry routeTmp = Helper.GetRoute(Convert.ToInt32(grid.SelectedRows[0].Cells[12].Value) - 1);
                    int iret = Helper.iph.DeleteRouteTableEntry(routeTmp);
                    if (iret != 0)
                        MessageBox.Show("Ошибка при удалении. Код ошибки - " + Convert.ToString(iret), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        Helper.FillRouteTable();
                }
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            Helper.FillRouteTable();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSave formSave = new FormSave();
            formSave.ShowDialog();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.route;
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!admin)
                return;
            if (grid.SelectedRows.Count > 0)
            {
                EditRecordInEditForm();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!admin)
                return;
            FormNew formNew = new FormNew();
            formNew.ShowDialog();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (!admin)
                return;
            if (grid.SelectedRows.Count > 0)
            {
                EditRecordInEditForm();
            }
        }

        private void добавитьМаршрутToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!admin)
                return;
            FormNew formNew = new FormNew();
            formNew.ShowDialog();
        }

        private void редактироватьМаршрутToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!admin)
                return;
            if (grid.SelectedRows.Count > 0)
            {
                EditRecordInEditForm();
            }
        }

        private void удалитьМаршрутToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!admin)
                return;
            if (grid.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Маршрут будет удален. Вы уверены?", "Внимание", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    RouteEntry routeTmp = Helper.GetRoute(Convert.ToInt32(grid.SelectedRows[0].Cells[12].Value) - 1);
                    int iret = Helper.iph.DeleteRouteTableEntry(routeTmp);
                    if (iret != 0)
                        MessageBox.Show("Ошибка при удалении. Код ошибки - " + Convert.ToString(iret), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        Helper.FillRouteTable();
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FormInterfaceInfo formInterfaceInfo = new FormInterfaceInfo();
            formInterfaceInfo.ShowDialog();
        }

        private void списокИнтерфейсовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInterfaceInfo formInterfaceInfo = new FormInterfaceInfo();
            formInterfaceInfo.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormCheckRoute formCheckRoute = new FormCheckRoute();
            formCheckRoute.ShowDialog();
        }

        private void проверитьМаршрутToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCheckRoute formCheckRoute = new FormCheckRoute();
            formCheckRoute.ShowDialog();
        }

        private void toolStripStatus_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawString("Маршрутизация:", this.Font, new SolidBrush(Color.Black), toolStripStatus.Width - 160, 2);
            if (!enableRoute)
            {
                e.Graphics.DrawString("Выкл", myFont, new SolidBrush(Color.Black), toolStripStatus.Width - 60, 2);
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), toolStripStatus.Width - 20, 1, 14, 14);
            }
            else
            {
                e.Graphics.DrawString("Вкл", myFont, new SolidBrush(Color.Black), toolStripStatus.Width - 60, 2);
                e.Graphics.FillEllipse(new SolidBrush(Color.Green), toolStripStatus.Width - 20, 1, 14, 14);
            }
        }

        private void включитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey writeKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters",true);
                if (writeKey == null)
                    return;
                writeKey.SetValue("IPEnableRouter", 1);
                enableRoute = true;                
                toolStrip1.Refresh();
                this.Width = this.Width + 1;
                this.Width = this.Width - 1;
            }
            catch (Exception E)
            {
                MessageBox.Show("Невозможно изменить значение в реестре " + E.Message);
            }
        }

        private void выключитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey writeKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters", true);
                if (writeKey == null)
                    return;
                writeKey.SetValue("IPEnableRouter", 0);
                enableRoute = false;
                toolStrip1.Refresh();
                this.Width = this.Width + 1;
                this.Width = this.Width - 1;
            }
            catch (Exception E)
            {
                MessageBox.Show("Невозможно изменить значение в реестре " + E.Message);
            }
        }
    }
}
