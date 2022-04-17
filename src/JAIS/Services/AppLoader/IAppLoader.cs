using System.Collections.Generic;
using JAIS.Entities;

namespace JAIS.Services.AppLoader;

public interface IAppLoader
{
    IEnumerable<AppInfo> LoadApp(string appBundlePath);
}