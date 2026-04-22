namespace PatreonDlServer.Models;

public sealed class AdminCreateUserRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }
}
