using System;
using System.Runtime.InteropServices;

namespace NetworkPortsLib.Type
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MIB_TCPTABLE2
    {
        public int dwNumEntries;
        public IntPtr table;
    }
}
