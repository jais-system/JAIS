using System.Reflection;
using Avalonia;
using Avalonia.Platform;

namespace AppCore.Helpers;

public class AvaloniaResourceHelper
{
    public static Stream? OpenResource(string path)
    {
        Uri uri;
        
        if (path.StartsWith("avares://"))
        {
            uri = new Uri(path);
        }
        else
        {
            string assemblyName = Assembly.GetEntryAssembly()!.GetName().Name!;
            uri = new Uri($"avares://{assemblyName}/{path}");
        }

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        return assets?.Open(uri);
    }
}