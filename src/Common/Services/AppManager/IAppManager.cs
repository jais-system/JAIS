using System.Reflection;
using Common.Services.AppManager.Entities;

namespace Common.Services.AppManager;

public interface IAppManager
{
    event EventHandler<AppInfo> OnNewAppInstalled;

    void Initialize(string appDirectory);

    void RegisterSideloadingRequestHandler(Func<SideloadingRequest, bool> callback);
    bool RequestSideloadingApp(SideloadingRequest request);
    IEnumerable<App> LoadApps();
    IEnumerable<App> LoadApp(AppInfo appInfo);
    IEnumerable<App> GetAppsFromAssembly(Assembly assembly, string bundleId);
    Task<InstallAppResult> InstallApp(string fileName, Stream appFileStream);
}