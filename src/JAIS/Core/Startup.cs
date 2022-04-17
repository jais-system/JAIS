using System;
using Common;
using JAIS.Services.SystemService;

namespace JAIS.Core;

public class Startup
{
    public static void Initialize()
    {
        DependencyInjection.AfterBuild += DependencyInjectionOnAfterBuild;

        Common.Startup.Initialize();

        DependencyInjection.RegisterSingleton<ISystemService, SystemService>();

    }

    private static void DependencyInjectionOnAfterBuild(object? sender, EventArgs e)
    {
        DependencyInjection.Resolve<ISystemService>()?.Initialize();
    }
}