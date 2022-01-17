using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using JAIS.Entities;

namespace JAIS.Apps.Settings;

[App("Settings", "/Apps/Settings/Assets/Icon.png")]
public class Settings : UserControl
{
    public Settings()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}