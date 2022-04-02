using JAIS.Entities;

namespace JAIS.Services.SystemService.Entities;

public class SystemConfig : Notifiable
{
    private bool _darkMode;
    private double _volume;
    private string _lastUsedApp;

    public bool DarkMode { get => _darkMode; set => Set(ref _darkMode, value); }
    public double Volume { get => _volume; set => Set(ref _volume, value); }
    public string LastUsedApp { get => _lastUsedApp; set => Set(ref _lastUsedApp, value); }
}