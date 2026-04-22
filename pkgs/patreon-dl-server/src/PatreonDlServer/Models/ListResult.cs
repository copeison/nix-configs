namespace PatreonDlServer.Models;

public sealed class ListResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }

    public required int Total { get; init; }
}
