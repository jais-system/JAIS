using System.Collections.ObjectModel;
using AppCore.Entities;
using ReactiveUI;
using Spotify.Entities;
using SpotifyAPI.Web;

namespace Spotify.Views.PlaylistPage;

public class PlaylistPageViewModel : ViewModelBase
{
    private Playlist _playlist = null!;
    private ObservableCollection<PlaylistTrack<IPlayableItem>> _playlistItems = new();

    public Playlist Playlist { get => _playlist; set => this.RaiseAndSetIfChanged(ref _playlist, value); }
    public ObservableCollection<PlaylistTrack<IPlayableItem>> PlaylistItems { get => _playlistItems; set => this.RaiseAndSetIfChanged(ref _playlistItems, value); }

    public async void PlayPlaylist()
    {
        await SpotifyMainWindow.SpotifyInstance.Play(Playlist.ContextUri);
    }
}