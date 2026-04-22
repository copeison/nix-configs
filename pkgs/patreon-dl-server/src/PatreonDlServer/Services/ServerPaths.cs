namespace PatreonDlServer.Services;

public sealed class ServerPaths
{
    private ServerPaths(
        string executableDirectory,
        string configDirectory,
        string configPath,
        string defaultStorageDirectory)
    {
        ExecutableDirectory = executableDirectory;
        ConfigDirectory = configDirectory;
        ConfigPath = configPath;
        DefaultStorageDirectory = defaultStorageDirectory;
    }

    public string ExecutableDirectory { get; }

    public string ConfigDirectory { get; }

    public string ConfigPath { get; }

    public string DefaultStorageDirectory { get; }

    public static ServerPaths Create()
    {
        var executableDirectory = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);

        string configDirectory;
        if (OperatingSystem.IsLinux())
        {
            configDirectory = "/var/lib/patreon-dl-server";
        }
        else if (OperatingSystem.IsWindows())
        {
            configDirectory = executableDirectory;
        }
        else if (OperatingSystem.IsMacOS())
        {
            configDirectory = executableDirectory;
        }
        else
        {
            configDirectory = executableDirectory;
        }

        return new ServerPaths(
            executableDirectory,
            configDirectory,
            Path.Combine(configDirectory, "config.toml"),
            configDirectory);
    }
}
