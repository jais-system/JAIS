using System.Text.Json.Serialization;

namespace AppCore.Services.AppManager.Entities;

public record AppInfo(string Name, string BundleId, string Version, string AppDirectoryPath, string DllPath)
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = Name;

    [JsonPropertyName("bundleId")]
    public string BundleId { get; set; } = BundleId;

    [JsonPropertyName("version")]
    public string Version { get; set; } = Version;

    [JsonPropertyName("appDirectoryPath")]
    public string AppDirectoryPath { get; set; } = AppDirectoryPath;

    [JsonPropertyName("dllPath")]
    public string DllPath { get; set; } = DllPath;
}