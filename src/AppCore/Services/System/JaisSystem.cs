using System.ComponentModel;
using System.Text.Json;
using AppCore.Entities;
using AppCore.Services.System.Entities;
using AppCore.Theme;
using FluentThemeMode = AppCore.Theme.FluentThemeMode;

namespace AppCore.Services.System;

public class JaisSystem : Notifiable, IJaisSystem
{
    private JaisAppTheme? _lightTheme;
    private JaisAppTheme? _darkTheme;
    private SystemConfig? _systemConfig;
    
    public static string ConfigDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Config");
    public static string ConfigFilePath => Path.Combine(ConfigDirectory, "System.json");

    public SystemConfig Configuration => _systemConfig ??= GetConfig();

    private static void WriteConfigToFile(SystemConfig systemConfig)
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            Directory.CreateDirectory(ConfigDirectory);
        }
        
        string configSerialized = JsonSerializer.Serialize(systemConfig, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigFilePath, configSerialized);
    }

    private static SystemConfig GetConfig()
    {
        try
        {
            string filePath = ConfigFilePath;

            bool fileExists = File.Exists(filePath);

            var currentConfig = new SystemConfig();

            if (fileExists)
            {
                string rawFile = File.ReadAllText(filePath);
                var configFile = JsonSerializer.Deserialize<SystemConfig>(rawFile);

                currentConfig = configFile;
            }
            else
            {
                WriteConfigToFile(currentConfig);
            }

            return currentConfig ?? new SystemConfig();
        }
        catch(Exception exception)
        {
            Console.WriteLine($"Error while reading config file {exception}");
            return new SystemConfig();
        }
    }

    private void CurrentConfigOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Configuration.DarkMode))
        {
            ChangeTheme(Configuration.DarkMode);
        }

        WriteConfigToFile(Configuration);
    }

    public JaisSystem()
    {
        Configuration.PropertyChanged += CurrentConfigOnPropertyChanged;
    }

    public void ChangeTheme(bool dark)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (dark && _darkTheme == null)
        {
            _darkTheme = new JaisAppTheme(new Uri("avares://JAIS/App.axaml"))
            {
                Mode = FluentThemeMode.Dark
            };
        }

        if (!dark && _lightTheme == null)
        {
            _lightTheme = new JaisAppTheme(new Uri("avares://JAIS/App.axaml"))
            {
                Mode = FluentThemeMode.Light
            };
        }

        Configuration.CurrentTheme = dark ? _darkTheme! : _lightTheme!;

        // if (MainApplication.MainWindow.Styles.Count <= 0)
        // {
        //     MainApplication.MainWindow.Styles.Add(style);
        // }
        //
        // MainApplication.MainWindow.Styles[0] = style;
    }

    public void Reboot()
    {

    }

    public void Shutdown()
    {

    }
}