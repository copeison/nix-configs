using PatreonDlServer.Models;

namespace PatreonDlServer.Services;

public sealed class ServerRuntime
{
    private readonly ServerPaths _paths;
    private readonly TomlConfigStore _configStore;

    public ServerRuntime(ServerPaths paths, TomlConfigStore configStore, ServerConfig config)
    {
        _paths = paths;
        _configStore = configStore;
        Config = Normalize(config);
    }

    public ServerConfig Config { get; private set; }

    public ServerPaths Paths => _paths;

    public bool IsConfigured =>
        Config.Configured &&
        !string.IsNullOrWhiteSpace(Config.DataDirectory) &&
        !string.IsNullOrWhiteSpace(Config.StorageDirectory);

    public string CurrentBaseUrl(int currentPort)
    {
        if (!string.IsNullOrWhiteSpace(Config.PublicBaseUrl))
        {
            return Config.PublicBaseUrl;
        }

        return $"http://127.0.0.1:{currentPort}";
    }

    public object GetRuntimeInfo(int currentPort)
    {
        return new
        {
            setupRequired = !IsConfigured,
            restartRequired = Config.Port != currentPort,
            platform = GetPlatformName(),
            currentPort,
            configPath = _paths.ConfigPath,
            config = new
            {
                configured = Config.Configured,
                port = Config.Port,
                dataDirectory = Config.DataDirectory,
                storageDirectory = Config.StorageDirectory,
                publicBaseUrl = Config.PublicBaseUrl,
                pageTitle = Config.PageTitle
            }
        };
    }

    public object SaveSetup(ServerSetupRequest request, int currentPort)
    {
        var normalized = Normalize(new ServerConfig
        {
            Configured = true,
            Port = Math.Clamp(request.Port, 1024, 65535),
            DataDirectory = request.DataDirectory,
            StorageDirectory = request.StorageDirectory,
            PublicBaseUrl = request.PublicBaseUrl,
            PageTitle = request.PageTitle
        });

        if (string.IsNullOrWhiteSpace(normalized.DataDirectory))
        {
            throw new InvalidOperationException("Data directory is required.");
        }

        Directory.CreateDirectory(normalized.StorageDirectory);

        Config = normalized;
        _configStore.Save(normalized);

        return new
        {
            saved = true,
            restartRequired = normalized.Port != currentPort,
            config = normalized
        };
    }

    private ServerConfig Normalize(ServerConfig config)
    {
        var storageDirectory = string.IsNullOrWhiteSpace(config.StorageDirectory)
            ? _paths.DefaultStorageDirectory
            : Path.GetFullPath(config.StorageDirectory);

        var dataDirectory = string.IsNullOrWhiteSpace(config.DataDirectory)
            ? string.Empty
            : Path.GetFullPath(config.DataDirectory);

        return new ServerConfig
        {
            Configured = config.Configured,
            Port = Math.Clamp(config.Port, 1024, 65535),
            DataDirectory = dataDirectory,
            StorageDirectory = storageDirectory,
            PublicBaseUrl = NormalizePublicBaseUrl(config.PublicBaseUrl),
            PageTitle = NormalizePageTitle(config.PageTitle)
        };
    }

    private static string NormalizePublicBaseUrl(string value)
    {
        var normalized = value.Trim().TrimEnd('/');
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return string.Empty;
        }

        if (!Uri.TryCreate(normalized, UriKind.Absolute, out var uri) ||
            string.IsNullOrWhiteSpace(uri.Scheme) ||
            string.IsNullOrWhiteSpace(uri.Host))
        {
            throw new InvalidOperationException("Public base URL must be a valid absolute URL, for example https://example.com.");
        }

        return normalized;
    }

    private static string NormalizePageTitle(string value)
    {
        var normalized = value.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? "Patreon Browse Server" : normalized;
    }

    private static string GetPlatformName()
    {
        if (OperatingSystem.IsLinux())
        {
            return "linux";
        }

        if (OperatingSystem.IsWindows())
        {
            return "windows";
        }

        if (OperatingSystem.IsMacOS())
        {
            return "darwin";
        }

        return "unknown";
    }
}
