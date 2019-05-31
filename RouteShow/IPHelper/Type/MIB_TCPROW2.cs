using System;
using System.Runtime.InteropServices;

namespace NetworkPortsLib.Type
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MIB_TCPROW2
    {
        public MIB_TCP_STATE dwState;
        public int dwLocalAddr;
        public int dwLocalPort;
        public int dwRemoteAddr;
        public int dwRemotePort;
        public TCP_CONNECTION_OFFLOAD_STATE dwOffloadState;
    }
}
