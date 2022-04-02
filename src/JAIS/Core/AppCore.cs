using JAIS.Services.SystemService;

namespace JAIS.Core;

public class AppCore
{
    public static void Initialize()
    {
        Ioc.Configure();
        Ioc.RegisterSingleton<ISystemService, SystemService>();

        Ioc.Build();

        Ioc.Resolve<ISystemService>()?.Initialize();
    }
}