using System;
using System.Net.NetworkInformation;
using System.Net;
using NetworkPortsLib.Type;

namespace NetworkPortsLib
{
    /// <summary>
    /// Interface IP.
    /// </summary>
    public class InterfaceIPEntry
    {
        private InterfaceAddressFlag _flags;
        private IPAddress _mask;
        private int _reasmSize;
        private IPAddress _address;
        private NetworkInterface _interface;
        private int _index;

        public int Index
        {
            get { return _index; }
        }

        public IPAddress Address
        {
            get { return _address; }
        }

        public IPAddress Mask
        {
            get { return _mask; }
        }

        public InterfaceAddressFlag Flags
        {
            get { return _flags; }
        }

        public int ReassembleSize
        {
            get { return _reasmSize; }
        }

        public NetworkInterface RelatedInterface
        {
            get { return _interface; }
        }

        public int Speed
        {
            //get { return (int)_interface.Speed / 1000 / 1000; }
            get { return 0; }//(int)_interface.Speed; }
        }

        public string InterfaceName
        {
            get
            {
                if (this.RelatedInterface == null)
                    return string.Empty;
                else
                    return this.RelatedInterface.Name;
            }
        }

        public string InterfaceDescription
        {
            get
            {
                if (this.RelatedInterface == null)
                    return string.Empty;
                else
                    return this.RelatedInterface.Description;
            }
        }


        public InterfaceIPEntry(int idx, UInt32 address, UInt32 mask, InterfaceAddressFlag flags,
            int reasmsize, NetworkInterface intf)
        {
            this._index = idx;
            this._address = new IPAddress(address);
            this._flags = flags;
            this._interface = intf;
            this._mask = new IPAddress(mask);
            this._reasmSize = reasmsize;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this._index, this.InterfaceName);
        }
    }
}
