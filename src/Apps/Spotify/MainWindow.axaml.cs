using AppCore;
using AppCore.Services.System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpotifyAPI.Web;

namespace Spotify;

[App("Spotify", "avares://Spotify/Assets/Icon.png")]
public class MainWindow : UserControl
{
    private readonly IJaisSystem _jaisSystem;
    private CursorPaging<PlayHistoryItem> _recentlyPlayed;

    public MainWindow()
    {
        _jaisSystem = DependencyInjector.Resolve<IJaisSystem>();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Test();
        AvaloniaXamlLoader.Load(this);
    }

    private async Task Test()
    {
        var spotify = new SpotifyClient("BQA-EThkeoFz7e1qM5tQ8ZubqXF0x3KvkCretyZuGRvLcq7_XbOF5tKWnZeKId4nb3cBkepwiolGBnXNH9CXbNXMNDzCZltgFCtgyl0VyXPZDLAkh-GdWl9rGEWlwHQ0pMkgkCJuZFmLnlPsl7wD85v_jSFycDQejIN4u7DvJFnICEGrPSYP4YMEQd97Xg_cAGf1v3a1wiEKsng9GouKqtF-77A");
        
        PrivateUser me = await spotify.UserProfile.Current();
        Console.WriteLine($"Hello there {me.DisplayName}");
    }
}