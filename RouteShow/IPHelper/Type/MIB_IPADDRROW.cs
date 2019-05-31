using System;

namespace NetworkPortsLib.Type
{
    internal struct MIB_IPADDRROW
    {
        public UInt32 dwAddr;
        public int dwIndex;
        public UInt32 dwMask;
        public UInt32 dwBCastAddr;
        public int dwReasmSize;
        public short unused1;
        public InterfaceAddressFlag wType;
    }
}
