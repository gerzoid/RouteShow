using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NetworkPortsLib.Type
{

    //une entrée de la table ARP
    internal struct MIB_IPNETROW
    {
        private const int MAXLEN_PHYSADDR = 8;

        public int dwIndex;
        public int dwPhysAddrLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXLEN_PHYSADDR)]
        public byte[] bPhysAddr;
        public UInt32 dwAddr;
        public ArpFlags dwType;
    }
}
