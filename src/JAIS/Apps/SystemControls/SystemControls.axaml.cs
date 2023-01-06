using AppCore;
using AppCore.Services.CoreSystem;
using AppCore.Services.CoreSystem.Entities;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace JAIS.Apps.SystemControls;

public class SystemControls : UserControl
{
    public SystemConfig Config { get; set; }

    public SystemControls()
    {
        var systemService = DependencyInjector.Resolve<IJaisSystem>();
        Config = systemService.Configuration;

        DataContext = this;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void ChangeThemeClicked(object? sender, RoutedEventArgs e)
    {
        Config.DarkMode = !Config.DarkMode;
    }
}