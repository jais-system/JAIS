using System.Collections.ObjectModel;
using AppCore.Entities;
using ReactiveUI;
using Spotify.Entities;

namespace Spotify.Views.Home;

internal class HomeViewModel : ViewModelBase
{
    private bool _isOnline;
    private bool _loading;
    
    public ObservableCollection<Playlist> Playlists { get; set; } = new();
    
    public bool IsOnline { get => _isOnline; set => this.RaiseAndSetIfChanged(ref _isOnline, value); }
    public bool Loading { get => _loading; set => this.RaiseAndSetIfChanged(ref _loading, value); }

    public void OpenPlaylist(Playlist test)
    {
        SpotifyMainWindow.SetContent(new PlaylistPage.PlaylistPage
        {
            Playlist = test
        });
    }
}