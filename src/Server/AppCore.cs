using Common;
using Common.Core;
using Server.Services.KeyService;

namespace Server;

public class AppCore
{
    public static void Initialize()
    {
        Ioc.Register<IKeyService, KeyService>();
    }
}