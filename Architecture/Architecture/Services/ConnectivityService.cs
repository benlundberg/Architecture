using Architecture.Core;
using Xamarin.Essentials;

namespace Architecture
{
    public class ConnectivityService : IConnectivityService
    {
        public bool IsConnected => Connectivity.NetworkAccess != NetworkAccess.None;
    }
}
