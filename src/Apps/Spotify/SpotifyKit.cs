using AppCore.Exceptions;
using AppCore.Services.ConnectionManager;
using AppCore.Services.CoreSystem;
using Avalonia.Media.Imaging;
using Spotify.Entities;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Spotify;

public class SpotifyKit
{
    private readonly IConnectionManager _connectionManager;
    private SpotifyClient? _spotify;

    private SpotifyClient? Spotify => _spotify ??= Initialize();

    public SpotifyKit()
    {
        // _connectionManager = DependencyInjector.Resolve<IConnectionManager>();
        //
        // if (_connectionManager.NetworkState == NetworkState.Online)
        // {
        //     Initialize();
        // }
        //
        // _connectionManager.NetworkStateChanged += (sender, state) =>
        // {
        //     if (state == NetworkState.Online)
        //     {
        //         Initialize();
        //     }
        // };
    }

    private static SpotifyClient? Initialize()
    {
        try
        {
            string configPath = Path.Join(JaisSystem.ConfigDirectory, "Spotify.json");
            string configFile = File.ReadAllText(configPath);

            var authConfig = JsonSerializer.Deserialize<SpotifyAuthenticationDto>(configFile);

            if (authConfig == null)
            {
                throw new SystemSettingNotSetException("Spotify Authentication config is not valid");
            }

            var response = new AuthorizationCodeTokenResponse
            {
                AccessToken = authConfig.AccessToken,
                RefreshToken = authConfig.RefreshToken
            };

            SpotifyClientConfig config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new AuthorizationCodeAuthenticator(authConfig.ClientId, authConfig.ClientSecret, response))
                .WithHTTPLogger(new SimpleConsoleHTTPLogger())
                .WithRetryHandler(new SimpleRetryHandler { RetryAfter = TimeSpan.FromSeconds(1) });

            return new SpotifyClient(config);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return null;
    }

    public async IAsyncEnumerable<Playlist> GetPlaylists()
    {
        if (Spotify == null)
        {
            yield break;
        }
        
        var playlists = await Spotify.Playlists.CurrentUsers();

        await foreach (SimplePlaylist simplePlaylist in Spotify.Paginate(playlists))
        {
            var playlist = new Playlist
            {
                Id = simplePlaylist.Id,
                ContextUri = simplePlaylist.Uri,
                Name = simplePlaylist.Name
            };

            Image image = simplePlaylist.Images.FirstOrDefault(image => image.Height == 300, simplePlaylist.Images.First());
            Bitmap? loadedImage = await ImageFetcher.LoadImage(image.Url);

            if (loadedImage != null)
            {
                playlist.Image = loadedImage;
            }
            
            yield return playlist;
        }
    }

    public async Task GetRecentlyPlayed()
    {
        var recentlyPlayed = await Spotify.Player.GetRecentlyPlayed();

        await foreach (PlayHistoryItem item in Spotify.Paginate(recentlyPlayed))
        {
            if (item.Context.Type != "track")
            {
                Console.WriteLine(item.Context.Type);
                switch (item.Context.Type)
                {
                    case "album":
                        Console.WriteLine(item.Track.Album.Name);
                        break;

                    case "artist":
                        Console.WriteLine(string.Join(", ", item.Track.Artists.Select(artist => artist.Name)));
                        break;
                    
                    case "playlist":
                        Console.WriteLine(item.Context.Uri);
                        break;
                    
                    default:
                        Console.WriteLine(item.Context.Type);
                        break;
                }
                
            }
        }
    }

    public async Task<bool> Play(string contextUri)
    {
        if (Spotify == null) { return false; }

        var options = new PlayerResumePlaybackRequest
        {
            DeviceId = "02c1067e1d4ac8d3584e5b00f2758f555289b3d9",
            ContextUri = contextUri
        };
        
        return await Spotify.Player.ResumePlayback(options);
    }

    public void Pause()
    {
        Spotify?.Player.PausePlayback();
    }

    public async IAsyncEnumerable<PlaylistTrack<IPlayableItem>> GetPlaylistTracks(string playlistId)
    {
        if (Spotify != null)
        {
            await foreach (var item in Spotify.Paginate(await Spotify.Playlists.GetItems(playlistId)))
            {
                yield return item;
            }
        }
    }
}