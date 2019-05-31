using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RouteShow
{
    public static class NativeMethods
    {
        public struct IP_ADAPTER_INDEX_MAP
        {
            public int Index;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ADAPTER_NAME)]
            public String Name;
        }

        public const int MAX_ADAPTER_NAME = 128;

        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int ERROR_SUCCESS = 0;
        
        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        public extern static int GetIpForwardTable(IntPtr /*PMIB_IPFORWARDTABLE*/ pIpForwardTable, ref int /*PULONG*/ pdwSize, bool bOrder);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        public extern static int CreateIpForwardEntry(IntPtr /*PMIB_IPFORWARDROW*/ pRoute);

        [DllImport("Iphlpapi.dll", CharSet = CharSet.Auto)]
        public static extern int GetInterfaceInfo(Byte[] PIfTableBuffer, ref int size);

        [DllImport("Iphlpapi.dll", CharSet = CharSet.Auto)]
        public static extern int IpReleaseAddress(ref IP_ADAPTER_INDEX_MAP AdapterInfo);
        
        [DllImport("iphlpapi.dll", CharSet = CharSet.Ansi)]
        public static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref Int64 pBufOutLen);
    }
}
