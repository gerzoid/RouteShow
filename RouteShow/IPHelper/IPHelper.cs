using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Timers;
using NetworkPortsLib.Type;
using System.Data;

namespace NetworkPortsLib
{
    /// <summary>
    /// Classe de fonction IP Helper API.
    /// </summary>
    public class IPHelper
    {
        const Int32 AF_INET = 2;
        const Int32 ERROR_INSUFFICIENT_BUFFER = 122;
        const Int32 ERROR_INVALID_PARAMETER = 87;
        const Int32 ERROR_ACCESS_DENIED = 5;

        private Timer _timer;
        AdaptersTable _adapters;
        private List<TcpEntry> _tcpTable = new List<TcpEntry>();
        private List<UdpEntry> _udpTable = new List<UdpEntry>();

        public delegate void DelEnumPortsEventHandler(IEnumerable<TcpEntry> tcpTable, IEnumerable<UdpEntry> udpTable);

        /// <summary>
        /// Surveillance des ports ouverts.
        /// </summary>
        public event DelEnumPortsEventHandler ListeningOpenPorts;

        /// <summary>
        /// Intervalle de surveillance.
        /// </summary>
        public int ListeingInterval { get; set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="adapters"><see cref="AdaptersTable"/> des interfaces IP.</param>
        public IPHelper(AdaptersTable adapters)
        {
            _adapters = adapters;
            ListeingInterval = 3000;
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
        }

        /// <summary>
        /// Liste des entrées de la table ARP.
        /// </summary>
        public IEnumerable<ArpEntry> GetArpEntries()
        {
            List<ArpEntry> arpEntries = new List<ArpEntry>();

            IntPtr pTable = IntPtr.Zero;
            int iBufLen = 0;
            int iRet = 0;

            //1ier appel pour déterminer la taille de la table ARP.
            iRet = NativeMethods.GetIpNetTable(IntPtr.Zero, ref iBufLen, true);

            try
            {
                pTable = Marshal.AllocHGlobal(iBufLen);

                //Lecture de la table.
                iRet = NativeMethods.GetIpNetTable(pTable, ref iBufLen, true);

                //Retour OK.
                if (iRet == 0)
                {
                    //Nombre d'entrées dans la table.
                    int iEntries = Marshal.ReadInt32(pTable);
                    //Pointeur sur 1ière entrée.
                    IntPtr pEntry = new IntPtr(pTable.ToInt32() + 4);

                    //Parcours chaque entrée.
                    for (int i = 0; i < iEntries; i++)
                    {
                        //Lit l'entrée.
                        MIB_IPNETROW entry = (MIB_IPNETROW)Marshal.PtrToStructure(pEntry, typeof(MIB_IPNETROW));

                        ArpEntry arpEntry = new ArpEntry(_adapters.GetAdapter(entry.dwIndex), entry.bPhysAddr, entry.dwAddr, entry.dwType);
                        arpEntry._ipArpNative = entry;

                        //Extrait les infos.
                        arpEntries.Add(arpEntry);

                        //Pointeur sur entrée suivante.
                        pEntry = new IntPtr(pEntry.ToInt32() + Marshal.SizeOf(typeof(MIB_IPNETROW)));
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Marshal.FreeHGlobal(pTable);
            }

            return arpEntries;
        }

        /// <summary>
        /// Envoit une requete ARP sur l'adresse <paramref name="ipAddress"/>.
        /// </summary>
        /// <param name="ipAddress">Adresse IP.</param>
        /// <returns>Adresse MAC sous forme d'un tableau d'octet.</returns>
        public byte[] GetMacAddrFromIpAddr(IPAddress ipAddress)
        {
            byte[] bytMacAddr = new byte[6];
            int iPhysLength = bytMacAddr.Length;

            int ret = NativeMethods.SendARP(BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0),
                0, bytMacAddr, ref iPhysLength);

            return bytMacAddr;
        }

        /// <summary>
        /// Convertit une adresse MAC représenté en tableau d'octets vers une chaine de caractère XX:XX:XX:XX:XX:XX.
        /// </summary>
        /// <param name="bytMacAddr">Tableau d'octets adresse MAC.</param>
        /// <returns>Chaine de caractère de l'adresse MAC.</returns>
        public string ConvertMacAddrArrayToString(byte[] bytMacAddr)
        {
            if ((bytMacAddr != null) && (bytMacAddr.Length == 6))
            {
                return string.Format("{0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}",
                    bytMacAddr[0], bytMacAddr[1], bytMacAddr[2], bytMacAddr[3],
                    bytMacAddr[4], bytMacAddr[5]);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Convertit une adresse MAC représenté en chaine de caractère vers un tableau d'octets.
        /// </summary>
        /// <param name="macAddr">Chaine de caractère représentant l'adresse MAC.</param>
        /// <returns>Tableau d'octet représentant l'adresse MAC.</returns>
        public byte[] ConvertMacAddrStringToByteArray(string macAddr)
        {
            if (!string.IsNullOrEmpty(macAddr))
            {
                string[] szVals = macAddr.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                byte[] bytMac = new byte[8];

                for (int i = 0; i < szVals.Length; i++)
                    bytMac[i] = byte.Parse(szVals[i], System.Globalization.NumberStyles.HexNumber);

                return bytMac;
            }
            else
                return null;
        }

        /// <summary>
        /// Liste les interfaces IP de la machine hôte.
        /// </summary>
        /// <returns></returns>
        public List<InterfaceIPEntry> GetInterfacesIP()
        {
            List<InterfaceIPEntry> interfacesIPs = new List<InterfaceIPEntry>();

            IntPtr pTable = IntPtr.Zero;
            int iBufLen = 0;
            int iRet = 0;

            //1ier appel pour déterminer la taille de la table des interfaces IP.
            iRet = NativeMethods.GetIpAddrTable(IntPtr.Zero, ref iBufLen, true);

            try
            {
                pTable = Marshal.AllocHGlobal(iBufLen);

                //Lecture de la table.
                iRet = NativeMethods.GetIpAddrTable(pTable, ref iBufLen, true);

                //Retour OK.
                if (iRet == 0)
                {
                    //Nombre d'entrées.
                    int iEntries = Marshal.ReadInt32(pTable);
                    IntPtr pEntry = new IntPtr(pTable.ToInt32() + 4);

                    //Parcours chaque entrée.
                    for (int i = 0; i <= iEntries - 1; i++)
                    {
                        //Lit l'entrée.
                        MIB_IPADDRROW entry = (MIB_IPADDRROW)Marshal.PtrToStructure(pEntry, typeof(MIB_IPADDRROW));

                        //Extrait les infos.
                        interfacesIPs.Add(new InterfaceIPEntry(entry.dwIndex, entry.dwAddr, entry.dwMask,
                            entry.wType, entry.dwReasmSize, _adapters.GetAdapter(entry.dwIndex)));

                        //Pointeur sur entrée suivante.
                        pEntry = new IntPtr(pEntry.ToInt32() + Marshal.SizeOf(typeof(MIB_IPADDRROW)));
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Marshal.FreeHGlobal(pTable);
            }

            return interfacesIPs;
        }

        public int DeleteRouteEntry(ref RouteEntry entryToSet, int IPInterface, IPAddress IPDestination,
            IPAddress Mask, ForwardProtocol Protocol, IPAddress NextHopAddr, int metric)
        {
            int ret = -1;

            try
            {
                MIB_IPFORWARDROW ipFwdSet = entryToSet._ipFwdNative;
                ipFwdSet.dwForwardIfIndex = IPInterface;
                ipFwdSet.dwForwardDest = (uint)BitConverter.ToInt32(IPDestination.GetAddressBytes(), 0);
                ipFwdSet.dwForwardMask = (uint)BitConverter.ToInt32(Mask.GetAddressBytes(), 0);
                ipFwdSet.dwForwardProto = Protocol;
                ipFwdSet.dwForwardNextHop = (uint)BitConverter.ToInt32(NextHopAddr.GetAddressBytes(), 0);
                ipFwdSet.dwForwardMetric1 = metric;
                //Appel api pour modifier la route.
                ret = NativeMethods.DeleteIpForwardEntry(ref ipFwdSet);
            }
            catch (Exception)
            {
            }

            return ret;
        }


        /// <summary>
        /// Modifie une route existante.
        /// </summary>
        /// <param name="entryToSet">Route à modifier.</param>
        /// <param name="IPInterface">Nouvelle interface.</param>
        /// <param name="IPDestination">Nouvelle IP de destination.</param>
        /// <param name="Mask">Nouveau masque.</param>
        /// <param name="Protocol">Protocol de routage.</param>
        /// <param name="NextHopAddr">Nouvelle adresse du prochain saut.</param>
        /// <returns>0 si réussi.</returns>
        public int ChangeRouteEntry(ref RouteEntry entryToSet, int IPInterface, IPAddress IPDestination,
            IPAddress Mask, ForwardProtocol Protocol, IPAddress NextHopAddr, int metric)
        {
            int ret = -1;

            try
            {
                MIB_IPFORWARDROW ipFwdSet = entryToSet._ipFwdNative;
                ipFwdSet.dwForwardIfIndex = IPInterface;
                ipFwdSet.dwForwardDest = (uint)BitConverter.ToInt32(IPDestination.GetAddressBytes(), 0);
                ipFwdSet.dwForwardMask = (uint)BitConverter.ToInt32(Mask.GetAddressBytes(), 0);
                ipFwdSet.dwForwardProto = Protocol;
                ipFwdSet.dwForwardNextHop = (uint)BitConverter.ToInt32(NextHopAddr.GetAddressBytes(), 0);
                //ipFwdSet.dwForwardMetric1 = metric;
                //Appel api pour modifier la route.
                ret = NativeMethods.SetIpForwardEntry(ref ipFwdSet);
                if (ret == 0)
                {
                    entryToSet.Index = IPInterface;
                    entryToSet.Destination = IPDestination;
                    entryToSet.Mask = Mask;
                    entryToSet.Protocol = Protocol;
                    entryToSet.NextHop = NextHopAddr;
                    entryToSet.Metric1 = metric;
                }
            }
            catch (Exception)
            {
            }

            return ret;
        }

        /// <summary>
        /// Indique la route qui sera utilisé pour joindre l'adresse <paramref name="destinationToReach"/>.
        /// </summary>
        /// <param name="destinationToReach">Adresse IP à joindre.</param>
        /// <returns></returns>
        public RouteEntry GetBestRoute(IPAddress destinationToReach)
        {
            try
            {
                MIB_IPFORWARDROW ipFwd;

                //Appel api qui déterminera la route à prendre.
                int ret = NativeMethods.GetBestRoute(
                    BitConverter.ToUInt32(destinationToReach.GetAddressBytes(), 0),
                    0, out ipFwd);

                if (ret == 0)
                {
                    RouteEntry re = new RouteEntry(ipFwd.dwForwardDest, ipFwd.dwForwardMask, ipFwd.dwForwardPolicy,
                        ipFwd.dwForwardNextHop, _adapters.GetAdapter(ipFwd.dwForwardIfIndex), ipFwd.dwForwardType,
                        ipFwd.dwForwardProto, ipFwd.dwForwardAge, (int)ipFwd.dwForwardNextHop, ipFwd.dwForwardMetric1,
                        ipFwd.dwForwardMetric2, ipFwd.dwForwardMetric3, ipFwd.dwForwardMetric4, ipFwd.dwForwardMetric5,
                        ipFwd.dwForwardIfIndex);

                    re._ipFwdNative = ipFwd;

                    return re;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Liste les entrées UDP.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UdpEntry> GetUDPTable()
        {
            RefreshOpenedPorts(PortType.UDP);
            return _udpTable;
        }

        /// <summary>
        /// Liste les entrées TCP.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TcpEntry> GetTCPTable()
        {
            RefreshOpenedPorts(PortType.TCP);
            return _tcpTable;
        }

        /// <summary>
        /// Démarre la surveillance des entrées TCP/UDP.
        /// </summary>
        public void StartListeningOpenedPorts()
        {
            _timer.Interval = ListeingInterval;
            _timer.Start();
        }

        /// <summary>
        /// Arrête la surveillance des entrées TCP/UDP.
        /// </summary>
        public void StopListeningOpenedPorts()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Fournit des statisques du protocole TCP sur couche IP v4 de la machine.
        /// </summary>
        /// <returns></returns>
        public IPGlobalStatistics GetTcpV4Statistics()
        {
            return IPGlobalProperties.GetIPGlobalProperties().GetIPv4GlobalStatistics();
        }

        /// <summary>
        /// Fournit des statisques du protocole UDP sur couche IP v4 de la machine.
        /// </summary>
        /// <returns></returns>
        public UdpStatistics GetUdpV4Statistics()
        {
            return IPGlobalProperties.GetIPGlobalProperties().GetUdpIPv4Statistics();
        }

        /// <summary>
        /// Fournit des statisques du protocole TCP sur couche IP v6 de la machine.
        /// </summary>
        /// <returns></returns>
        public TcpStatistics GetTcpV6Statistics()
        {
            return IPGlobalProperties.GetIPGlobalProperties().GetTcpIPv6Statistics();
        }

        /// <summary>
        /// Fournit des statisques du protocole UDP sur couche IP v6 de la machine.
        /// </summary>
        /// <returns></returns>
        public UdpStatistics GetUdpV6Statistics()
        {
            return IPGlobalProperties.GetIPGlobalProperties().GetUdpIPv6Statistics();
        }

        /// <summary>
        /// Fournit des statisques du protocole ICMP sur couche IP v4 de la machine.
        /// </summary>
        /// <returns></returns>
        public IcmpV4Statistics GetIcmpV4Statistics()
        {
            return IPGlobalProperties.GetIPGlobalProperties().GetIcmpV4Statistics();
        }

        /// <summary>
        /// Fournit des statisques du protocole ICMP sur couche IP v6 de la machine.
        /// </summary>
        /// <returns></returns>
        public IcmpV6Statistics GetIcmpV6Statistics()
        {
            return IPGlobalProperties.GetIPGlobalProperties().GetIcmpV6Statistics();
        }

        /// <summary>
        /// Liste les routes de la table de routage.
        /// </summary>
        /// <returns></returns>
        public List<RouteEntry> GetRoutesTable()
        {
            List<RouteEntry> forwardEntries = new List<RouteEntry>();

            IntPtr pTable = IntPtr.Zero;
            int iBufLen = 0;
            int iRet = 0;

            //1ier appel pour déterminer la taille de la table de routage.           
            iRet = NativeMethods.GetIpForwardTable(IntPtr.Zero, ref iBufLen, true);

            try
            {
                pTable = Marshal.AllocHGlobal(iBufLen);

                //Lecture de la table.
                iRet = NativeMethods.GetIpForwardTable(pTable, ref iBufLen, true);

                //Retour OK.
                if (iRet == 0)
                {
                    //Nombre d'entrées dans la table.
                    int iEntries = Marshal.ReadInt32(pTable);

                    //Pointeur sur 1ière entrée.
                    IntPtr pEntry = new IntPtr(pTable.ToInt32() + 4);

                    for (int i = 0; i < iEntries; i++)
                    {
                        //Lit la ligne de la table de routage.
                        MIB_IPFORWARDROW entry = (MIB_IPFORWARDROW)Marshal.PtrToStructure(pEntry, typeof(MIB_IPFORWARDROW));

                        RouteEntry rteEntry = new RouteEntry(
                            entry.dwForwardDest, entry.dwForwardMask, entry.dwForwardPolicy, entry.dwForwardNextHop,
                            _adapters.GetAdapter(entry.dwForwardIfIndex), entry.dwForwardType, entry.dwForwardProto,
                            entry.dwForwardAge, entry.dwForwardNextHopAS, entry.dwForwardMetric1, entry.dwForwardMetric2,
                            entry.dwForwardMetric3, entry.dwForwardMetric4, entry.dwForwardMetric5, entry.dwForwardIfIndex);

                        rteEntry._ipFwdNative = entry;

                        //Extrait les infos.
                        forwardEntries.Add(rteEntry);

                        //Pointeur sur entrée suivante.
                        pEntry = new IntPtr(pEntry.ToInt32() + Marshal.SizeOf(typeof(MIB_IPFORWARDROW)));
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Marshal.FreeHGlobal(pTable);
            }

            return forwardEntries;
        }



        public DataTable GetRoutesTableInTable()
        {
            List<RouteEntry> forwardEntries = new List<RouteEntry>();
            DataTable tmp = new DataTable();
            tmp.Columns.Add("DestIP", System.Type.GetType("System.String"));
            tmp.Columns.Add("SubnetMask", System.Type.GetType("System.String"));
            tmp.Columns.Add("NextHop", System.Type.GetType("System.String"));
            tmp.Columns.Add("IfIndex", System.Type.GetType("System.Int32"));
            tmp.Columns.Add("Type", System.Type.GetType("System.Int32"));
            tmp.Columns.Add("TypeText", System.Type.GetType("System.String"));
            tmp.Columns.Add("Proto", System.Type.GetType("System.Int32"));
            tmp.Columns.Add("ProtoText", System.Type.GetType("System.String"));
            tmp.Columns.Add("Age", System.Type.GetType("System.Int32"));            
            tmp.Columns.Add("Metric1", System.Type.GetType("System.Int32"));
            tmp.Columns.Add("Type_Text", System.Type.GetType("System.Int32"));
            tmp.Columns.Add("IFText", System.Type.GetType("System.String"));
            tmp.Columns.Add("AgeText", System.Type.GetType("System.String"));
            IntPtr pTable = IntPtr.Zero;
            int iBufLen = 0;
            int iRet = 0;
            iRet = NativeMethods.GetIpForwardTable(IntPtr.Zero, ref iBufLen, true);
            try
            {
                pTable = Marshal.AllocHGlobal(iBufLen);
                iRet = NativeMethods.GetIpForwardTable(pTable, ref iBufLen, true);
                if (iRet == 0)
                {
                    int iEntries = Marshal.ReadInt32(pTable);
                    IntPtr pEntry = new IntPtr(pTable.ToInt32() + 4);

                    for (int i = 0; i < iEntries; i++)
                    {
                        MIB_IPFORWARDROW entry = (MIB_IPFORWARDROW)Marshal.PtrToStructure(pEntry, typeof(MIB_IPFORWARDROW));
                        RouteEntry rteEntry = new RouteEntry(
                            entry.dwForwardDest, entry.dwForwardMask, entry.dwForwardPolicy, entry.dwForwardNextHop,
                            _adapters.GetAdapter(entry.dwForwardIfIndex), entry.dwForwardType, entry.dwForwardProto,
                            entry.dwForwardAge, entry.dwForwardNextHopAS, entry.dwForwardMetric1, entry.dwForwardMetric2,
                            entry.dwForwardMetric3, entry.dwForwardMetric4, entry.dwForwardMetric5, entry.dwForwardIfIndex);

                        tmp.Rows.Add();
                        tmp.Rows[tmp.Rows.Count - 1][0] = rteEntry.Destination;
                        tmp.Rows[tmp.Rows.Count - 1][1] = rteEntry.Mask;
                        tmp.Rows[tmp.Rows.Count - 1][2] = rteEntry.NextHop;
                        tmp.Rows[tmp.Rows.Count - 1][3] = entry.dwForwardIfIndex;
                        tmp.Rows[tmp.Rows.Count - 1][4] = rteEntry.ForwardType;
                        tmp.Rows[tmp.Rows.Count - 1][5] = rteEntry.ForwardType;
                        tmp.Rows[tmp.Rows.Count - 1][6] = rteEntry.Protocol;
                        tmp.Rows[tmp.Rows.Count - 1][7] = rteEntry.Protocol;                        
                        tmp.Rows[tmp.Rows.Count - 1][8] = rteEntry.Age;
                        tmp.Rows[tmp.Rows.Count - 1][9] = rteEntry.Metric1;
                        pEntry = new IntPtr(pEntry.ToInt32() + Marshal.SizeOf(typeof(MIB_IPFORWARDROW)));
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Marshal.FreeHGlobal(pTable);
            }

            return tmp;
        }




        /// <summary>
        /// Ajoute une nouvelle route <paramref name="entry"/> dans la table de routage.
        /// </summary>
        /// <param name="entry">Route à ajouter.</param>
        /// <returns></returns>
        public int AddRouteEntry(RouteEntry entry)
        {
            int ret = 0;
                        
            entry._ipFwdNative.dwForwardIfIndex = entry.Index;
            entry._ipFwdNative.dwForwardDest = (uint)entry.Destination.Address;
            entry._ipFwdNative.dwForwardMask = (uint)entry.Mask.Address;
            entry._ipFwdNative.dwForwardProto = entry.Protocol;
            entry._ipFwdNative.dwForwardNextHop = (uint)entry.NextHop.Address;
            entry._ipFwdNative.dwForwardMetric1 = entry.Metric1;
            try
            {
                if (ret == 0)
                    ret = NativeMethods.CreateIpForwardEntry(ref entry._ipFwdNative);
            }
            catch (Exception)
            {
            }

            return ret;
        }

        /// <summary>
        /// Supprime la route <paramref name="entry"/> dans la table de routage.
        /// </summary>
        /// <param name="entry">Route à ajouter.</param>
        /// <returns></returns>
        public int DeleteRouteTableEntry(RouteEntry entry)
        {
            int ret = 0;

            try
            {
                if (ret == 0)
                    ret = NativeMethods.DeleteIpForwardEntry(ref entry._ipFwdNative);
            }
            catch (Exception)
            {
            }

            return ret;
        }

        /// <summary>
        /// Ajoute une entrée <paramref name="entry"/> dans la table ARP.
        /// </summary>
        /// <param name="entry">Entrée ARP.</param>
        /// <returns></returns>
        public bool AddArpEntry(ArpEntry entry)
        {
            int ret = 0;

            try
            {
                if (ret == 0)
                    ret = NativeMethods.CreateIpNetEntry(ref entry._ipArpNative);
            }
            catch (Exception)
            {
            }

            return ret == 0 ? true : false;
        }

        /// <summary>
        /// Supprime l'entrée <paramref name="entry"/> dans la table ARP.
        /// </summary>
        /// <param name="entry">Entrée ARP.</param>
        /// <returns></returns>
        public bool DeleteArpEntry(ArpEntry entry)
        {
            int ret = 0;

            try
            {
                if (ret == 0)
                    ret = NativeMethods.DeleteIpNetEntry(ref entry._ipArpNative);
            }
            catch (Exception)
            {
            }

            return ret == 0 ? true : false;
        }

        /// <summary>
        /// Vide le cache ARP de l'interface IP <paramref name="networkInterface"/>.
        /// </summary>
        /// <param name="networkInterface">Interface IP.</param>
        /// <returns></returns>
        public bool FlushArp(NetworkInterface networkInterface)
        {
            int ret = 0;

            try
            {
                if (ret == 0)
                    ret = NativeMethods.FlushIpNetTable(_adapters.GetAdapterIndex(networkInterface));
            }
            catch (Exception)
            {
            }

            return ret == 0 ? true : false;
        }

        /// <summary>
        /// Vide le cache ARP de l'interface IP <paramref name="interfaceIndex"/>.
        /// </summary>
        /// <param name="interfaceIndex">Interface IP.</param>
        /// <returns></returns>
        public bool FlushArp(int interfaceIndex)
        {
            int ret = 0;

            try
            {
                if (ret == 0)
                    ret = NativeMethods.FlushIpNetTable(interfaceIndex);
            }
            catch (Exception)
            {
            }

            return ret == 0 ? true : false;
        }

        /// <summary>
        /// Modifie une entrée ARP existante.
        /// </summary>
        /// <param name="entryToSet">Entrée à modifier.</param>
        /// <param name="IPInterface">Nouvelle interface.</param>
        /// <param name="PhysicalAddress">Nouvelle adresse physique (MAC).</param>
        /// <param name="Address">Nouvelle adresse IP.</param>
        /// <param name="Flags">Flag ARP.</param>
        /// <returns></returns>
        public int ChangeArpEntry(ref ArpEntry entryToSet, int IPInterface, byte[] PhysicalAddress, IPAddress Address, ArpFlags Flags)
        {
            int ret = -1;

            try
            {
                MIB_IPNETROW ipArp = entryToSet._ipArpNative;
                ipArp.dwIndex = IPInterface;
                ipArp.dwType = Flags;
                ipArp.dwAddr = (uint)BitConverter.ToInt32(Address.GetAddressBytes(), 0);
                ipArp.bPhysAddr = PhysicalAddress;
                ipArp.dwPhysAddrLen = PhysicalAddress.Length;

                //Appel api pour modifier l'entrée.
                ret = NativeMethods.SetIpNetEntry(ref ipArp);
                if (ret == 0)
                {
                    entryToSet.Index = ipArp.dwIndex;
                    entryToSet.Address = Address;
                    entryToSet.Flags = Flags;
                    entryToSet.MacAddress = PhysicalAddress;
                    entryToSet.RelatedInterface = _adapters.GetAdapter(IPInterface);
                }

            }
            catch (Exception)
            {
            }

            return ret;
        }

        /// <summary>
        /// Libère l'adresse IP de l'interface <paramref name="interfaceIP"/>.
        /// </summary>
        /// <param name="interfaceIP">Interface IP.</param>
        /// <returns></returns>
        public bool ReleaseIpAddress(int interfaceIP)
        {
            int ret = -1;

            try
            {
                IP_ADAPTER_INDEX_MAP ipAdpt = GetInternalInterfaceInfoFromIndex(interfaceIP);
                ret = NativeMethods.IpReleaseAddress(ref ipAdpt);
            }
            catch (Exception)
            {
            }

            return ret == 0 ? true : false;
        }

        /// <summary>
        /// Acquisition d'une adresse IP pour l'interface <paramref name="interfaceIP"/>.
        /// </summary>
        /// <param name="interfaceIP">Interface IP.</param>
        /// <returns></returns>
        public bool RenewIpAddress(int interfaceIP)
        {
            int ret = -1;

            try
            {
                IP_ADAPTER_INDEX_MAP ipAdpt = GetInternalInterfaceInfoFromIndex(interfaceIP);
                ret = NativeMethods.IpRenewAddress(ref ipAdpt);
            }
            catch (Exception)
            {
            }

            return ret == 0 ? true : false;
        }

        /// <summary>
        /// Obtient des infos. basiques sur une interface IP.
        /// </summary>
        /// <param name="interfaceIP">Index de l'interface IP.</param>
        /// <returns></returns>
        private IP_ADAPTER_INDEX_MAP GetInternalInterfaceInfoFromIndex(int index)
        {
            IP_ADAPTER_INDEX_MAP ipAdpIdxName = new IP_ADAPTER_INDEX_MAP();
            int iResSize = 0;

            //1ier appel pour déterminer la taille des infos à lire.
            NativeMethods.GetInterfaceInfo(IntPtr.Zero, ref iResSize);

            IntPtr pInfo = Marshal.AllocHGlobal(iResSize);

            //Obtient les infos.
            int ret = NativeMethods.GetInterfaceInfo(pInfo, ref iResSize);

            if (ret == 0)
            {
                //Nombre d'entrées.
                int iEntries = Marshal.ReadInt32(pInfo);

                //Pointeur sur la 1ière entrée.
                IntPtr pInfo2 = new IntPtr(pInfo.ToInt32() + Marshal.SizeOf(typeof(int)));

                //Lecture des infos.
                for (int i = 0; i < iEntries; i++)
                {
                    IP_ADAPTER_INDEX_MAP ipAdapMap = (IP_ADAPTER_INDEX_MAP)
                        Marshal.PtrToStructure(pInfo2, typeof(IP_ADAPTER_INDEX_MAP));

                    //Si index demandé = index de l'entrée lut on récupère ces infos et ont sort.
                    if (index == ipAdapMap.Index)
                    {
                        ipAdpIdxName = ipAdapMap;
                        break;
                    }

                    //Pointeur sur entrée suivante.
                    pInfo2 = new IntPtr(pInfo2.ToInt32() + Marshal.SizeOf(typeof(IP_ADAPTER_INDEX_MAP)));
                }
            }

            Marshal.FreeHGlobal(pInfo);

            return ipAdpIdxName;
        }

        /// <summary>
        /// Maj des ports TCP/UDP ouverts de la machine hôte.
        /// </summary>
        private void RefreshAllOpenedPorts()
        {
            RefreshOpenedPorts(PortType.TCP);
            RefreshOpenedPorts(PortType.UDP);
        }

        private void RefreshOpenedPorts(PortType type)
        {
            IntPtr pTable = IntPtr.Zero;
            int iOutBufLen = 0;
            int iRet = 0;

            //Demande pour port TCP uniquement.
            if (type == PortType.TCP)
            {
                _tcpTable.Clear();

                //Récupère la taille de la table TCP.
                iRet = NativeMethods.GetExtendedTcpTable(IntPtr.Zero, ref iOutBufLen, true, AF_INET,
                    TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);

                try
                {
                    pTable = Marshal.AllocHGlobal(iOutBufLen);
                    iRet = NativeMethods.GetExtendedTcpTable(pTable, ref iOutBufLen, true, AF_INET,
                        TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);

                    //Appel OK.
                    if (iRet == 0)
                    {
                        //récupère le nombre d'entrées
                        int iEntries = Marshal.ReadInt32(pTable);

                        //Pointeur sur 1ière entrée.
                        IntPtr pEntry = new IntPtr(pTable.ToInt32() + 4);

                        //Lecture de chaque entrée.
                        for (int i = 0; i < iEntries; i++)
                        {
                            //Lit l'entrée.
                            MIB_TCPROW_EX entry = (MIB_TCPROW_EX)Marshal.PtrToStructure(pEntry, typeof(MIB_TCPROW_EX));

                            //Extrait les infos.
                            this._tcpTable.Add(new TcpEntry(entry.dwState, entry.dwRemoteAddr, entry.dwRemotePort,
                                entry.dwLocalAddr, entry.dwLocalPort, entry.dwProcessId));

                            //Pointeur sur entrée suivante.
                            pEntry = new IntPtr(pEntry.ToInt32() + Marshal.SizeOf(typeof(MIB_TCPROW_EX)));
                        }

                        this._tcpTable.Sort();
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    Marshal.FreeHGlobal(pTable);
                }
            }

            //Demande pour port UDP uniquement.
            if (type == PortType.UDP)
            {
                _udpTable.Clear();

                //Rcupère la taille de la table UDP.
                iRet = NativeMethods.GetExtendedUdpTable(pTable, ref iOutBufLen, true, AF_INET,
                    UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0);

                try
                {
                    pTable = Marshal.AllocHGlobal(iOutBufLen);
                    iRet = NativeMethods.GetExtendedUdpTable(pTable, ref iOutBufLen, true, AF_INET,
                        UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0);

                    if (iRet == 0)
                    {

                        int iEntries = Marshal.ReadInt32(pTable);
                        IntPtr ptr = new IntPtr(pTable.ToInt32() + 4);

                        //Lecture de chaque entrée.
                        for (int i = 0; i < iEntries; i++)
                        {
                            //Lit l'entrée.
                            MIB_UDPROW_EX entry = (MIB_UDPROW_EX)Marshal.PtrToStructure(ptr, typeof(MIB_UDPROW_EX));

                            //Extrait les infos.
                            this._udpTable.Add(new UdpEntry(entry.dwLocalAddr, entry.dwLocalPort, entry.dwProcessId));
                            ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(MIB_UDPROW_EX)));
                        }

                        this._udpTable.Sort();
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    Marshal.FreeHGlobal(pTable);
                }
            }
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ListeningOpenPorts != null)
            {
                RefreshAllOpenedPorts();
                ListeningOpenPorts(_tcpTable, _udpTable);
            }
        }
    }
}