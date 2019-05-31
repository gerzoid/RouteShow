using System;
using System.Runtime.InteropServices;

namespace NetworkPortsLib.Type
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct MIB_IFROW
    {
        const int MAX_INTERFACE_NAME_LEN = 256;
        const int MAXLEN_IFDESCR = 256;
        const int MAXLEN_PHYSADDR = 8;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_INTERFACE_NAME_LEN)]
        public string wszName;
        public int dwIndex;
        public int dwType;
        public int dwMtu;
        public int dwSpeed;
        public int dwPhysAddrLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXLEN_PHYSADDR)]
        public byte[] bPhysAddr;
        public int dwAdminStatus;
        public int dwOperStatus;
        public int dwLastChange;
        public int dwInOctets;
        public int dwInUcastPkts;
        public int dwInNUcastPkts;
        public int dwInDiscards;
        public int dwInErrors;
        public int dwInUnknownProtos;
        public int dwOutOctets;
        public int dwOutUcastPkts;
        public int dwOutNUcastPkts;
        public int dwOutDiscards;
        public int dwOutErrors;
        public int dwOutQLen;
        public int dwDescrLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXLEN_IFDESCR)]
        public byte[] bDescr;
    }
}
