using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using JAIS.Entities;

namespace JAIS.Apps.Spotify;

[App("Spotify", "/Apps/Spotify/Assets/Icon.png")]
public class Spotify : UserControl
{
    public Spotify()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}