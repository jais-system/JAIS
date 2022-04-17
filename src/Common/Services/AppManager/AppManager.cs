using System.IO.Compression;
using System.Text.Json;
using Common.Services.AppManager.Entities;

namespace Common.Services.AppManager;

public class AppManager : IAppManager
{
    private const string AppListFileName = "Apps.json";

    private string _appDirectory = "";
    private Func<SideloadingRequest, bool>? _onSideloadingRequestCallback;

    public event EventHandler<AppInfo> OnNewAppInstalled = delegate { };

    public void Initialize(string appDirectory)
    {
        if (!Directory.Exists(appDirectory))
        {
            Directory.CreateDirectory(appDirectory);
        }

        _appDirectory = appDirectory;
    }

    public void RegisterSideloadingRequestHandler(Func<SideloadingRequest, bool> callback)
    {
        _onSideloadingRequestCallback = callback;
    }

    public bool RequestSideloadingApp(SideloadingRequest request)
    {
        return _onSideloadingRequestCallback?.Invoke(request) ?? false;
    }

    public void LoadApps()
    {
        throw new NotImplementedException();
    }

    public async Task<InstallAppResult> InstallApp(string fileName, Stream appFileStream)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string appDirectoryPath = Path.Join(_appDirectory, fileNameWithoutExtension);
        string appInfoPath = Path.Join(appDirectoryPath, "AppInfo.json");

        if (Directory.Exists(appDirectoryPath))
        {
            Directory.Delete(appDirectoryPath, true);
        }

        await using var fileStream = new MemoryStream();
        await appFileStream.CopyToAsync(fileStream);
        appFileStream.Close();

        using var archive = new ZipArchive(fileStream);
        archive.ExtractToDirectory(appDirectoryPath);

        string appInfoText = await File.ReadAllTextAsync(appInfoPath);

        var packagedAppInfo = JsonSerializer.Deserialize<PackagedAppInfo>(appInfoText);

        if (packagedAppInfo == null)
        {
            Console.WriteLine("AppInfo.json is null");
            return InstallAppResult.UnknownError;
        }

        string appListPath = Path.Join(_appDirectory, AppListFileName);

        var appList = new HashSet<AppInfo>();

        if (File.Exists(appListPath))
        {
            string currentAppListText = await File.ReadAllTextAsync(appListPath);
            appList = JsonSerializer.Deserialize<HashSet<AppInfo>>(currentAppListText)!;
        }

        string dllPath = Path.Join(appDirectoryPath, packagedAppInfo.DllPath);
        var appInfo = new AppInfo(packagedAppInfo.AppName ?? "", packagedAppInfo.Version ?? "", appDirectoryPath, dllPath);
        appList.Add(appInfo);

        string serialized = JsonSerializer.Serialize(appList);
        await File.WriteAllTextAsync(appListPath, serialized);

        OnNewAppInstalled.Invoke(this, appInfo);

        return InstallAppResult.Success;
    }
}