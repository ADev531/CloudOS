using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public static class CloudOSNetwork
    {
        public static bool IsConnected { get; private set; } = false;

        public static void Init()
        {
            DHCPClient dhcp = new();
            int connect = dhcp.SendDiscoverPacket();
            if (connect == -1)
            {
                IsConnected = false;
            } 
            else
            {
                IsConnected = true;
            }
        }
    }
}
