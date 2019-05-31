using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using NetworkPortsLib.Type;

namespace NetworkPortsLib
{
    /// <summary>
    /// Une entrée de la table ARP
    /// </summary>
    /// <remarks></remarks>
    public class ArpEntry
    {
        internal MIB_IPNETROW _ipArpNative;
        private NetworkInterface _interface;
        private ArpFlags _flags;
        private byte[] _macAddress;
        private IPAddress _address;
        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        /// <summary>
        /// Adresse IP distante
        /// </summary>
        public IPAddress Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Interface sur laquelle les adresses IP et MAC sont accessibles directement
        /// </summary>
        public NetworkInterface RelatedInterface
        {
            get { return _interface; }
            set { _interface = value; }
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

        /// <summary>
        /// Adresse MAC distante
        /// </summary>
        public byte[] MacAddress
        {
            get { return _macAddress; }
            set { _macAddress = value; }
        }

        public string MacAddressString
        {
            get
            {
                return string.Format("{0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}",
                    this.MacAddress[0], this.MacAddress[1], this.MacAddress[2], this.MacAddress[3],
                    this.MacAddress[4], this.MacAddress[5]);
            }
        }

        /// <summary>
        /// Type d'entrée ARP
        /// </summary>
        public ArpFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        public ArpEntry(NetworkInterface intf, byte[] macAddress, UInt32 address, ArpFlags flags)
        {
            this._macAddress = macAddress;
            this._flags = flags;
            this._address = new IPAddress(address);

            this._interface = intf;
        }
    }
}
