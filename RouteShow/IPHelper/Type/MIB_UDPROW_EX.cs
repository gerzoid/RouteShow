using System;
using System.Runtime.InteropServices;


namespace NetworkPortsLib.Type
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MIB_UDPROW_EX
    {
        public uint dwLocalAddr;
        public int dwLocalPort;
        public int dwProcessId;
    }
}
