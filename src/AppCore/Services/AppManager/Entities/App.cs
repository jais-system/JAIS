using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace AppCore.Services.AppManager.Entities;

public record App
{
    public string? Id { get; set; }
    public string? BundleId { get; set; }
    public Type? Type { get; set; }
    // public AppAttribute Attribute { get; set; }

    public string? Name { get; set; }
    public Bitmap? Icon { get; set; }

    public UserControl? Instance { get; set; }
}