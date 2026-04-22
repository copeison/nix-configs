namespace PatreonDlServer.Models;

public sealed class AuthBootstrapRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
