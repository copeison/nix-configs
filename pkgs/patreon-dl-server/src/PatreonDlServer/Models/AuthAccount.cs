namespace PatreonDlServer.Models;

public sealed class AuthAccount
{
    public long Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }

    public string CreatedAt { get; set; } = string.Empty;
}
