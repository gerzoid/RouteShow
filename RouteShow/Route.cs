using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using System.Net;

namespace RouteShow
{
    static class Route
    {
        static IntPtr fwdTable;
        static public IPForwardTable forwardTable;
        static DataTable tmpTable = new DataTable();
        //Структура таблицы маршрутизации
        [ComVisible(false), StructLayout(LayoutKind.Sequential)]
        internal struct IPForwardTable
        {
            public uint Size;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public IPFORWARDROW[] Table;
        };

        //Структура одного маршрута
        [ComVisible(false), StructLayout(LayoutKind.Sequential)]
        internal struct IPFORWARDROW
        {
            internal uint /*DWORD*/ dwForwardDest;
            internal uint /*DWORD*/ dwForwardMask;
            internal uint /*DWORD*/ dwForwardPolicy;
            internal uint /*DWORD*/ dwForwardNextHop;
            internal uint /*DWORD*/ dwForwardIfIndex;
            internal uint /*DWORD*/ dwForwardType;
            internal uint /*DWORD*/ dwForwardProto;
            internal uint /*DWORD*/ dwForwardAge;
            internal uint /*DWORD*/ dwForwardNextHopAS;
            internal uint /*DWORD*/ dwForwardMetric1;
            internal uint /*DWORD*/ dwForwardMetric2;
            internal uint /*DWORD*/ dwForwardMetric3;
            internal uint /*DWORD*/ dwForwardMetric4;
            internal uint /*DWORD*/ dwForwardMetric5;
        };
        
        //Получаем Таблицу маршрутизации.
        static IPForwardTable ReadIPForwardTable(IntPtr tablePtr)   
        {
            var result = (IPForwardTable)Marshal.PtrToStructure(tablePtr, typeof(IPForwardTable));

            IPFORWARDROW[] table = new IPFORWARDROW[result.Size];
            IntPtr p = new IntPtr(tablePtr.ToInt64() + Marshal.SizeOf(result.Size));
            for (int i = 0; i < result.Size; ++i)
            {
                table[i] = (IPFORWARDROW)Marshal.PtrToStructure(p, typeof(IPFORWARDROW));
                p = new IntPtr(p.ToInt64() + Marshal.SizeOf(typeof(IPFORWARDROW)));
            }
            result.Table = table;

            return result;
        }
        
        //Инициализация всех структур и считывание таблицы маршрутизации
        static public void Init()
        {
            fwdTable = IntPtr.Zero;
            int size = 0;
            var result = NativeMethods.GetIpForwardTable(fwdTable, ref size, true);
            fwdTable = Marshal.AllocHGlobal(size);
            result = NativeMethods.GetIpForwardTable(fwdTable, ref size, true);
            forwardTable = ReadIPForwardTable(fwdTable);
            Marshal.FreeHGlobal(fwdTable);

            tmpTable.Columns.Add("DestIP", System.Type.GetType("System.String"));
            tmpTable.Columns.Add("SubnetMask", System.Type.GetType("System.String"));
            tmpTable.Columns.Add("NextHop", System.Type.GetType("System.String"));
            tmpTable.Columns.Add("IfIndex", System.Type.GetType("System.Int32"));
            tmpTable.Columns.Add("Type", System.Type.GetType("System.Int32"));
            tmpTable.Columns.Add("Proto", System.Type.GetType("System.String"));
            tmpTable.Columns.Add("Age", System.Type.GetType("System.Int32"));
            tmpTable.Columns.Add("Metric1", System.Type.GetType("System.Int32"));
            tmpTable.Columns.Add("Type_Text", System.Type.GetType("System.Int32"));
            tmpTable.Columns.Add("TypeText", System.Type.GetType("System.String"));
            tmpTable.Columns.Add("IFText", System.Type.GetType("System.String"));
        }

        static public void RefreshRouteInfo()
        {
            fwdTable = IntPtr.Zero;
            int size = 0;
            var result = NativeMethods.GetIpForwardTable(fwdTable, ref size, true);
            fwdTable = Marshal.AllocHGlobal(size);
            result = NativeMethods.GetIpForwardTable(fwdTable, ref size, true);
            forwardTable = ReadIPForwardTable(fwdTable);
            Marshal.FreeHGlobal(fwdTable);
        }

        public static DataTable FillTableRouteInfo()
        {
            tmpTable.Rows.Clear();
            AdapterInfo.IP_ADAPTER_INFO info = new AdapterInfo.IP_ADAPTER_INFO();            
            for (int i = 0; i < forwardTable.Table.Length; ++i)
            {
                tmpTable.Rows.Add();
                tmpTable.Rows[tmpTable.Rows.Count - 1]["DestIP"] = new IPAddress((long)forwardTable.Table[i].dwForwardDest).ToString();
                tmpTable.Rows[tmpTable.Rows.Count - 1]["SubnetMask"] = new IPAddress((long)forwardTable.Table[i].dwForwardMask).ToString();
                tmpTable.Rows[tmpTable.Rows.Count - 1]["NextHop"] = new  IPAddress((long)forwardTable.Table[i].dwForwardNextHop).ToString();
                tmpTable.Rows[tmpTable.Rows.Count - 1]["IfIndex"] = forwardTable.Table[i].dwForwardIfIndex;
                tmpTable.Rows[tmpTable.Rows.Count - 1]["Type"] = forwardTable.Table[i].dwForwardType;
                tmpTable.Rows[tmpTable.Rows.Count - 1]["Proto"] = forwardTable.Table[i].dwForwardProto;
                tmpTable.Rows[tmpTable.Rows.Count - 1]["Age"] = forwardTable.Table[i].dwForwardAge;
                tmpTable.Rows[tmpTable.Rows.Count - 1]["Metric1"] = forwardTable.Table[i].dwForwardMetric1;
                tmpTable.Rows[tmpTable.Rows.Count - 1]["TypeText"] = GetForwardTypeText(Convert.ToByte(forwardTable.Table[i].dwForwardType));
                string sss = AdapterInfo.GetAdapterInfo(Convert.ToByte(forwardTable.Table[i].dwForwardIfIndex) - 1).IpAddressList.IpAddress.Address;
                tmpTable.Rows[tmpTable.Rows.Count - 1]["IFText"] = sss;
            }            
            return tmpTable;
        }
        static public IPFORWARDROW GetIPFroward(int position)
        {
            return (forwardTable.Table[position]);
        }

        static public string GetForwardTypeText(byte type)
        {
            switch (type)
            {
                case 1:
                    return "Другое";
                case 2:
                    return "Ошибка";
                case 3:
                    return "Локальный";
                case 4:
                    return "Удаленный";
                default:
                    return "Unknown";
            }
        }
    

    }

}
