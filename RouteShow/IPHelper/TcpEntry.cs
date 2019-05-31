using System;
using System.Diagnostics;
using System.Net;
using NetworkPortsLib.Type;

namespace NetworkPortsLib
{
    /// <summary>
    /// Une connexion TCP.
    /// </summary>
    /// <remarks></remarks>
    public class TcpEntry : IComparable<TcpEntry>
    {
        private MIB_TCP_STATE _state;
        private IPEndPoint _remoteEndPoint;
        private IPEndPoint _localEndPoint;
        private int _processID;
        private string _processName = null;

        public MIB_TCP_STATE State
        {
            get { return _state; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return _remoteEndPoint; }
        }

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

        public TcpEntry(MIB_TCP_STATE state, UInt32 remoteAddr, int remotePort, UInt32 localAddress, int localPort, int processID)
        {
            this._state = state;
            this._remoteEndPoint = new IPEndPoint(remoteAddr, remotePort);
            this._processID = processID;
            this._localEndPoint = new IPEndPoint(localAddress, localPort);
        }

        /// <summary>
        /// Ferme la connexion de force
        /// </summary>
        /// <remarks></remarks>
        public void Close()
        {
            MIB_TCPROW_EX tcpEntry = new MIB_TCPROW_EX();
            //reremplit chacune des infos identifiant la connexion
            {
                tcpEntry.dwLocalAddr = BitConverter.ToUInt32(this.LocalEndPoint.Address.GetAddressBytes(), 0);
                tcpEntry.dwLocalPort = this.LocalEndPoint.Port;
                tcpEntry.dwRemoteAddr = BitConverter.ToUInt32(this.RemoteEndPoint.Address.GetAddressBytes(), 0);
                tcpEntry.dwRemotePort = this.RemoteEndPoint.Port;
                //la définie comme supprimée
                tcpEntry.dwState = MIB_TCP_STATE.DELETE_TCB;
            }
            //envoie la demande de close
            int ret = NativeMethods.SetTcpEntry(ref tcpEntry);
            if (ret != 0) throw new System.ComponentModel.Win32Exception(ret);
        }

        #region IComparable<TcpEntry> Members

        public int CompareTo(TcpEntry other)
        {
            //if (other != null)
            //{
            //d'abord par processus
            if (this.ProcessName.Equals(other.ProcessName))
            {
                //puis par état
                if (this.State.Equals(other.State))
                {
                    //puis par machine distante
                    return this.RemoteEndPoint.Port.CompareTo(other.RemoteEndPoint.Port);
                }
                else
                {
                    return this.State.CompareTo(other.State);
                }
            }
            else
            {
                return this.ProcessName.CompareTo(other.ProcessName);
            }
            //}
            //return 0;
        }

        #endregion
    }
}
