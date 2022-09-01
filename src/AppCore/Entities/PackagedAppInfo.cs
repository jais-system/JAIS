using System.Text.Json.Serialization;

namespace AppCore.Entities;

public record PackagedAppInfo(string? AppName, string? Author, string? Version, string? DllPath)
{
    [JsonPropertyName("appName")]
    public string? AppName { get; set; } = AppName;

    [JsonPropertyName("author")]
    public string? Author { get; set; } = Author;

    [JsonPropertyName("version")]
    public string? Version { get; set; } = Version;

    [JsonPropertyName("dllPath")]
    public string? DllPath { get; set; } = DllPath;

    [JsonPropertyName("bundleId")]
    public string? BundleId { get; set; }
}