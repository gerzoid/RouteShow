using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NetworkPortsLib;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RouteShow
{
    static class Helper
    {
        public static DataTable routeTable;

        internal static IPHelper iph;
        static AdaptersTable adapters;
        static List<RouteEntry> _routesTableEntries;
        static IEnumerable<ArpEntry> _arpEntries;
        public static List<InterfaceIPEntry> interfaceInfo;
        
        public static PersistentRoute[] persistentRoutes = new PersistentRoute[0];

        public struct PersistentRoute
        {
            public string dest;
            public string mask;
            public string hop;
            public string metric;
        }


        public static RouteEntry GetRoute(int id)
        {
            return _routesTableEntries[id];
        }

        public static void Init()
        {
            adapters = new AdaptersTable();
            iph = new IPHelper(adapters);            

            _routesTableEntries = iph.GetRoutesTable();
            interfaceInfo = iph.GetInterfacesIP();

            routeTable = new DataTable();
            routeTable.Columns.Add("DestIP", System.Type.GetType("System.String"));
            routeTable.Columns.Add("SubnetMask", System.Type.GetType("System.String"));
            routeTable.Columns.Add("NextHop", System.Type.GetType("System.String"));
            routeTable.Columns.Add("IfIndex", System.Type.GetType("System.Int32"));
            routeTable.Columns.Add("Type", System.Type.GetType("System.Int32"));
            routeTable.Columns.Add("TypeText", System.Type.GetType("System.String"));
            routeTable.Columns.Add("Proto", System.Type.GetType("System.Int32"));
            routeTable.Columns.Add("ProtoText", System.Type.GetType("System.String"));
            routeTable.Columns.Add("Age", System.Type.GetType("System.Int32"));
            routeTable.Columns.Add("Metric1", System.Type.GetType("System.Int32"));
            routeTable.Columns.Add("IFText", System.Type.GetType("System.String"));
            routeTable.Columns.Add("AgeText", System.Type.GetType("System.String"));
            routeTable.Columns.Add("N", System.Type.GetType("System.Int32"));
            routeTable.Columns.Add("Persistent", System.Type.GetType("System.String"));
        }


        private static void GetPersistentRoutes()
        {
            RegistryKey readKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\Tcpip\\Parameters\\PersistentRoutes\\");
            if (readKey == null)
                return;
            string[] loadString = readKey.GetValueNames();
            if ((loadString != null) && (loadString.Length > 0))
            {
                persistentRoutes = new PersistentRoute[loadString.Length];
                for (int x = 0; x <= loadString.Length - 1; x++)
                {
                    string[] tmp = loadString[x].Split(',');
                    if (tmp.Length - 1 >= 0)
                        persistentRoutes[x].dest = tmp[0];
                    if (tmp.Length - 1 >= 1)
                        persistentRoutes[x].mask = tmp[1];
                    if (tmp.Length - 1 >= 2)
                        persistentRoutes[x].hop = tmp[2];
                    if (tmp.Length - 1 >= 3)
                        persistentRoutes[x].metric = tmp[3];
                }
            }
            readKey.Close();
        }



        public static void RefreshRouteTable()
        {
            _routesTableEntries = iph.GetRoutesTable();
            GetPersistentRoutes();
            bool finded = false;
            //routeTable
            for (int y = 0; y <= routeTable.Rows.Count - 1; y++)
            {
                for (int x = 0; x <= _routesTableEntries.Count - 1; x++)
                {

                    if ((routeTable.Rows[y]["DestIp"].ToString() == _routesTableEntries[x].Destination.ToString()) && (routeTable.Rows[y]["SubNetMask"].ToString() == _routesTableEntries[x].Mask.ToString())
                      && (routeTable.Rows[y]["ifindex"].ToString() == Convert.ToString(_routesTableEntries[x].Index)) && (routeTable.Rows[y]["NextHop"].ToString() == _routesTableEntries[x].NextHop.ToString()))
                    {
                        routeTable.Rows[y][0] = _routesTableEntries[x].Destination;
                        routeTable.Rows[y][1] = _routesTableEntries[x].Mask;
                        routeTable.Rows[y][2] = _routesTableEntries[x].NextHop;
                        routeTable.Rows[y][3] = _routesTableEntries[x].Index;
                        routeTable.Rows[y][4] = _routesTableEntries[x].ForwardType;
                        routeTable.Rows[y][5] = _routesTableEntries[x].ForwardType;
                        routeTable.Rows[y][6] = _routesTableEntries[x].Protocol;
                        routeTable.Rows[y][7] = _routesTableEntries[x].Protocol;
                        routeTable.Rows[y][8] = _routesTableEntries[x].Age;
                        routeTable.Rows[y][9] = _routesTableEntries[x].Metric1;
                        routeTable.Rows[y][11] = TimeSpan.FromSeconds(_routesTableEntries[x].Age);

                    }
                }
            }           
            
            
        }

        public static string GetBestRoute(System.Net.IPAddress destination)
        {
            RouteEntry tmpRouteEntry = iph.GetBestRoute(destination);
            return tmpRouteEntry.NextHop.ToString();
        }

        public static void FillRouteTable()
        {
            adapters = new AdaptersTable();
            iph = new IPHelper(adapters);
            interfaceInfo = iph.GetInterfacesIP();
            GetPersistentRoutes();
            _routesTableEntries = iph.GetRoutesTable();
            routeTable.Clear();
            for (int x = 0; x <= _routesTableEntries.Count - 1; x++)
            {
                routeTable.Rows.Add();
                routeTable.Rows[routeTable.Rows.Count - 1][0] = _routesTableEntries[x].Destination;
                routeTable.Rows[routeTable.Rows.Count - 1][1] = _routesTableEntries[x].Mask;
                routeTable.Rows[routeTable.Rows.Count - 1][2] = _routesTableEntries[x].NextHop;
                routeTable.Rows[routeTable.Rows.Count - 1][3] = _routesTableEntries[x].Index;
                routeTable.Rows[routeTable.Rows.Count - 1][4] = _routesTableEntries[x].ForwardType;
                routeTable.Rows[routeTable.Rows.Count - 1][5] = _routesTableEntries[x].ForwardType;
                routeTable.Rows[routeTable.Rows.Count - 1][6] = _routesTableEntries[x].Protocol;
                routeTable.Rows[routeTable.Rows.Count - 1][7] = _routesTableEntries[x].Protocol;
                routeTable.Rows[routeTable.Rows.Count - 1][8] = _routesTableEntries[x].Age;
                routeTable.Rows[routeTable.Rows.Count - 1][9] = _routesTableEntries[x].Metric1;
                InterfaceIPEntry inter = GetInterfaceByID(_routesTableEntries[x].Index);
                if (inter != null)
                    routeTable.Rows[routeTable.Rows.Count - 1][10] = inter.InterfaceDescription;
                else
                    MessageBox.Show("sdfgdfgfg");
                routeTable.Rows[routeTable.Rows.Count - 1][11] = TimeSpan.FromSeconds(_routesTableEntries[x].Age);
                routeTable.Rows[routeTable.Rows.Count - 1][12] = Convert.ToString(x + 1);
                routeTable.Rows[routeTable.Rows.Count - 1][13] = Convert.ToString("Нет");
                    if (persistentRoutes.Length - 1 >= 0)
                    {
                        bool find = false;
                        for (int y = 0; y <= persistentRoutes.Length - 1; y++)
                        {
                            if (_routesTableEntries[x].Destination.ToString() == persistentRoutes[y].dest)
                                if (_routesTableEntries[x].Mask.ToString() == persistentRoutes[y].mask)
                                    if (_routesTableEntries[x].NextHop.ToString() == persistentRoutes[y].hop)
                                    {
                                        routeTable.Rows[routeTable.Rows.Count - 1][13] = Convert.ToString("Да");
                                        find = true;
                                    }
                            if (!find)
                                routeTable.Rows[routeTable.Rows.Count - 1][13] = Convert.ToString("Нет");
                        }
                    }

            }
        }

        public static InterfaceIPEntry GetInterfaceByID(int id)
        {
            for (int x = 0; x <= interfaceInfo.Count - 1; x++)
            {
                if (interfaceInfo[x].Index == id)
                    return interfaceInfo[x];
            }
            return null;
        }

    }
}
