using Common.Core;
using JAIS.Services.SystemService;

namespace JAIS.Core;

public class AppCore
{
    public static void Initialize()
    {
        CommonCore.Initialize();

        Ioc.Register<ISystemService, SystemService>();

        Ioc.Resolve<ISystemService>()?.Initialize();
    }
}