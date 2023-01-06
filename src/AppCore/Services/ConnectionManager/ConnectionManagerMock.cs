using AppCore.Services.CoreSystem;

namespace AppCore.Services.ConnectionManager;

public class ConnectionManagerMock : IConnectionManager
{
    public event EventHandler<NetworkState>? NetworkStateChanged = delegate {  };
    public NetworkState NetworkState { get; set; } = NetworkState.Online;

    public Task GetAccessPoints()
    {
        return Task.CompletedTask;
    }
}