using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using JAIS.Entities;
// using SpotifyAPI.Web;

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
        // var spotify = new SpotifyClient("BQDDyorsv7zbOhy1hZaiOhBHs_d9hs9TKGIIqijUPJC9mHOwcJdC5z9BYy_dREdT6u0kpYu8Um-sN-7jzyJFWbOahdgwnxGbnsPadvomZh7TFj6FXbWqQLFpmhjxbwJRZRY4RINf39J0S5LOgPxCEqq8mp-x");
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