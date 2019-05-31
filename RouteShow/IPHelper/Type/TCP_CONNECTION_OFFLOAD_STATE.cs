using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkPortsLib.Type
{
    internal enum TCP_CONNECTION_OFFLOAD_STATE
    {
        TcpConnectionOffloadStateInHost = 0,
        TcpConnectionOffloadStateOffloading = 1,
        TcpConnectionOffloadStateOffloaded = 2,
        TcpConnectionOffloadStateUploading = 3,
        TcpConnectionOffloadStateMax = 4
    }
}
