using AppCore.Services.AppManager;
using AppCore.Services.ConnectionManager;
using AppCore.Services.CoreSystem;
using AppCore.Services.OBD;

namespace AppCore;

public class Startup
{
    public static void Initialize()
    {
        DependencyInjector.RegisterSingleton<IJaisSystem, JaisSystem>();
        DependencyInjector.RegisterSingleton<IAppManager, AppManager>();
        DependencyInjector.RegisterSingleton<IOBDCommunicator, OBDCommunicator>();

        if (OperatingSystem.IsLinux())
        {
            DependencyInjector.RegisterSingleton<IConnectionManager, ConnectionManager>();
        }
        else
        {
            DependencyInjector.RegisterSingleton<IConnectionManager, ConnectionManagerMock>();
        }
    }
}