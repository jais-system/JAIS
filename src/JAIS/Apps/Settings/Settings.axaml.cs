using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Common.Core;
using Common.Services.ServerService;
using JAIS.Entities;
using JAIS.Services.SystemService;
using JAIS.Services.SystemService.Entities;

namespace JAIS.Apps.Settings;

[App("Settings", "/Apps/Settings/Assets/Icon.png")]
public class Settings : UserControl
{
    private readonly IServerService _serverService;
    private SettingsBindings Bindings { get; } = new SettingsBindings();

    public SystemConfig Config { get; set; }

    public Settings()
    {
        // var systemService = Ioc.Resolve<ISystemService>();
        // Config = systemService.CurrentSystemConfig;

        Initialize();

        DataContext = this;
        InitializeComponent();
    }

    private async Task Initialize()
    {
        // Bindings.IpAddress = await _serverService.GetIpAddress();
        // Bindings.Port = _serverService.Port;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void ChangeThemeClicked(object? sender, RoutedEventArgs e)
    {
        // Config.DarkMode = !Config.DarkMode;
    }
}