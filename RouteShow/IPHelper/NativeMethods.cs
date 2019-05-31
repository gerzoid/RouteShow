using System;
using System.Runtime.InteropServices;
using NetworkPortsLib.Type;

namespace NetworkPortsLib
{
    internal class NativeMethods
    {
        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetIpNetTable(IntPtr pIpNetTable, ref int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int FlushIpNetTable(int dwIfIndex);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int DeleteIpNetEntry(ref  MIB_IPNETROW pArpEntry);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int CreateIpNetEntry(ref  MIB_IPNETROW pArpEntry);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int SetIpNetEntry(ref MIB_IPNETROW pArpEntry);

        [DllImport("iphlpapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int IpRenewAddress(ref IP_ADAPTER_INDEX_MAP AdapterInfo);

        [DllImport("iphlpapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int IpReleaseAddress(ref IP_ADAPTER_INDEX_MAP AdapterInfo);

        [DllImport("Iphlpapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetInterfaceInfo(IntPtr pIfTable, ref int dwOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetExtendedTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder, int ulAf,
            TCP_TABLE_CLASS TableClass, uint Reserved);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetExtendedUdpTable(IntPtr pUdpTable, ref int pdwSize, bool bOrder, int ulAf,
            UDP_TABLE_CLASS TableClass, uint Reserved);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetIpForwardTable(IntPtr pIpForwardTable, ref int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int SetTcpEntry(ref MIB_TCPROW_EX pTcpRow);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetIfEntry(ref MIB_IFROW pIfRow);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetIfTable(IntPtr pIfTable, ref int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetIpAddrTable(IntPtr pIpAddrTable, ref int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetRTTAndHopCount(int destIpAddress, ref int hopCount, int maxHops, ref int RTT);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int CreateIpForwardEntry(ref MIB_IPFORWARDROW pRoute);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int DeleteIpForwardEntry(ref MIB_IPFORWARDROW pRoute);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int SetIpForwardEntry(ref MIB_IPFORWARDROW pRoute);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int GetBestRoute(uint dwDestAddr, int dwSourceAddr, out MIB_IPFORWARDROW pRoute);   //dwSourceAddr = 0 for the caller

    }
}