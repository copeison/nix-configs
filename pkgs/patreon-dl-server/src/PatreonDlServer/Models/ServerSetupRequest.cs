namespace PatreonDlServer.Models;

public sealed class ServerSetupRequest
{
    public int Port { get; set; } = 5000;

    public string DataDirectory { get; set; } = string.Empty;

    public string StorageDirectory { get; set; } = string.Empty;

    public string PublicBaseUrl { get; set; } = string.Empty;

    public string PageTitle { get; set; } = "Patreon Browse Server";
}
