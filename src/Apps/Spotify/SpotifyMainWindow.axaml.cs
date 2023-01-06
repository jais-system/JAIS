using AppCore;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Spotify.Views.Home;

namespace Spotify;

[App("Spotify", "avares://Spotify/Assets/Icon.png")]
public class SpotifyMainWindow : UserControl
{
    private static UserControl? _home;
    private static Border? _content;
    public static SpotifyKit SpotifyInstance => new SpotifyKit();
    
    public SpotifyMainWindow()
    {
        _home = new Home();
        
        _content = new Border
        {
            Child = _home
        };
        
        Content = _content;
        
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static void SetContent(UserControl userControl)
    {
        if (_content != null)
        {
            _content.Child = userControl;
        }
    }
    
    public static void SetHome()
    {
        if (_content != null)
        {
            _content.Child = _home;
        }
    }
}