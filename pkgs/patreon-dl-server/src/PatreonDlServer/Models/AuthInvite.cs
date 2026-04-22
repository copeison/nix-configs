namespace PatreonDlServer.Models;

public sealed class AuthInvite
{
    public string Token { get; set; } = string.Empty;

    public string CreatedAt { get; set; } = string.Empty;

    public string? ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public bool IsUsed { get; set; }

    public string? UsedAt { get; set; }

    public string? CreatedByUserName { get; set; }

    public string? UsedByUserName { get; set; }
}
