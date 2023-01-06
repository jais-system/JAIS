using AppCore.Services.CoreSystem;

namespace AppCore.Services.ConnectionManager;

public interface IConnectionManager
{
    event EventHandler<NetworkState> NetworkStateChanged;
    NetworkState NetworkState { get; set; }
    Task GetAccessPoints();
}