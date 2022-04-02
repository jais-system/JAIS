using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using JAIS.Services.SystemService.Entities;
using Theme;

namespace JAIS.Services.SystemService;

public class SystemService : ISystemService
{
    private CustomTheme? _lightTheme;
    private CustomTheme? _darkTheme;

    public SystemConfig CurrentSystemConfig { get; set; }

    public string ConfigDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Config");
    public string ConfigFilePath => Path.Combine(ConfigDirectory, "System.json");

    private void WriteConfigToFile(SystemConfig systemConfig)
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            Directory.CreateDirectory(ConfigDirectory);
        }

        string configSerialized = JsonSerializer.Serialize(systemConfig, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigFilePath, configSerialized);
    }

    private async Task<SystemConfig> GetConfig()
    {
        try
        {
            string filePath = ConfigFilePath;

            bool fileExists = File.Exists(filePath);

            var currentConfig = new SystemConfig();

            if (fileExists)
            {
                string rawFile = await File.ReadAllTextAsync(filePath);
                var configFile = JsonSerializer.Deserialize<SystemConfig>(rawFile);

                currentConfig = configFile;
            }
            else
            {
                WriteConfigToFile(currentConfig);
            }

            return currentConfig;
        }
        catch(Exception)
        {
            return new SystemConfig();
        }
    }

    private void CurrentConfigOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CurrentSystemConfig.DarkMode))
        {
            ChangeTheme(CurrentSystemConfig.DarkMode);
        }

        WriteConfigToFile(CurrentSystemConfig);
    }

    public async Task Initialize()
    {
        CurrentSystemConfig = await GetConfig();
        CurrentSystemConfig.PropertyChanged += CurrentConfigOnPropertyChanged;
    }

    public void ChangeTheme(bool dark)
    {
        if (dark && _darkTheme == null)
        {
            _darkTheme = new CustomTheme(new Uri("avares://JAIS/App.axaml"))
            {
                Mode = FluentThemeMode.Dark
            };
        }

        if (!dark && _lightTheme == null)
        {
            _lightTheme = new CustomTheme(new Uri("avares://JAIS/App.axaml"))
            {
                Mode = FluentThemeMode.Light
            };
        }

        CustomTheme style = dark ? _darkTheme! : _lightTheme!;

        if (App.MainWindow.Styles.Count <= 0)
        {
            App.MainWindow.Styles.Add(style);
        }

        App.MainWindow.Styles[0] = style;
    }

    public void Reboot()
    {

    }

    public void Shutdown()
    {

    }
}