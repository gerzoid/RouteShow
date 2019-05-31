using System;

namespace NetworkPortsLib.Type
{
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]   
    public enum ForwardType
    {
        Other = 1,
        Invalid = 2,
        Direct = 3,
        Indirect = 4
    }
}