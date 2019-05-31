using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using NetworkPortsLib.Type;
using System.Text;

namespace NetworkPortsLib
{
    /// <summary>
    /// Table des interfaces IP.
    /// </summary>
    public class AdaptersTable
    {
        private Dictionary<int, NetworkInterface> _adapters = new Dictionary<int, NetworkInterface>();

        /// <summary>
        /// Obtient la liste des interfaces IP.
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, NetworkInterface> GetAdapters()
        {
            return _adapters;
        }

        /// <summary>
        /// Obtient l'interface IP avec l'index <paramref name="interfaceIndex"/>.
        /// </summary>
        /// <param name="interfaceIndex">Index de l'interface.</param>
        /// <returns></returns>
        public NetworkInterface GetAdapter(int interfaceIndex)
        {
            NetworkInterface ni = null;
            _adapters.TryGetValue(interfaceIndex, out ni);
            return ni;
        }
      
        
        /// <summary>
        /// Obtient l'index de l'interface IP <paramref name="networkInterface"/>.
        /// </summary>
        /// <param name="networkInterface">Interface IP.</param>
        /// <returns></returns>                       
        public int GetAdapterIndex(NetworkInterface networkInterface)
        {
            //int iIdx = _adapters.First(a => a.Value == networkInterface).Key;
            int iIdx = 0;
            return iIdx;
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        public AdaptersTable()
        {
            IntPtr pTable = IntPtr.Zero;
            int iOutBufLen = 0;
            int iRet = 0;

            //1er appel détermine la taille de la table.
            iRet = NativeMethods.GetIfTable(IntPtr.Zero, ref iOutBufLen, true);

            NetworkInterface[] intfs = NetworkInterface.GetAllNetworkInterfaces();
            for (int x=0;x<=intfs.Length-1;x++)
            {
                int index = 0;
                if (intfs[x].Supports(NetworkInterfaceComponent.IPv6))
                {
                    IPInterfaceProperties prop = intfs[x].GetIPProperties();
                    index = prop.GetIPv6Properties().Index;
                }
                else
                if (intfs[x].Supports(NetworkInterfaceComponent.IPv4))
                {
                    if (intfs[x].NetworkInterfaceType == NetworkInterfaceType.Loopback)
                        index = NetworkInterface.LoopbackInterfaceIndex;
                    else
                    {
                        IPInterfaceProperties prop = intfs[x].GetIPProperties();
                        index = prop.GetIPv4Properties().Index;
                    }
                }
                this._adapters.Add(index, intfs[x]);
            }
            
            /*try
            {
                pTable = Marshal.AllocHGlobal(iOutBufLen);
                iRet = NativeMethods.GetIfTable(pTable, ref iOutBufLen, true);

                //Lecture OK.
                if (iRet == 0)
                {
                    //Récupère le nombre d'entrée dans la table.
                    int iEntries = Marshal.ReadInt32(pTable);

                    IntPtr pInterface = new IntPtr(pTable.ToInt32() + 4);

                    //pour chaque entrée.
                    for (int i = 0; i < iEntries; i++)
                    {
                        //Lit l'entrée.
                        MIB_IFROW entry = (MIB_IFROW)Marshal.PtrToStructure(pInterface, typeof(MIB_IFROW));

                        //Lit l'entrée à partir de son index.
                        MIB_IFROW NetIntInfo = new MIB_IFROW();
                        NetIntInfo.dwIndex = entry.dwIndex;
//                        string desc = System.Text.Encoding.ASCII.GetString(entry.bDescr, 0, NetIntInfo.dwDescrLen - 1);


//                        this._adapters.Add(entry.dwIndex, intfs[entry.dwIndex].);

                        if (NativeMethods.GetIfEntry(ref NetIntInfo) == 0)
                        {
                            //Récupère la description.

                            this._adapters.Add(entry.dwIndex, ni);
                            //Essaie de faire correspondre l'interface à celle de .Net
/*                            foreach (NetworkInterface ni in intfs)
                            {
                                //intfs[0].GetIPProperties().GetIPv4Properties().Index;
                                int a = ni.GetIPProperties().GetIPv4Properties().Index;
                                if (ni.Description == desc)
                                {
                                    //this._adapters.Add(entry.dwIndex, ni);
                                    break;
                                }
                            }
                        }
                        
                        //Pointeur prochaine interface.
                        pInterface = new IntPtr(pInterface.ToInt32() + Marshal.SizeOf(typeof(MIB_IFROW)));
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Marshal.FreeHGlobal(pTable);
            }*/
        }
    }
}