using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using XLSExportDemo;
using System.Globalization;

namespace RouteShow.Forms
{
    public partial class FormSave : Form
    {
        int[] checkedInterface;

        public FormSave()
        {
            InitializeComponent();
        }

        private string GetFormattedText(int length, string value)
        {
            string tmp = value.PadRight(length);
            return tmp;
        }

        private void button1_Click_saved(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "MyRouters";
            
            if (radioTXT.Checked)
                saveFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt";
            if (radioCSV.Checked)
                saveFileDialog1.Filter = "CSV разделители - ; (*.csv)|*.csv";
            if (radioBAT.Checked)
                saveFileDialog1.Filter = "Пакетные файлы - ; (*.bat)|*.bat";
            
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
//Экспорт в TXT
                if (radioTXT.Checked)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default);
                    writer.WriteLine("Таблица маршрутизации от " + DateTime.Now.ToLocalTime() + "\nВсего маршрутов - " + Convert.ToString(Helper.routeTable.Rows.Count) + "\nФайл создан в программе RouteShow" + "\nhttp:\\\\jobtools.ru\n");
                    writer.WriteLine(GetFormattedText(18, "Destination IP") + GetFormattedText(18, "Mask") + GetFormattedText(18, "Hop") + GetFormattedText(12, "Type") + GetFormattedText(12, "Protocol") + GetFormattedText(8, "Metric") + GetFormattedText(16, "Interface Index") + GetFormattedText(12, "Pesistent"));
                    for (int x = 0; x <= Helper.routeTable.Rows.Count - 1; x++)
                    {
                        writer.WriteLine(GetFormattedText(18, Helper.routeTable.Rows[x]["DestIP"].ToString()) + GetFormattedText(18, Helper.routeTable.Rows[x]["SubnetMask"].ToString()) + GetFormattedText(18, Helper.routeTable.Rows[x]["NextHop"].ToString()) + GetFormattedText(12, Helper.routeTable.Rows[x]["TypeText"].ToString()) + GetFormattedText(12, Helper.routeTable.Rows[x]["ProtoText"].ToString()) + GetFormattedText(8, Helper.routeTable.Rows[x]["Metric1"].ToString()) + GetFormattedText(16, Helper.routeTable.Rows[x]["IfIndex"].ToString()) + GetFormattedText(12, Helper.routeTable.Rows[x]["Persistent"].ToString()));
                    }
                    writer.WriteLine("\nСписок интерфейсов:");                    
                    for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
                    {
                        writer.WriteLine(Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName);
                    }

                    writer.Flush();
                    writer.Close();
                }
//Экcпорт в CSV                
                if (radioCSV.Checked)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default);
                    writer.WriteLine("Таблица маршрутизации от " + DateTime.Now.ToLocalTime() + "\nВсего маршрутов - " + Convert.ToString(Helper.routeTable.Rows.Count) + "\nФайл создан в программе RouteShow" + "\nhttp:\\\\jobtools.ru\n");
                    writer.WriteLine("Destination IP" + ";" + "Mask" + ";" + "Hop" + ";" + "Type" + ";" + "Protocol" + ";" + "Metric" + ";" + "Interface Index" + ";" + "Pesistent");
                    for (int x = 0; x <= Helper.routeTable.Rows.Count - 1; x++)
                    {
                        writer.WriteLine(Helper.routeTable.Rows[x]["DestIP"].ToString() + ";" + Helper.routeTable.Rows[x]["SubnetMask"].ToString() + ";" + Helper.routeTable.Rows[x]["NextHop"].ToString() + ";" + Helper.routeTable.Rows[x]["TypeText"].ToString() + ";" + Helper.routeTable.Rows[x]["ProtoText"].ToString() + ";" + Helper.routeTable.Rows[x]["Metric1"].ToString() + ";" + Helper.routeTable.Rows[x]["IfIndex"].ToString() + ";" + Helper.routeTable.Rows[x]["Persistent"].ToString());
                    }
                    writer.WriteLine("\nСписок интерфейсов:");
                    for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
                    {
                        writer.WriteLine(Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName);
                    }
                    writer.Flush();
                    writer.Close();
                }
//Экcпорт в BAT
                if (radioBAT.Checked)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default);
                    writer.WriteLine("rem Таблица маршрутизации от " + DateTime.Now.ToLocalTime() + "\nrem Всего маршрутов - " + Convert.ToString(Helper.routeTable.Rows.Count) + "\nrem Файл создан в программе RouteShow" + "\nrem http:\\\\jobtools.ru\n");
                    bool persistent = false;

                    for (int x = 0; x <= Helper.routeTable.Rows.Count - 1; x++)
                    {
                        persistent = false;
                        if (Helper.routeTable.Rows[x]["Persistent"].ToString() == "Да") persistent = true;
                        writer.WriteLine("route delete " + Helper.routeTable.Rows[x]["DestIP"].ToString()+" MASK "+Helper.routeTable.Rows[x]["SubnetMask"].ToString()+" " + Helper.routeTable.Rows[x]["NextHop"].ToString());
                        if (persistent)
                            writer.WriteLine("route add -p " + Helper.routeTable.Rows[x]["DestIP"].ToString()+" MASK "+Helper.routeTable.Rows[x]["SubnetMask"].ToString()+" " + Helper.routeTable.Rows[x]["NextHop"].ToString()+" METRIC "+Helper.routeTable.Rows[x]["Metric1"].ToString());
                        else
                            writer.WriteLine("route add " + Helper.routeTable.Rows[x]["DestIP"].ToString() + " MASK " + Helper.routeTable.Rows[x]["SubnetMask"].ToString() + " " + Helper.routeTable.Rows[x]["NextHop"].ToString() + " METRIC " + Helper.routeTable.Rows[x]["Metric1"].ToString() + " IF " + Helper.routeTable.Rows[x]["IfIndex"].ToString());
                    }
                    writer.WriteLine("\nrem Список интерфейсов:");
                    for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
                    {
                        writer.WriteLine("rem " + Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName);
                    }


                    writer.Flush();
                    writer.Close();
                }

                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkedInterface = new int[checkedListBox1.CheckedItems.Count];
            for (int x=0; x<=checkedListBox1.CheckedItems.Count-1;x++)
            {
                string ttt = checkedListBox1.CheckedItems[x].ToString().Substring(0, checkedListBox1.CheckedItems[x].ToString().IndexOf('-')).Trim();
                checkedInterface[x] = Convert.ToInt32(ttt);
            }
            
            //Считаем все маршруты по данному интерфейсу - да геморно, да фигово решение, но пока так 8)
            int countAllRoutersWhereInterfacesChecked = 0;            
            for (int x = 0; x <= Helper.routeTable.Rows.Count - 1; x++)
                for (int y = 0; y <= checkedInterface.Length - 1; y++)
                    if (Convert.ToInt32(Helper.routeTable.Rows[x]["IfIndex"]) == checkedInterface[y])
                        countAllRoutersWhereInterfacesChecked++;

            saveFileDialog1.FileName = "MyRouters";

            if (radioTXT.Checked)
                saveFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt";
            if (radioCSV.Checked)
                saveFileDialog1.Filter = "CSV разделители - ; (*.csv)|*.csv";
            if (radioBAT.Checked)
                saveFileDialog1.Filter = "Пакетные файлы - ; (*.bat)|*.bat";
            if (radioXLS.Checked)
                saveFileDialog1.Filter = "Файлы Excel - ; (*.xls)|*.xls";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Экспорт в TXT
                if (radioTXT.Checked)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default);
                    writer.WriteLine("Таблица маршрутизации от " + DateTime.Now.ToLocalTime() + "\nВсего маршрутов - " + Convert.ToString(countAllRoutersWhereInterfacesChecked) + "\nФайл создан в программе RouteShow" + "\nhttp:\\\\jobtools.ru\n");
                    writer.WriteLine(GetFormattedText(18, "Destination IP") + GetFormattedText(18, "Mask") + GetFormattedText(18, "Hop") + GetFormattedText(12, "Type") + GetFormattedText(12, "Protocol") + GetFormattedText(8, "Metric") + GetFormattedText(16, "Interface Index") + GetFormattedText(12, "Persistent"));
                    for (int x = 0; x <= Helper.routeTable.Rows.Count - 1; x++)
                        for (int y = 0; y <= checkedInterface.Length - 1; y++)
                            if (Convert.ToInt32(Helper.routeTable.Rows[x]["IfIndex"]) == checkedInterface[y])
                                writer.WriteLine(GetFormattedText(18, Helper.routeTable.Rows[x]["DestIP"].ToString()) + GetFormattedText(18, Helper.routeTable.Rows[x]["SubnetMask"].ToString()) + GetFormattedText(18, Helper.routeTable.Rows[x]["NextHop"].ToString()) + GetFormattedText(12, Helper.routeTable.Rows[x]["TypeText"].ToString()) + GetFormattedText(12, Helper.routeTable.Rows[x]["ProtoText"].ToString()) + GetFormattedText(8, Helper.routeTable.Rows[x]["Metric1"].ToString()) + GetFormattedText(16, Helper.routeTable.Rows[x]["IfIndex"].ToString()) + GetFormattedText(12, Helper.routeTable.Rows[x]["Persistent"].ToString()));
                    writer.WriteLine("\nСписок интерфейсов:");
                    for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
                        for (int z = 0; z <= checkedInterface.Length - 1; z++)
                            if (Convert.ToInt32(Helper.interfaceInfo[y].Index) == checkedInterface[z])
                                writer.WriteLine(Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName);
                    writer.Flush();
                    writer.Close();
                }
                //Экcпорт в CSV                
                if (radioCSV.Checked)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default);
                    writer.WriteLine("Таблица маршрутизации от " + DateTime.Now.ToLocalTime() + "\nВсего маршрутов - " + Convert.ToString(countAllRoutersWhereInterfacesChecked) + "\nФайл создан в программе RouteShow" + "\nhttp:\\\\jobtools.ru\n");
                    writer.WriteLine("Destination IP" + ";" + "Mask" + ";" + "Hop" + ";" + "Type" + ";" + "Protocol" + ";" + "Metric" + ";" + "Interface Index" + ";" + "Persistent");
                    for (int x = 0; x <= Helper.routeTable.Rows.Count - 1; x++)
                        for (int y = 0; y <= checkedInterface.Length - 1; y++)
                            if (Convert.ToInt32(Helper.routeTable.Rows[x]["IfIndex"]) == checkedInterface[y])
                                writer.WriteLine(Helper.routeTable.Rows[x]["DestIP"].ToString() + ";" + Helper.routeTable.Rows[x]["SubnetMask"].ToString() + ";" + Helper.routeTable.Rows[x]["NextHop"].ToString() + ";" + Helper.routeTable.Rows[x]["TypeText"].ToString() + ";" + Helper.routeTable.Rows[x]["ProtoText"].ToString() + ";" + Helper.routeTable.Rows[x]["Metric1"].ToString() + ";" + Helper.routeTable.Rows[x]["IfIndex"].ToString() + ";" + Helper.routeTable.Rows[x]["Persistent"].ToString());
                    writer.WriteLine("\nСписок интерфейсов:");
                    for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
                        for (int z = 0; z <= checkedInterface.Length - 1; z++)
                            if (Convert.ToInt32(Helper.interfaceInfo[y].Index) == checkedInterface[z])
                                writer.WriteLine(Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName);
                    writer.Flush();
                    writer.Close();
                }
                //Экcпорт в BAT
                if (radioBAT.Checked)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.Default);
                    writer.WriteLine("rem Таблица маршрутизации от " + DateTime.Now.ToLocalTime() + "\nrem Всего маршрутов - " + Convert.ToString(countAllRoutersWhereInterfacesChecked) + "\nrem Файл создан в программе RouteShow" + "\nrem http:\\\\jobtools.ru\n");
                    bool persistent = false;

                    for (int x = 0; x <= Helper.routeTable.Rows.Count - 1; x++)
                        for (int y = 0; y <= checkedInterface.Length - 1; y++)
                            if (Convert.ToInt32(Helper.routeTable.Rows[x]["IfIndex"]) == checkedInterface[y])
                            {
                                persistent = false;
                                if (Helper.routeTable.Rows[x]["Persistent"].ToString() == "Да") persistent = true;
                                writer.WriteLine("route delete " + Helper.routeTable.Rows[x]["DestIP"].ToString() + " MASK " + Helper.routeTable.Rows[x]["SubnetMask"].ToString() + " " + Helper.routeTable.Rows[x]["NextHop"].ToString());
                                if (persistent)
                                    writer.WriteLine("route add -p " + Helper.routeTable.Rows[x]["DestIP"].ToString() + " MASK " + Helper.routeTable.Rows[x]["SubnetMask"].ToString() + " " + Helper.routeTable.Rows[x]["NextHop"].ToString() + " METRIC " + Helper.routeTable.Rows[x]["Metric1"].ToString());
                                else
                                    writer.WriteLine("route add " + Helper.routeTable.Rows[x]["DestIP"].ToString() + " MASK " + Helper.routeTable.Rows[x]["SubnetMask"].ToString() + " " + Helper.routeTable.Rows[x]["NextHop"].ToString() + " METRIC " + Helper.routeTable.Rows[x]["Metric1"].ToString() + " IF " + Helper.routeTable.Rows[x]["IfIndex"].ToString());
                            }
                    writer.WriteLine("\nrem Список интерфейсов:");
                    for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
                        for (int z = 0; z <= checkedInterface.Length - 1; z++)
                            if (Convert.ToInt32(Helper.interfaceInfo[y].Index) == checkedInterface[z])
                                writer.WriteLine("rem " + Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName);
                    writer.Flush();
                    writer.Close();
                }

                if (radioXLS.Checked)
                {
                    ExcelDocument document = new ExcelDocument();
                    document.UserName = "DBFShow";
                    document.CodePage = CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
                    int pos = 0;
                    Font font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //document[pos, 0].Font = font;
                    document[0, 0].Value = "Таблица маршрутизации от " + DateTime.Now.ToLocalTime();
                    document[0, 0].Font = font;
                    document[1, 0].Value = "Всего маршрутов - " + Convert.ToString(countAllRoutersWhereInterfacesChecked);
                    document[1, 0].Font = font;
                    document[2, 0].Value = "Файл создан в программе RouteShow " + "http:\\\\jobtools.ru";
                    document[2, 0].Font = font;

                    document.ColumnWidth(0, 120);
                    document.ColumnWidth(1, 120);
                    document.ColumnWidth(2, 120);

                    document[4, 0].Value = "Destination IP";
                    document[4, 0].Font = font;
                    document[4, 1].Value = "Mask";
                    document[4, 1].Font = font;                    
                    document[4, 2].Value = "Hop";
                    document[4, 2].Font = font;
                    document[4, 3].Value = "Type";
                    document[4, 3].Font = font;                    
                    document[4, 4].Value = "Protocol";
                    document[4, 4].Font = font;
                    document[4, 5].Value = "Metric";
                    document[4, 5].Font = font;
                    document[4, 6].Value = "Interface index";
                    document[4, 6].Font = font;
                    pos = 5;
                    for (int x = 0; x <= Helper.routeTable.Rows.Count - 1; x++)
                        for (int y = 0; y <= checkedInterface.Length - 1; y++)
                            if (Convert.ToInt32(Helper.routeTable.Rows[x]["IfIndex"]) == checkedInterface[y])
                            {
                                document[pos, 0].Value = Helper.routeTable.Rows[x]["DestIP"].ToString();
                                document[pos, 1].Value = Helper.routeTable.Rows[x]["SubnetMask"].ToString();
                                document[pos, 2].Value = Helper.routeTable.Rows[x]["NextHop"].ToString();
                                document[pos, 3].Value = Helper.routeTable.Rows[x]["TypeText"].ToString();
                                document[pos, 4].Value = Helper.routeTable.Rows[x]["ProtoText"].ToString();
                                document[pos, 5].Value = Helper.routeTable.Rows[x]["Metric1"].ToString();
                                document[pos, 6].Value = Helper.routeTable.Rows[x]["IfIndex"].ToString();
                                document[pos, 7].Value = Helper.routeTable.Rows[x]["Persistent"].ToString();
                                pos++;
                            }
                    pos++;
                    document[pos, 0].Value = "Список интерфейсов:";
                    document[pos, 0].Font = font;
                    pos++;
                    for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
                        for (int z = 0; z <= checkedInterface.Length - 1; z++)
                            if (Convert.ToInt32(Helper.interfaceInfo[y].Index) == checkedInterface[z])
                            {
                                document[pos, 0].Value = Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName;
                                pos++;
                            }
                    Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                    document.Save(stream);
                    stream.Close();                    
                }
                Close();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormSave_Shown(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.route;
            for (int y = 0; y <= Helper.interfaceInfo.Count - 1; y++)
            {
                checkedListBox1.Items.Add(Convert.ToString(Helper.interfaceInfo[y].Index) + " - " + Helper.interfaceInfo[y].InterfaceDescription + " " + Helper.interfaceInfo[y].InterfaceName, true);
            }
        }

        private void FormSave_Resize(object sender, EventArgs e)
        {
            checkedListBox1.Width = Width - 120;
        }
    }
}
