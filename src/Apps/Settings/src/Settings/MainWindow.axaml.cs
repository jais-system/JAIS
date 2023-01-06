using AppCore;
using AppCore.Services.CoreSystem;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Settings;

[App("Settings", "avares://Settings/Assets/Icon.png")]
public class MainWindow : UserControl
{
    private readonly IJaisSystem _jaisSystem;

    public MainWindow()
    {
        _jaisSystem = DependencyInjector.Resolve<IJaisSystem>();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnChangeThemeClicked(object? sender, RoutedEventArgs e)
    {
        _jaisSystem.Configuration.DarkMode = !_jaisSystem.Configuration.DarkMode;
    }
}