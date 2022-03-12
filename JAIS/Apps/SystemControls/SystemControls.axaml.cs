using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Common;
using Common.Core;
using JAIS.Core;
using JAIS.Services.SystemService;
using JAIS.Services.SystemService.Entities;
using Theme;

namespace JAIS.Apps.SystemControls;

public class SystemControls : UserControl
{
    public SystemConfig Config { get; set; }

    public SystemControls()
    {
        var systemService = Ioc.Resolve<ISystemService>();
        Config = systemService.CurrentSystemConfig;

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