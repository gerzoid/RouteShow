using System;
using System.Runtime.InteropServices;

namespace NetworkPortsLib.Type
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct IP_INTERFACE_INFO
    {
        public int NumAdapters;
        public IntPtr Adapter;
    }
}
