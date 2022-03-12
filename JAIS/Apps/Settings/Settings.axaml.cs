using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Common.Core;
using Common.Services.ServerService;
using JAIS.Entities;

namespace JAIS.Apps.Settings;

[App("Settings", "/Apps/Settings/Assets/Icon.png")]
public class Settings : UserControl
{
    private readonly IServerService _serverService;
    private SettingsBindings Bindings { get; } = new SettingsBindings();

    public Settings()
    {
        _serverService = Ioc.Resolve<IServerService>();

        Initialize();

        DataContext = this;
        InitializeComponent();
    }

    private async Task Initialize()
    {
        Bindings.IpAddress = await _serverService.GetIpAddress();
        Bindings.Port = _serverService.Port;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}