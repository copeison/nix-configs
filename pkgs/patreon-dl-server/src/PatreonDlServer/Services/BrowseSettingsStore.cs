using System.Text.Json;
using PatreonDlServer.Models;

namespace PatreonDlServer.Services;

public sealed class BrowseSettingsStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    private readonly ServerRuntime _runtime;

    public BrowseSettingsStore(ServerRuntime runtime)
    {
        _runtime = runtime;
    }

    public BrowseSettings Get()
    {
        var settingsPath = GetSettingsPath();
        if (!File.Exists(settingsPath))
        {
            return new BrowseSettings();
        }

        var json = File.ReadAllText(settingsPath);
        return JsonSerializer.Deserialize<BrowseSettings>(json, SerializerOptions) ?? new BrowseSettings();
    }

    public BrowseSettings Save(BrowseSettings settings)
    {
        var settingsPath = GetSettingsPath();
        Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
        var json = JsonSerializer.Serialize(settings, SerializerOptions);
        File.WriteAllText(settingsPath, json);
        return settings;
    }

    public object GetOptions()
    {
        return new
        {
            themes = new[] { "default" },
            pageSize = new
            {
                campaigns = new[] { 10, 20, 50, 100 },
                content = new[] { 10, 20, 50, 100 },
                media = new[] { 20, 30, 60, 100 }
            }
        };
    }

    private string GetSettingsPath()
    {
        return Path.Combine(_runtime.Config.StorageDirectory, "browse-settings.json");
    }
}
