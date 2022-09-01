namespace AppCore.Services.AppManager.Entities;

public record SideloadingRequest(string AppName, string Author, string Version)
{
    public string AppName { get; set; } = AppName;
    public string Author { get; set; } = Author;
    public string Version { get; set; } = Version;
}