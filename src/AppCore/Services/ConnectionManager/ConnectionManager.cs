using System.Text;
using AppCore.Services.CoreSystem;
using Avalonia.Threading;
using Tmds.DBus;

namespace AppCore.Services.ConnectionManager;

public class ConnectionManager : IConnectionManager
{
    private readonly INetworkManager _networkManager;
    private NetworkState _networkState;

    public event EventHandler<NetworkState> NetworkStateChanged = delegate {  };

    public NetworkState NetworkState
    {
        get => _networkState;
        set
        {
            _networkState = value;
            Console.WriteLine($"Network State: {value}");

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                NetworkStateChanged.Invoke(this, value);
            });
        }
    }

    public ConnectionManager()
    {
        _networkManager = Connection.System.CreateProxy<INetworkManager>("org.freedesktop.NetworkManager", "/org/freedesktop/NetworkManager");
        
        Initialize();
    }
    
    private async void Initialize()
    {
        NetworkState = (await _networkManager.GetConnectivityAsync() == ConnectivityState.Full)
            ? NetworkState.Online
            : NetworkState.Offline;

        foreach (IDevice device in await _networkManager.GetDevicesAsync())
        {
            string interfaceName = await device.GetInterfaceAsync();
            DeviceState state = await device.GetStateAsync();

            if (state == DeviceState.Activated)
            {
                NetworkState = NetworkState.Online;
            }

            await device.WatchStateChangedAsync(
                change =>
                {
                    NetworkState = change.newState == DeviceState.Activated
                        ? NetworkState.Online
                        : NetworkState.Offline;

                    Console.WriteLine($"{interfaceName}: {change.oldState} -> {change.newState}");
                }
            );
        }
    }

    public async Task GetAccessPoints()
    {
        const uint deviceTypeWireless = 2;
        
        foreach (IDevice device in await _networkManager.GetDevicesAsync())
        {
            if (await device.GetDeviceTypeAsync() == deviceTypeWireless)
            {
                var wirelessDevice = Connection.System.CreateProxy<IWireless>("org.freedesktop.NetworkManager", device.ObjectPath);
                var accessPoints = await wirelessDevice.GetAllAccessPointsAsync();

                foreach (ObjectPath point in accessPoints)
                {
                    var accessPoint = Connection.System.CreateProxy<IAccessPoint>("org.freedesktop.NetworkManager", point);
                    Console.WriteLine($"SSID: {Encoding.UTF8.GetString(await accessPoint.GetSsidAsync())}");
                }
            }
        }
    }
}