using System.Threading.Tasks;
using JAIS.Services.SystemService;

namespace JAIS.Core;

public class AppCore
{
    public static void Initialize()
    {
        Ioc.Register<ISystemService, SystemService>();

        Ioc.Resolve<ISystemService>()?.Initialize();
    }
}