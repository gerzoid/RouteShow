using System;

namespace NetworkPortsLib.Type
{
    internal struct MIB_IPFORWARDROW
    {
        public UInt32 dwForwardDest;
        public UInt32 dwForwardMask;
        public int dwForwardPolicy;
        public UInt32 dwForwardNextHop;
        public int dwForwardIfIndex;
        public ForwardType dwForwardType;
        public ForwardProtocol dwForwardProto;
        public int dwForwardAge;
        public int dwForwardNextHopAS;
        public int dwForwardMetric1;
        public int dwForwardMetric2;
        public int dwForwardMetric3;
        public int dwForwardMetric4;
        public int dwForwardMetric5;

        public static implicit operator MIB_IPFORWARDROW(RouteEntry value)
        {
            MIB_IPFORWARDROW ipForward = new MIB_IPFORWARDROW();

            ipForward.dwForwardAge = value.Age;
            ipForward.dwForwardDest = BitConverter.ToUInt32(value.Destination.GetAddressBytes(), 0);
            ipForward.dwForwardMask = BitConverter.ToUInt32(value.Mask.GetAddressBytes(), 0);
            ipForward.dwForwardMetric1 = value.Metric1;
            ipForward.dwForwardMetric2 = value.Metric2;
            ipForward.dwForwardMetric3 = value.Metric3;
            ipForward.dwForwardMetric4 = value.Metric4;
            ipForward.dwForwardMetric5 = value.Metric5;
            ipForward.dwForwardNextHop = BitConverter.ToUInt32(value.NextHop.GetAddressBytes(), 0);
            ipForward.dwForwardNextHopAS = value.NextHopAS;
            ipForward.dwForwardPolicy = value.Policy;
            ipForward.dwForwardProto = value.Protocol;
            ipForward.dwForwardType = value.ForwardType;

            AdaptersTable adapters = new AdaptersTable();
            ipForward.dwForwardIfIndex = adapters.GetAdapterIndex(value.RelatedInterface);

            return ipForward;
        }
    }
}