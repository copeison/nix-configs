using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using PatreonDlServer.Models;
using PatreonDlServer.Utilities;
using System.Text.Json.Nodes;

namespace PatreonDlServer.Services;

public sealed class PatreonRepository
{
    private static readonly Regex InlineImageRegex = new(
        "<img\\b[^>]*data-media-id=[\"'](?<id>[^\"']+)[\"'][^>]*>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private readonly ILogger<PatreonRepository> _logger;
    private readonly ServerRuntime _runtime;

    public PatreonRepository(ServerRuntime runtime, ILogger<PatreonRepository> logger)
    {
        _logger = logger;
        _runtime = runtime;
    }

    public string DataDirectory => _runtime.Config.DataDirectory;

    public string DatabasePath => Path.Combine(DataDirectory, ".patreon-dl", "db.sqlite");

    public void EnsureReady()
    {
        if (!File.Exists(DatabasePath))
        {
            throw new FileNotFoundException($"Patreon DB file was not found at '{DatabasePath}'.");
        }
    }

    public ListResult<JsonObject> GetCampaigns(string sortBy, int limit, int offset)
    {
        var orderByClause = sortBy switch
        {
            "z-a" => "campaign_name DESC",
            "most_content" => "content_count DESC",
            "most_media" => "media_count DESC",
            "last_downloaded" => "last_download DESC",
            _ => "campaign_name ASC"
        };

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            SELECT
              details,
              IFNULL(post_count, 0) AS post_count,
              IFNULL(product_count, 0) AS product_count,
              IFNULL(media_count, 0) AS media_count,
              IFNULL(collection_count, 0) AS collection_count
            FROM campaign
            LEFT JOIN (
              SELECT COUNT(*) AS post_count, campaign_id
              FROM content
              WHERE content_type = 'post'
              GROUP BY campaign_id
            ) postc ON postc.campaign_id = campaign.campaign_id
            LEFT JOIN (
              SELECT COUNT(*) AS product_count, campaign_id
              FROM content
              WHERE content_type = 'product'
              GROUP BY campaign_id
            ) productc ON productc.campaign_id = campaign.campaign_id
            LEFT JOIN (
              SELECT COUNT(*) AS media_count, campaign_id
              FROM content_media
              GROUP BY campaign_id
            ) mc ON mc.campaign_id = campaign.campaign_id
            LEFT JOIN (
              SELECT COUNT(*) AS collection_count, campaign_id
              FROM collection
              GROUP BY campaign_id
            ) cc ON cc.campaign_id = campaign.campaign_id
            ORDER BY {orderByClause}
            LIMIT $limit OFFSET $offset;
            """;
        command.Parameters.AddWithValue("$limit", limit);
        command.Parameters.AddWithValue("$offset", offset);

        var rows = new List<CampaignRow>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                rows.Add(new CampaignRow(
                    reader.GetString(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4)));
            }
        }

        var items = ParseCampaignRows(rows);

        var total = ExecuteScalar<int>(connection, "SELECT COUNT(*) FROM campaign;");
        return new ListResult<JsonObject> { Items = items, Total = total };
    }

    public JsonObject? GetCampaign(string id, bool withCounts)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = withCounts
            ? """
              SELECT
                details,
                IFNULL(post_count, 0) AS post_count,
                IFNULL(product_count, 0) AS product_count,
                IFNULL(media_count, 0) AS media_count,
                IFNULL(collection_count, 0) AS collection_count
              FROM campaign
              LEFT JOIN (
                SELECT COUNT(*) AS post_count, campaign_id
                FROM content
                WHERE content_type = 'post'
                GROUP BY campaign_id
              ) postc ON postc.campaign_id = campaign.campaign_id
              LEFT JOIN (
                SELECT COUNT(*) AS product_count, campaign_id
                FROM content
                WHERE content_type = 'product'
                GROUP BY campaign_id
              ) productc ON productc.campaign_id = campaign.campaign_id
              LEFT JOIN (
                SELECT COUNT(*) AS media_count, campaign_id
                FROM content_media
                GROUP BY campaign_id
              ) mc ON mc.campaign_id = campaign.campaign_id
              LEFT JOIN (
                SELECT COUNT(*) AS collection_count, campaign_id
                FROM collection
                GROUP BY campaign_id
              ) cc ON cc.campaign_id = campaign.campaign_id
              WHERE campaign.campaign_id = $id;
              """
            : """
              SELECT details
              FROM campaign
              WHERE campaign_id = $id;
              """;
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var campaign = JsonNodeExtensions.ParseObject(reader.GetString(0));
        if (withCounts)
        {
            campaign["postCount"] = reader.GetInt32(1);
            campaign["productCount"] = reader.GetInt32(2);
            campaign["mediaCount"] = reader.GetInt32(3);
            campaign["collectionCount"] = reader.GetInt32(4);
        }

        return campaign;
    }

    public ListResult<JsonObject> GetContentList(
        string campaignId,
        string contentType,
        bool? isViewable,
        string? datePublished,
        string? search,
        string sortBy,
        int limit,
        int offset)
    {
        using var connection = OpenConnection();
        var useFts = !string.IsNullOrWhiteSpace(search) && TableExists(connection, $"{contentType}_fts");

        var whereParts = new List<string>
        {
            "content.campaign_id = $campaignId",
            "content.content_type = $contentType"
        };

        if (isViewable.HasValue)
        {
            whereParts.Add("content.is_viewable = $isViewable");
        }

        if (!string.IsNullOrWhiteSpace(datePublished))
        {
            var format = datePublished!.Length == 4 ? "%Y" : "%Y-%m";
            whereParts.Add($"strftime('{format}', datetime(content.published_at / 1000, 'unixepoch')) = $datePublished");
        }

        if (useFts)
        {
            whereParts.Add($"{contentType}_fts MATCH $search");
        }

        var orderByClause = sortBy switch
        {
            "z-a" => "content.title DESC",
            "latest" => "content.published_at DESC",
            "oldest" => "content.published_at ASC",
            _ => "content.title ASC"
        };

        var fromClause = useFts
            ? $"""
               FROM {contentType}_fts
               LEFT JOIN {contentType}_fts_source ON {contentType}_fts_source.fts_rowid = {contentType}_fts.rowid
               LEFT JOIN content ON content.content_id = {contentType}_fts_source.{contentType}_id
               """
            : "FROM content";

        using var command = connection.CreateCommand();
        command.CommandText = $"""
            SELECT DISTINCT
              content.details,
              IFNULL(post_comments.comment_count, 0) AS comment_count,
              post_comments.comments
            {fromClause}
            LEFT JOIN post_comments ON post_comments.post_id = content.content_id AND content.content_type = 'post'
            WHERE {string.Join(" AND ", whereParts)}
            ORDER BY {orderByClause}
            LIMIT $limit OFFSET $offset;
            """;
        command.Parameters.AddWithValue("$campaignId", campaignId);
        command.Parameters.AddWithValue("$contentType", contentType);
        command.Parameters.AddWithValue("$limit", limit);
        command.Parameters.AddWithValue("$offset", offset);
        if (isViewable.HasValue)
        {
            command.Parameters.AddWithValue("$isViewable", isViewable.Value ? 1 : 0);
        }
        if (!string.IsNullOrWhiteSpace(datePublished))
        {
            command.Parameters.AddWithValue("$datePublished", datePublished);
        }
        if (useFts)
        {
            command.Parameters.AddWithValue("$search", search);
        }

        var rows = new List<ContentRow>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                rows.Add(new ContentRow(
                    reader.GetString(0),
                    reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader.GetValue(1)),
                    reader.IsDBNull(2) ? null : reader.GetString(2)));
            }
        }

        var items = ParseContentRows(rows);

        using var totalCommand = connection.CreateCommand();
        totalCommand.CommandText = $"""
            SELECT COUNT(DISTINCT content.content_id)
            {fromClause}
            WHERE {string.Join(" AND ", whereParts)};
            """;
        CloneParameters(command, totalCommand, "$campaignId", "$contentType", "$isViewable", "$datePublished", "$search");
        var total = Convert.ToInt32(totalCommand.ExecuteScalar());

        return new ListResult<JsonObject> { Items = items, Total = total };
    }

    public JsonObject? GetPost(string id)
    {
        return GetSingleContent(id, "post");
    }

    public JsonObject? GetProduct(string id)
    {
        return GetSingleContent(id, "product");
    }

    public JsonObject? GetCollection(string id)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT campaign_id, details,
              (SELECT COUNT(post_id) FROM post_collection WHERE collection_id = $id) AS post_count
            FROM collection
            WHERE collection_id = $id;
            """;
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var collection = JsonNodeExtensions.ParseObject(reader.GetString(1));
        collection["numPosts"] = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

        return new JsonObject
        {
            ["collection"] = collection,
            ["campaignId"] = reader.GetString(0)
        };
    }

    public ListResult<JsonObject> GetCollectionPosts(string collectionId, string sortBy, int limit, int offset)
    {
        using var connection = OpenConnection();

        var orderByClause = sortBy switch
        {
            "z-a" => "content.title DESC",
            "oldest" => "content.published_at ASC",
            _ => "content.published_at DESC"
        };

        using var command = connection.CreateCommand();
        command.CommandText = $"""
            SELECT
              content.details,
              IFNULL(post_comments.comment_count, 0) AS comment_count,
              post_comments.comments
            FROM post_collection
            INNER JOIN content ON content.content_id = post_collection.post_id
            LEFT JOIN post_comments ON post_comments.post_id = content.content_id
            WHERE post_collection.collection_id = $collectionId
              AND content.content_type = 'post'
            ORDER BY {orderByClause}
            LIMIT $limit OFFSET $offset;
            """;
        command.Parameters.AddWithValue("$collectionId", collectionId);
        command.Parameters.AddWithValue("$limit", limit);
        command.Parameters.AddWithValue("$offset", offset);

        var rows = new List<ContentRow>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                rows.Add(new ContentRow(
                    reader.GetString(0),
                    reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader.GetValue(1)),
                    reader.IsDBNull(2) ? null : reader.GetString(2)));
            }
        }

        var items = ParseContentRows(rows);

        using var totalCommand = connection.CreateCommand();
        totalCommand.CommandText = """
            SELECT COUNT(*)
            FROM post_collection
            INNER JOIN content ON content.content_id = post_collection.post_id
            WHERE post_collection.collection_id = $collectionId
              AND content.content_type = 'post';
            """;
        totalCommand.Parameters.AddWithValue("$collectionId", collectionId);
        var total = Convert.ToInt32(totalCommand.ExecuteScalar());

        return new ListResult<JsonObject> { Items = items, Total = total };
    }

    public ListResult<JsonObject> GetCollections(string campaignId, string? search, string sortBy, int limit, int offset)
    {
        using var connection = OpenConnection();
        var useFts = !string.IsNullOrWhiteSpace(search) && TableExists(connection, "collection_fts");

        var whereParts = new List<string> { "collection.campaign_id = $campaignId" };
        if (useFts)
        {
            whereParts.Add("collection_fts MATCH $search");
        }

        var orderByClause = sortBy switch
        {
            "a-z" => "collection.title ASC",
            "z-a" => "collection.title DESC",
            "last_created" => "collection.created_at DESC",
            _ => "collection.edited_at DESC"
        };

        var fromClause = useFts
            ? """
              FROM collection_fts
              LEFT JOIN collection_fts_source ON collection_fts_source.fts_rowid = collection_fts.rowid
              LEFT JOIN collection ON collection.collection_id = collection_fts_source.collection_id
              """
            : "FROM collection";

        using var command = connection.CreateCommand();
        command.CommandText = $"""
            SELECT collection.details, IFNULL(pcc.post_count, 0)
            {fromClause}
            LEFT JOIN (
              SELECT COUNT(post_id) AS post_count, collection_id
              FROM post_collection
              GROUP BY collection_id
            ) pcc ON pcc.collection_id = collection.collection_id
            WHERE {string.Join(" AND ", whereParts)}
            ORDER BY {orderByClause}
            LIMIT $limit OFFSET $offset;
            """;
        command.Parameters.AddWithValue("$campaignId", campaignId);
        command.Parameters.AddWithValue("$limit", limit);
        command.Parameters.AddWithValue("$offset", offset);
        if (useFts)
        {
            command.Parameters.AddWithValue("$search", search);
        }

        var rows = new List<CollectionRow>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                rows.Add(new CollectionRow(reader.GetString(0), reader.GetInt32(1)));
            }
        }

        var items = ParseCollectionRows(rows);

        using var totalCommand = connection.CreateCommand();
        totalCommand.CommandText = $"""
            SELECT COUNT(DISTINCT collection.collection_id)
            {fromClause}
            WHERE {string.Join(" AND ", whereParts)};
            """;
        CloneParameters(command, totalCommand, "$campaignId", "$search");
        var total = Convert.ToInt32(totalCommand.ExecuteScalar());

        return new ListResult<JsonObject> { Items = items, Total = total };
    }

    public ListResult<JsonObject> GetPostTags(string campaignId)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT details
            FROM post_tag
            WHERE campaign_id = $campaignId
            ORDER BY value ASC;
            """;
        command.Parameters.AddWithValue("$campaignId", campaignId);

        var rows = new List<string>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            rows.Add(reader.GetString(0));
        }

        var items = ParseJsonObjects(rows);
        return new ListResult<JsonObject> { Items = items, Total = items.Count };
    }

    public string? ResolveMediaPath(string id, bool thumbnail)
    {
        return GetMediaFileInfo(id, thumbnail)?.Path;
    }

    public MediaFileInfo? GetMediaFileInfo(string id, bool thumbnail)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT
              media_id,
              mime_type,
              thumbnail_mime_type,
              download_path,
              thumbnail_download_path
            FROM media
            WHERE media_id = $id;
            """;
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var contentType = thumbnail && !reader.IsDBNull(2)
            ? reader.GetString(2)
            : reader.IsDBNull(1) ? null : reader.GetString(1);

        var relativePath = thumbnail && !reader.IsDBNull(4)
            ? reader.GetString(4)
            : reader.IsDBNull(3) ? null : reader.GetString(3);

        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return null;
        }

        return new MediaFileInfo(
            reader.GetString(0),
            Path.GetFullPath(Path.Combine(DataDirectory, relativePath)),
            contentType);
    }

    private JsonObject? GetSingleContent(string id, string contentType)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT content.details, IFNULL(post_comments.comment_count, 0), post_comments.comments
            FROM content
            LEFT JOIN post_comments ON post_comments.post_id = content.content_id AND content.content_type = 'post'
            WHERE content.content_id = $id AND content.content_type = $contentType;
            """;
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$contentType", contentType);

        using var reader = command.ExecuteReader();
        return reader.Read()
            ? ParseContent(new ContentRow(
                reader.GetString(0),
                reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader.GetValue(1)),
                reader.IsDBNull(2) ? null : reader.GetString(2)))
            : null;
    }

    private JsonObject ParseContent(ContentRow row)
    {
        var details = JsonNodeExtensions.ParseObject(row.Details);
        var type = details["type"]?.GetValue<string>();
        if (type == "post")
        {
            details["commentCount"] = row.CommentCount;
            details["comments"] = row.CommentsJson is null ? null : JsonNode.Parse(row.CommentsJson);

            var content = details["content"]?.GetValue<string>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                details["content"] = RewritePostHtmlForLocalMedia(content);
            }
        }

        return details;
    }

    private List<JsonObject> ParseCampaignRows(IReadOnlyList<CampaignRow> rows)
    {
        if (rows.Count == 0)
        {
            return [];
        }

        var parsed = new JsonObject[rows.Count];
        Parallel.For(0, rows.Count, index =>
        {
            var row = rows[index];
            var campaign = JsonNodeExtensions.ParseObject(row.Details);
            campaign["postCount"] = row.PostCount;
            campaign["productCount"] = row.ProductCount;
            campaign["mediaCount"] = row.MediaCount;
            campaign["collectionCount"] = row.CollectionCount;
            parsed[index] = campaign;
        });

        return [.. parsed];
    }

    private List<JsonObject> ParseContentRows(IReadOnlyList<ContentRow> rows)
    {
        if (rows.Count == 0)
        {
            return [];
        }

        var parsed = new JsonObject[rows.Count];
        Parallel.For(0, rows.Count, index =>
        {
            parsed[index] = ParseContent(rows[index]);
        });

        return [.. parsed];
    }

    private List<JsonObject> ParseCollectionRows(IReadOnlyList<CollectionRow> rows)
    {
        if (rows.Count == 0)
        {
            return [];
        }

        var parsed = new JsonObject[rows.Count];
        Parallel.For(0, rows.Count, index =>
        {
            var row = rows[index];
            var collection = JsonNodeExtensions.ParseObject(row.Details);
            collection["numPosts"] = row.PostCount;
            parsed[index] = collection;
        });

        return [.. parsed];
    }

    private List<JsonObject> ParseJsonObjects(IReadOnlyList<string> rows)
    {
        if (rows.Count == 0)
        {
            return [];
        }

        var parsed = new JsonObject[rows.Count];
        Parallel.For(0, rows.Count, index =>
        {
            parsed[index] = JsonNodeExtensions.ParseObject(rows[index]);
        });

        return [.. parsed];
    }

    private static void CloneParameters(SqliteCommand source, SqliteCommand target, params string[] names)
    {
        foreach (var name in names)
        {
            if (source.Parameters.Contains(name))
            {
                var parameter = source.Parameters[name];
                target.Parameters.AddWithValue(name, parameter.Value);
            }
        }
    }

    private string RewritePostHtmlForLocalMedia(string html)
    {
        return InlineImageRegex.Replace(html, match =>
        {
            var id = match.Groups["id"].Value;
            return $"<img src=\"/media/{id}\" data-media-id=\"{id}\" loading=\"lazy\" />";
        });
    }

    private static bool TableExists(SqliteConnection connection, string tableName)
    {
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT COUNT(*)
            FROM sqlite_master
            WHERE type = 'table' AND name = $name;
            """;
        command.Parameters.AddWithValue("$name", tableName);
        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    private static T ExecuteScalar<T>(SqliteConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        return (T)Convert.ChangeType(command.ExecuteScalar()!, typeof(T));
    }

    private SqliteConnection OpenConnection()
    {
        EnsureReady();
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = DatabasePath,
            Mode = SqliteOpenMode.ReadOnly
        }.ToString());
        connection.Open();
        return connection;
    }

    private sealed record CampaignRow(string Details, int PostCount, int ProductCount, int MediaCount, int CollectionCount);
    private sealed record ContentRow(string Details, int CommentCount, string? CommentsJson);
    private sealed record CollectionRow(string Details, int PostCount);

    public sealed record MediaFileInfo(string Id, string Path, string? ContentType);
}
