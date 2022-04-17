using Common;
using RestServer.Services.KeyService;

namespace RestServer;

public class Startup
{
    public static void Initialize()
    {
        DependencyInjection.RegisterSingleton<IKeyService, KeyService>();
    }
}