using System.Text;
using PatreonDlServer.Models;

namespace PatreonDlServer.Services;

public sealed class TomlConfigStore
{
    private readonly ServerPaths _paths;

    public TomlConfigStore(ServerPaths paths)
    {
        _paths = paths;
    }

    public ServerConfig Load()
    {
        var config = new ServerConfig
        {
            Configured = false,
            Port = 5000,
            DataDirectory = string.Empty,
            StorageDirectory = _paths.DefaultStorageDirectory,
            PublicBaseUrl = string.Empty,
            PageTitle = "Patreon Browse Server"
        };

        if (!File.Exists(_paths.ConfigPath))
        {
            return config;
        }

        foreach (var rawLine in File.ReadAllLines(_paths.ConfigPath))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            {
                continue;
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex < 0)
            {
                continue;
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim();

            switch (key)
            {
                case "configured":
                    if (bool.TryParse(value, out var configured))
                    {
                        config.Configured = configured;
                    }
                    break;
                case "port":
                    if (int.TryParse(value, out var port))
                    {
                        config.Port = port;
                    }
                    break;
                case "data_directory":
                    config.DataDirectory = ParseTomlString(value);
                    break;
                case "storage_directory":
                    config.StorageDirectory = ParseTomlString(value);
                    break;
                case "public_base_url":
                    config.PublicBaseUrl = ParseTomlString(value);
                    break;
                case "page_title":
                    config.PageTitle = ParseTomlString(value);
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(config.StorageDirectory))
        {
            config.StorageDirectory = _paths.DefaultStorageDirectory;
        }

        return config;
    }

    public void Save(ServerConfig config)
    {
        Directory.CreateDirectory(_paths.ConfigDirectory);

        var builder = new StringBuilder();
        builder.AppendLine($"configured = {config.Configured.ToString().ToLowerInvariant()}");
        builder.AppendLine($"port = {config.Port}");
        builder.AppendLine($"data_directory = {FormatTomlString(config.DataDirectory)}");
        builder.AppendLine($"storage_directory = {FormatTomlString(config.StorageDirectory)}");
        builder.AppendLine($"public_base_url = {FormatTomlString(config.PublicBaseUrl)}");
        builder.AppendLine($"page_title = {FormatTomlString(config.PageTitle)}");

        File.WriteAllText(_paths.ConfigPath, builder.ToString());
    }

    private static string ParseTomlString(string value)
    {
        if (value.Length >= 2 && value.StartsWith('"') && value.EndsWith('"'))
        {
            value = value[1..^1];
        }

        return value
            .Replace("\\\\", "\\", StringComparison.Ordinal)
            .Replace("\\\"", "\"", StringComparison.Ordinal);
    }

    private static string FormatTomlString(string value)
    {
        var escaped = value
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("\"", "\\\"", StringComparison.Ordinal);

        return $"\"{escaped}\"";
    }
}
