using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using JAIS.Entities;

namespace JAIS.Apps.Spotify;

[App("Spotify", "/Apps/Spotify/Assets/Icon.png")]
public class Spotify : UserControl
{
    public Spotify()
    {
        InitializeComponent();
        Test();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void Test()
    {
        // var config = SpotifyClientConfig.CreateDefault();
        // var request = new ClientCredentialsRequest("6691748ea0f84d8284ca03b06fda129f", "53c7746d4c944f6dbf84827d1060db68");
        // var response = await new OAuthClient(config).RequestToken(request);
        // var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
        // FullTrack track = await spotify.Tracks.Get("2LV5joNDrsyuXEh4FBARVK");
        //
        // string id = (await spotify.UserProfile.Current()).Id;
        //
        // var playlists = await spotify.Playlists.GetUsers(id);
        //
        // foreach (SimplePlaylist playlist in playlists.Items)
        // {
        //     Console.WriteLine(playlist.Name);
        // }
        //
        // Console.WriteLine(track.Name);
    }
}