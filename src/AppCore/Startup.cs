using AppCore.Services.AppManager;
using AppCore.Services.OBD;
using AppCore.Services.System;

namespace AppCore;

public class Startup
{
    public static void Initialize()
    {
        DependencyInjector.RegisterSingleton<IJaisSystem, JaisSystem>();
        DependencyInjector.RegisterSingleton<IAppManager, AppManager>();
        DependencyInjector.RegisterSingleton<IOBDCommunicator, OBDCommunicator>();
    }
}