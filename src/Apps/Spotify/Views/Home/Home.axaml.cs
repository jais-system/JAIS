using AppCore;
using AppCore.Services.ConnectionManager;
using AppCore.Services.CoreSystem;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Spotify.Entities;

namespace Spotify.Views.Home;

internal class Home : UserControl
{
    private readonly SpotifyKit _spotify;
    private readonly HomeViewModel _viewModel;
    private bool _initialized;


    public Home()
    {
        _spotify = SpotifyMainWindow.SpotifyInstance;
        _viewModel = new HomeViewModel();

        DataContext = _viewModel;
        AvaloniaXamlLoader.Load(this);

        var connectionManager = DependencyInjector.Resolve<IConnectionManager>();

        if (connectionManager.NetworkState == NetworkState.Online)
        {
            _viewModel.IsOnline = true;
            _initialized = true;
            Initialize();
        }

        connectionManager.NetworkStateChanged += (sender, state) =>
        {
            _viewModel.IsOnline = true;

            if (!_initialized)
            {
                if (state == NetworkState.Online)
                {
                    _initialized = true;
                    Initialize();
                }
            }
            
        };
    }

    private async void Initialize()
    {
        _viewModel.Loading = true;
        await foreach (Playlist playlist in _spotify.GetPlaylists())
        {
            _viewModel.Loading = false;
            _viewModel.Playlists.Add(playlist);
        }
    }
}