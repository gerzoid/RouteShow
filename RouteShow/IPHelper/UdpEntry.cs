using System;
using System.Diagnostics;
using System.Net;

namespace NetworkPortsLib
{
    /// <summary>
    /// Une connexion UDP.
    /// </summary>
    public class UdpEntry : IComparable<UdpEntry>
    {
        private IPEndPoint _localEndPoint;
        private int _processID;
        private string _processName = null;

        public IPEndPoint LocalEndPoint
        {
            get { return _localEndPoint; }
        }

        public int ProcessID
        {
            get { return _processID; }
        }

        public string ProcessName
        {
            get
            {
                if (string.IsNullOrEmpty(this._processName))
                {
                    this._processName = Process.GetProcessById(this.ProcessID).ProcessName;
                }
                return this._processName;
            }
        }

        public UdpEntry(UInt32 localAddress, int localPort, int processID)
        {
            this._processID = processID;
            this._localEndPoint = new IPEndPoint(localAddress, localPort);
        }



        #region IComparable<UdpEntry> Members

        public int CompareTo(UdpEntry other)
        {
            //d'abord par processus
            if (this.ProcessName.Equals(other.ProcessName))
            {
                //puis par port local
                return this.LocalEndPoint.Port.CompareTo(other.LocalEndPoint.Port);
            }
            else
            {
                return this.ProcessName.CompareTo(other.ProcessName);
            }
        }

        #endregion
    }
}