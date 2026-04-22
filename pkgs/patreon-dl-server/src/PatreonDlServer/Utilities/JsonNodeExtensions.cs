using System.Text.Json;
using System.Text.Json.Nodes;

namespace PatreonDlServer.Utilities;

public static class JsonNodeExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static JsonObject ParseObject(string json)
    {
        var node = JsonNode.Parse(json);
        return node as JsonObject ?? new JsonObject();
    }

    public static T ParseOrDefault<T>(string? json, T fallback)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return fallback;
        }

        return JsonSerializer.Deserialize<T>(json, SerializerOptions) ?? fallback;
    }
}
