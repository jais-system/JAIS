using AppCore.Entities;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace AppCore.Services.AppManager.Entities;

public class App : Notifiable
{
    private bool _isActive = false;
    
    public string? Id { get; set; }
    public string? BundleId { get; set; }
    public Type? Type { get; set; }
    // public AppAttribute Attribute { get; set; }

    public string? Name { get; set; }
    public Bitmap? Icon { get; set; }

    public UserControl? Instance { get; set; }
    
    public bool IsActive { get => _isActive; set => Set(ref _isActive, value); }
}