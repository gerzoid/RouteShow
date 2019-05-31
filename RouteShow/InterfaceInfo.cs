using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RouteShow
{
    public static class InterfaceInfo
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct IP_INTERFACE_INFO
        {
            public int NumAdapters;
            public NativeMethods.IP_ADAPTER_INDEX_MAP[] Adapter;

            public static IP_INTERFACE_INFO FromByteArray(Byte[] buffer)
            {
                unsafe
                {
                    IP_INTERFACE_INFO rv = new IP_INTERFACE_INFO();
                    int iNumAdapters = 0;
                    Marshal.Copy(buffer, 0, new IntPtr(&iNumAdapters), 4);
                    NativeMethods.IP_ADAPTER_INDEX_MAP[] adapters = new NativeMethods.IP_ADAPTER_INDEX_MAP[iNumAdapters];
                    rv.NumAdapters = iNumAdapters;
                    rv.Adapter = new NativeMethods.IP_ADAPTER_INDEX_MAP[iNumAdapters];
                    int offset = sizeof(int);
                    for (int i = 0; i < iNumAdapters; i++)
                    {
                        NativeMethods.IP_ADAPTER_INDEX_MAP map = new NativeMethods.IP_ADAPTER_INDEX_MAP();
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(map));
                        Marshal.StructureToPtr(map, ptr, false);
                        Marshal.Copy(buffer, offset, ptr, Marshal.SizeOf(map));
                        map = (NativeMethods.IP_ADAPTER_INDEX_MAP)Marshal.PtrToStructure(ptr, typeof(NativeMethods.IP_ADAPTER_INDEX_MAP));
                        Marshal.FreeHGlobal(ptr);
                        rv.Adapter[i] = map;
                        offset += Marshal.SizeOf(map);
                    }
                    return rv;
                }
            }
        }

        public static IP_INTERFACE_INFO GetInterfaceInfo()
        {
            int size = 0;
            int r = NativeMethods.GetInterfaceInfo(null, ref size);
            Byte[] buffer = new Byte[size];
            r = NativeMethods.GetInterfaceInfo(buffer, ref size);
            if (r != NativeMethods.ERROR_SUCCESS)
                throw new Exception("GetInterfaceInfo returned an error.");
            IP_INTERFACE_INFO info = IP_INTERFACE_INFO.FromByteArray(buffer);
            return info;
        }

        public static bool IpReleaseAddress(NativeMethods.IP_ADAPTER_INDEX_MAP adapter)
        {
            if (NativeMethods.IpReleaseAddress(ref adapter) == NativeMethods.ERROR_SUCCESS)
                return true;
            else
                return false;
        }

        public static void IpReleaseAllAddresses()
        {
            IP_INTERFACE_INFO info = GetInterfaceInfo();
            for (int i = 0; i < info.NumAdapters; i++)
                IpReleaseAddress(info.Adapter[i]);
        }
    }
}
