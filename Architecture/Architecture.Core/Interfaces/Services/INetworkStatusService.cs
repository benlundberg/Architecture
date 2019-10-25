namespace Architecture.Core
{
    public interface INetworkStatusService
    {
        bool IsConnected { get; }
        bool HasWifi { get; }
        bool HasBluetooth { get; }
    }
}
