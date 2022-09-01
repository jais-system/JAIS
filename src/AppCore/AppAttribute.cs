// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace AppCore;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class AppAttribute : Attribute
{
    public string AppName { get; }
    public string AppIcon { get; }

    public AppAttribute(string name, string iconPath)
    {
        AppName = name;
        AppIcon = iconPath;
    }
}