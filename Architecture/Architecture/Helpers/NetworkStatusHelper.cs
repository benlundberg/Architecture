using Architecture.Core;
using Plugin.Connectivity;
using System.Linq;

namespace Architecture
{
    public class NetworkStatusHelper : INetworkStatusHelper
    {
        public bool IsConnected => CrossConnectivity.Current.IsConnected;

        public bool HasWifi => CrossConnectivity.Current.ConnectionTypes?.Contains(Plugin.Connectivity.Abstractions.ConnectionType.WiFi) == true;

        public bool HasBluetooth => CrossConnectivity.Current.ConnectionTypes?.Contains(Plugin.Connectivity.Abstractions.ConnectionType.Bluetooth) == true;
    }
}
