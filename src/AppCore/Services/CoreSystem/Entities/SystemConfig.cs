using System.Text.Json.Serialization;
using AppCore.Entities;
using AppCore.Theme;

namespace AppCore.Services.CoreSystem.Entities;

public record SystemConfigDto(
    bool DarkMode,
    double Volume,
    string? LastUsedApp,
    string? ObdSerialPort
);

public class SystemConfig : Notifiable
{
    private bool _darkMode;
    private double _volume;
    private string? _lastUsedApp;
    private string? _obdSerialPort;
    private JaisAppTheme? _currentTheme;

    public bool DarkMode { get => _darkMode; set => Set(ref _darkMode, value); }
    public double Volume { get => _volume; set => Set(ref _volume, value); }
    public string? LastUsedApp { get => _lastUsedApp; set => Set(ref _lastUsedApp, value); }
    public string? ObdSerialPort { get => _obdSerialPort; set => Set(ref _obdSerialPort, value); }
    
    [JsonIgnore]
    public JaisAppTheme? CurrentTheme { get => _currentTheme; set => Set(ref _currentTheme, value); }

    public SystemConfigDto ToDto()
    {
        return new SystemConfigDto(
            DarkMode,
            Volume,
            LastUsedApp,
            ObdSerialPort
        );
    }

    public SystemConfig FromDto(SystemConfigDto dto)
    {
        DarkMode = dto.DarkMode;
        Volume = dto.Volume;
        LastUsedApp = dto.LastUsedApp;
        ObdSerialPort = dto.ObdSerialPort;

        return this;
    }
}