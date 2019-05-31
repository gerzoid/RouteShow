using System;
using System.Net;
using NetworkPortsLib.Type;
using System.Net.NetworkInformation;

namespace NetworkPortsLib
{
    /// <summary>
    /// Une route dans la table de routage.
    /// </summary>
    public class RouteEntry
    {
        internal MIB_IPFORWARDROW _ipFwdNative;
        private int _metric1;
        private int _metric2;
        private int _metric3;
        private int _metric4;
        private int _metric5;
        private IPAddress _destination;
        private IPAddress _mask;
        private int _policy;
        private IPAddress _nextHop;
        private NetworkInterface _interface;
        private ForwardProtocol _protocol;
        private ForwardType _type;
        private int _nextHopAS;
        private int _age;
        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public IPAddress Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        public IPAddress Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }

        public int Policy
        {
            get { return _policy; }
            set { _policy = value; }
        }

        public IPAddress NextHop
        {
            get { return _nextHop; }
            set { _nextHop = value; }
        }

        public NetworkInterface RelatedInterface
        {
            get { return _interface; }
        }

        public string InterfaceName
        {
            get
            {
                if (this.RelatedInterface == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.RelatedInterface.Name;
                }
            }
        }

        public ForwardType ForwardType
        {
            get { return _type; }
            set { _type = value; }
        }

        public ForwardProtocol Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public int NextHopAS
        {
            get { return _nextHopAS; }
            set { _nextHopAS = value; }
        }

        public int Metric1
        {
            get { return _metric1; }
            set { _metric1 = value; }
        }

        public int Metric2
        {
            get { return _metric2; }
            set { _metric2 = value; }
        }

        public int Metric3
        {
            get { return _metric3; }
            set { _metric3 = value; }
        }

        public int Metric4
        {
            get { return _metric4; }
            set { _metric4 = value; }
        }

        public int Metric5
        {
            get { return _metric5; }
            set { _metric5 = value; }
        }

        public RouteEntry(UInt32 destination, UInt32 mask, int policy, UInt32 nextHop, NetworkInterface intf,
            ForwardType type, ForwardProtocol proto, int age, int nextHopAS, int metric1,
            int metric2, int metric3, int metric4, int metric5, int idx)
        {
            this._age = age;
            this._policy = policy;
            this._protocol = proto;
            this._type = type;

            this._destination = new IPAddress(destination);
            this._mask = new IPAddress(mask);
            this._nextHop = new IPAddress(nextHop);
            this._nextHopAS = nextHopAS;

            this._interface = intf;

            this._metric1 = metric1;
            this._metric2 = metric2;
            this._metric3 = metric3;
            this._metric4 = metric4;
            this._metric5 = metric5;
            this._index = idx;
        }
    }
}