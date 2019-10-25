using Architecture.Core;
using System.Linq;
using Xamarin.Essentials;

namespace Architecture
{
    public class NetworkStatusService : INetworkStatusService
    {
        public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public bool HasWifi => Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi);

        public bool HasBluetooth => Connectivity.ConnectionProfiles.Contains(ConnectionProfile.Bluetooth);
    }
}
