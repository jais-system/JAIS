using Common.Services.ServerService;

namespace Common.Core;

public class CommonCore
{
    public static void Initialize()
    {
        Ioc.Register<IServerService, ServerService>();
    }
}