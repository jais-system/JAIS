using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Spotify.Entities;
using SpotifyAPI.Web;

namespace Spotify.Views.PlaylistPage;

public partial class PlaylistPage : UserControl
{
    private readonly PlaylistPageViewModel _viewModel;

    public Playlist Playlist
    {
        set
        {
            _viewModel.Playlist = value;
            InitializeComponent();
        }
    }

    public PlaylistPage()
    {
        _viewModel = new PlaylistPageViewModel();
        
        DataContext = _viewModel;
        AvaloniaXamlLoader.Load(this);
    }

    private async void InitializeComponent()
    {
        await foreach (var item in SpotifyMainWindow.SpotifyInstance.GetPlaylistTracks(_viewModel.Playlist.Id))
        {
            _viewModel.PlaylistItems.Add(item);
            
            // if (item.Track is FullTrack track)
            // {
            //     Console.WriteLine(track.Artists);
            // }
        }
    }

    private void Back(object? sender, RoutedEventArgs e)
    {
        SpotifyMainWindow.SetHome();
    }
}