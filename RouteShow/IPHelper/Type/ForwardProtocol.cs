using System;

namespace NetworkPortsLib.Type
{
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]   
    public enum ForwardProtocol
    {
        Other = 1,
        Local = 2,
        Static = 3,
        ICMP = 4,
        EGP = 5,
        GGP = 6,
        Hello = 7,
        RIP = 8,
        IS_IS = 9,
        ES_IS = 10,
        CISCO = 11,
        BBN = 12,
        OSPF = 13,
        BGP = 14,
        NT_AUTOSTATIC = 10002,
        NT_STATIC = 10006,
        NT_STATIC_NON_DOD = 10007
    }
}
