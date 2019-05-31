using System;

namespace NetworkPortsLib.Type
{
    [Flags()]
    public enum InterfaceAddressFlag : short
    {
        Primary = 1,
        Unk1 = 2,
        Dynamic = 4,
        Disconnected = 8,
        Unk2 = 16,
        Unk3 = 32,
        Deleted = 64,
        Transient = 128
    }
}
