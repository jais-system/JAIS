using JAIS.Entities;

namespace JAIS.Apps.Settings;

public class SettingsBindings : Notifiable
{
    private string _ipAddress;
    private int _port;

    public string IpAddress { get => _ipAddress; set => Set(ref _ipAddress, value); }
    public int Port { get => _port; set => Set(ref _port, value); }
}