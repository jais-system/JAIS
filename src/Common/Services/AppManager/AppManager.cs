using System.IO.Compression;
using System.Reflection;
using System.Text.Json;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Common.Services.AppManager.Entities;
using JaisAppCore;

namespace Common.Services.AppManager;

public class AppManager : IAppManager
{
    private const string AppListFileName = "Apps.json";

    private string _appDirectory = "";
    private string _appListFilePath = "";
    private HashSet<AppInfo>? _appList;
    private Func<SideloadingRequest, bool>? _onSideloadingRequestCallback;

    public event EventHandler<AppInfo> OnNewAppInstalled = delegate { };

    public HashSet<AppInfo> AppList
    {
        get => _appList ??= GetAppListFromFile();
        set
        {
            _appList = value;
            SaveAppList();
        }
    }

    public void Initialize(string appDirectory)
    {
        if (!Directory.Exists(appDirectory))
        {
            Directory.CreateDirectory(appDirectory);
        }

        _appDirectory = Path.Join(Directory.GetCurrentDirectory(), appDirectory);
        _appListFilePath = Path.Join(_appDirectory, AppListFileName);
    }

    private HashSet<AppInfo> GetAppListFromFile()
    {
        if (File.Exists(_appListFilePath))
        {
            string currentAppListText = File.ReadAllText(_appListFilePath);
            return JsonSerializer.Deserialize<HashSet<AppInfo>>(currentAppListText)!;
        }

        return new HashSet<AppInfo>();
    }

    private void SaveAppList()
    {
        string serialized = JsonSerializer.Serialize(AppList);
        File.WriteAllText(_appListFilePath, serialized);
    }

    private void AddOrUpdateApp(AppInfo newApp)
    {
        AppInfo? existingApp = AppList.FirstOrDefault(app => app.BundleId == newApp.BundleId);

        if (existingApp != null)
        {
            AppList.Remove(existingApp);
        }

        AppList.Add(newApp);
        SaveAppList();
    }

    public void RegisterSideloadingRequestHandler(Func<SideloadingRequest, bool> callback)
    {
        _onSideloadingRequestCallback = callback;
    }

    public bool RequestSideloadingApp(SideloadingRequest request)
    {
        return _onSideloadingRequestCallback?.Invoke(request) ?? false;
    }

    public IEnumerable<App> GetAppsFromAssembly(Assembly assembly, string bundleId)
    {
        var apps = new List<App>();

        foreach (Type type in assembly.GetTypes())
        {
            object[] attributes = type.GetCustomAttributes(typeof(AppAttribute), true);

            if (attributes.Any())
            {
                foreach (object rawAttribute in attributes)
                {
                    var attribute = (AppAttribute) rawAttribute;

                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    Stream? asset = assets?.Open(new Uri(attribute.AppIcon));

                    apps.Add(new App
                    {
                        BundleId = bundleId,
                        Type = type,
                        Name = attribute.AppName,
                        Icon = new Bitmap(asset)
                    });

                }
            }
        }

        return apps;
    }

    public IEnumerable<App> LoadApps()
    {
        var apps = new List<App>();

        foreach (AppInfo appInfo in AppList)
        {
            string dllFile = appInfo.DllPath;

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllFile);
                apps.AddRange(GetAppsFromAssembly(assembly, appInfo.BundleId));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        return apps;
    }

    public IEnumerable<App> LoadApp(AppInfo appInfo)
    {
        try
        {
            byte[] assemblyFile = File.ReadAllBytes(appInfo.DllPath);

            Assembly assembly = Assembly.Load(assemblyFile);

            return GetAppsFromAssembly(assembly, appInfo.BundleId);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return Array.Empty<App>();
    }

    public async Task<InstallAppResult> InstallApp(string fileName, Stream appFileStream)
    {
        await using var fileStream = new MemoryStream();
        await appFileStream.CopyToAsync(fileStream);
        appFileStream.Close();

        using var archive = new ZipArchive(fileStream);

        Stream? appInfoFromZip = archive.GetEntry("AppInfo.json")?.Open();

        if (appInfoFromZip == null)
        {
            return InstallAppResult.AppInfoInvalid;
        }

        var packagedAppInfo = await JsonSerializer.DeserializeAsync<PackagedAppInfo>(appInfoFromZip);

        if (packagedAppInfo == null)
        {
            Console.WriteLine("AppInfo.json is null");
            return InstallAppResult.AppInfoInvalid;
        }

        string appDirectoryPath = Path.Join(_appDirectory, packagedAppInfo.BundleId);

        if (Directory.Exists(appDirectoryPath))
        {
            Directory.Delete(appDirectoryPath, true);
        }

        archive.ExtractToDirectory(appDirectoryPath);

        string dllPath = Path.Join(appDirectoryPath, packagedAppInfo.DllPath);
        var appInfo = new AppInfo(packagedAppInfo.AppName ?? "", packagedAppInfo.BundleId ?? "", packagedAppInfo.Version ?? "", appDirectoryPath, dllPath);

        AddOrUpdateApp(appInfo);

        OnNewAppInstalled.Invoke(this, appInfo);

        return InstallAppResult.Success;
    }
}