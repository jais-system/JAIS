using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using JAIS.Entities;
using JAIS.Services.SystemService;
using AppInfo = Common.Services.AppManager.Entities.AppInfo;

namespace JAIS.Services.AppLoader;

public class AppLoader : IAppLoader
{
    private readonly string _appListFilePath;

    public AppInfo[] Apps { get; private set; } = Array.Empty<AppInfo>();

    public AppLoader(ISystemService systemService)
    {
        string configDirectory = systemService.ConfigDirectory;
        _appListFilePath = Path.Join(configDirectory, "Apps.json");
        GetInstalledApps().Wait();
    }

    private async Task GetInstalledApps()
    {
        if (!File.Exists(_appListFilePath))
        {
            return;
        }

        string appListFileText = await File.ReadAllTextAsync(_appListFilePath);
        var apps = JsonSerializer.Deserialize<List<AppInfo>>(appListFileText);

        Apps = apps?.ToArray() ?? Array.Empty<AppInfo>();
    }

    public IEnumerable<Entities.AppInfo> LoadApp(string appBundlePath)
    {
        Assembly dllFile = Assembly.LoadFile(appBundlePath);

        var apps =
            from type in dllFile.GetTypes()
            let attributes = type.GetCustomAttributes(typeof(AppAttribute), true)
            where attributes is { Length: > 0 }
            select new Entities.AppInfo { Type = type, Attribute = attributes.Cast<AppAttribute>().First() };

        return apps;
    }
}