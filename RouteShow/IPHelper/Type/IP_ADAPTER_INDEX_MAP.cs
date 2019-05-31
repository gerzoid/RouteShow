using System;
using System.Runtime.InteropServices;

namespace NetworkPortsLib.Type
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IP_ADAPTER_INDEX_MAP
    {
        const int MAX_ADAPTER_NAME = 128;

        public int Index;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ADAPTER_NAME)]
        public String Name;
    }
}