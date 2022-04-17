using Common.Services.AppManager;
using Common.Services.ServerService;

namespace Common;

public class Startup
{
    public static void Initialize()
    {
        DependencyInjection.Configure();
        DependencyInjection.AfterBuild += OnAfterBuild;

        RegisterServices();
    }

    private static void OnAfterBuild(object? sender, EventArgs e)
    {
        DependencyInjection.Resolve<IAppManager>().Initialize("Apps");
    }

    private static void RegisterServices()
    {
        DependencyInjection.RegisterSingleton<IServerService, ServerService>();
        DependencyInjection.RegisterSingleton<IAppManager, AppManager>();
    }
}