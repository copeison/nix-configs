using PatreonDlServer.Models;
using PatreonDlServer.Services;

const string AuthUserItemKey = "auth.user";

var serverPaths = ServerPaths.Create();
var configStore = new TomlConfigStore(serverPaths);
var serverConfig = configStore.Load();
var startupPort = Math.Clamp(serverConfig.Port, 1024, 65535);

var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = args });

builder.Configuration.AddCommandLine(args);
builder.WebHost.UseUrls($"http://0.0.0.0:{startupPort}");

builder.Services.AddSingleton(serverPaths);
builder.Services.AddSingleton(configStore);
builder.Services.AddSingleton(new ServerRuntime(serverPaths, configStore, serverConfig));
builder.Services.AddSingleton<PatreonRepository>();
builder.Services.AddSingleton<BrowseSettingsStore>();
builder.Services.AddSingleton<AuthService>();

var app = builder.Build();
var currentPort = startupPort;

app.Use(async (context, next) =>
{
    var authService = context.RequestServices.GetRequiredService<AuthService>();
    var runtime = context.RequestServices.GetRequiredService<ServerRuntime>();
    if (context.Request.Cookies.TryGetValue(AuthService.AuthCookieName, out var sessionToken) &&
        !string.IsNullOrWhiteSpace(sessionToken))
    {
        var user = authService.GetUserBySessionToken(sessionToken);
        if (user is not null)
        {
            context.Items[AuthUserItemKey] = user;
        }
    }

    if (!RequiresAuthentication(context.Request.Path))
    {
        await next();
        return;
    }

    if (IsBootstrapSetupRequest(context.Request.Path, runtime, authService))
    {
        await next();
        return;
    }

    var currentUser = context.Items[AuthUserItemKey] as AuthAccount;
    if (currentUser is null)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsJsonAsync(new { error = "Authentication is required." });
        return;
    }

    if (RequiresAdmin(context.Request.Path))
    {
        if (!currentUser.IsAdmin)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { error = "Administrator access is required." });
            return;
        }
    }

    await next();
});

app.MapGet("/api/server/runtime", (HttpContext context, ServerRuntime runtime, AuthService authService) =>
{
    var currentUser = GetCurrentUser(context);
    return Results.Ok(new
    {
        setupRequired = !runtime.IsConfigured,
        restartRequired = runtime.Config.Port != currentPort,
        platform = OperatingSystem.IsLinux() ? "linux" : OperatingSystem.IsWindows() ? "windows" : OperatingSystem.IsMacOS() ? "darwin" : "unknown",
        currentPort,
        configPath = runtime.Paths.ConfigPath,
        config = new
        {
            configured = runtime.Config.Configured,
            port = runtime.Config.Port,
            dataDirectory = runtime.Config.DataDirectory,
            storageDirectory = runtime.Config.StorageDirectory,
            publicBaseUrl = runtime.Config.PublicBaseUrl,
            pageTitle = runtime.Config.PageTitle
        },
        auth = new
        {
            requiresBootstrap = runtime.IsConfigured && !authService.HasUsers(),
            authenticated = currentUser is not null,
            currentUser = currentUser is null ? null : new
            {
                id = currentUser.Id,
                userName = currentUser.UserName,
                isAdmin = currentUser.IsAdmin
            }
        }
    });
});

app.MapPost("/api/server/setup", async (HttpContext context, HttpRequest request, ServerRuntime runtime, AuthService authService) =>
{
    var currentUser = GetCurrentUser(context);
    if (runtime.IsConfigured && authService.HasUsers() && (currentUser is null || !currentUser.IsAdmin))
    {
        return Results.Forbid();
    }

    var setup = await request.ReadFromJsonAsync<ServerSetupRequest>() ?? new ServerSetupRequest();
    var result = runtime.SaveSetup(setup, currentPort);
    return Results.Ok(result);
});

app.MapGet("/api/auth/status", (HttpContext context, ServerRuntime runtime, AuthService authService) =>
{
    var currentUser = GetCurrentUser(context);
    return Results.Ok(new
    {
        requiresSetup = !runtime.IsConfigured,
        requiresBootstrap = runtime.IsConfigured && !authService.HasUsers(),
        authenticated = currentUser is not null,
        currentUser = currentUser is null ? null : new
        {
            id = currentUser.Id,
            userName = currentUser.UserName,
            isAdmin = currentUser.IsAdmin
        }
    });
});

app.MapPost("/api/auth/bootstrap", async (HttpContext context, HttpRequest request, AuthService authService, ServerRuntime runtime) =>
{
    if (!runtime.IsConfigured)
    {
        return Results.BadRequest(new { error = "Server setup must be completed before creating the first admin account." });
    }

    var payload = await request.ReadFromJsonAsync<AuthBootstrapRequest>() ?? new AuthBootstrapRequest();
    var user = authService.CreateBootstrapAdmin(payload);
    var login = authService.Login(payload.UserName, payload.Password);
    SetAuthCookie(context.Response, login.SessionToken, context.Request.IsHttps);
    return Results.Ok(new { user });
});

app.MapPost("/api/auth/login", async (HttpContext context, HttpRequest request, AuthService authService) =>
{
    var payload = await request.ReadFromJsonAsync<AuthLoginRequest>() ?? new AuthLoginRequest();
    var login = authService.Login(payload.UserName, payload.Password);
    SetAuthCookie(context.Response, login.SessionToken, context.Request.IsHttps);
    return Results.Ok(new { user = login.User });
});

app.MapPost("/api/auth/logout", (HttpContext context, AuthService authService) =>
{
    if (context.Request.Cookies.TryGetValue(AuthService.AuthCookieName, out var sessionToken) &&
        !string.IsNullOrWhiteSpace(sessionToken))
    {
        authService.Logout(sessionToken);
    }

    context.Response.Cookies.Delete(AuthService.AuthCookieName, new CookieOptions
    {
        Path = "/",
        SameSite = SameSiteMode.Lax,
        HttpOnly = true
    });

    return Results.Ok(new { loggedOut = true });
});

app.MapGet("/api/auth/invites/{token}", (string token, AuthService authService) =>
{
    try
    {
        var invite = authService.ValidateInvite(token);
        return Results.Ok(new
        {
            valid = true,
            token = invite.Token,
            expiresAt = invite.ExpiresAt
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { valid = false, error = ex.Message });
    }
});

app.MapPost("/api/auth/register", async (HttpContext context, HttpRequest request, AuthService authService) =>
{
    var payload = await request.ReadFromJsonAsync<AuthRegisterInviteRequest>() ?? new AuthRegisterInviteRequest();
    var user = authService.RegisterWithInvite(payload);
    var login = authService.Login(payload.UserName, payload.Password);
    SetAuthCookie(context.Response, login.SessionToken, context.Request.IsHttps);
    return Results.Ok(new { user });
});

app.MapGet("/api/admin/users", (AuthService authService) =>
{
    return Results.Ok(new { users = authService.GetUsers() });
});

app.MapPost("/api/admin/users", async (HttpContext context, HttpRequest request, AuthService authService) =>
{
    var payload = await request.ReadFromJsonAsync<AdminCreateUserRequest>() ?? new AdminCreateUserRequest();
    var currentUser = GetRequiredUser(context);
    var user = authService.CreateUser(payload, currentUser);
    return Results.Ok(new { user });
});

app.MapPut("/api/admin/users/{id:long}", async (long id, HttpContext context, HttpRequest request, AuthService authService) =>
{
    var payload = await request.ReadFromJsonAsync<AdminUpdateUserRequest>() ?? new AdminUpdateUserRequest();
    var currentUser = GetRequiredUser(context);
    var user = authService.UpdateUser(id, payload, currentUser);
    return Results.Ok(new { user });
});

app.MapDelete("/api/admin/users/{id:long}", (long id, HttpContext context, AuthService authService) =>
{
    var currentUser = GetRequiredUser(context);
    authService.DeleteUser(id, currentUser);
    return Results.Ok(new { deleted = true });
});

app.MapGet("/api/admin/invites", (AuthService authService, ServerRuntime runtime) =>
{
    var baseUrl = runtime.CurrentBaseUrl(currentPort);
    return Results.Ok(new
    {
        invites = authService.GetInvites().Select(invite => new
        {
            token = invite.Token,
            createdAt = invite.CreatedAt,
            expiresAt = invite.ExpiresAt,
            isRevoked = invite.IsRevoked,
            isUsed = invite.IsUsed,
            usedAt = invite.UsedAt,
            createdByUserName = invite.CreatedByUserName,
            usedByUserName = invite.UsedByUserName,
            inviteUrl = $"{baseUrl}/?invite={invite.Token}"
        })
    });
});

app.MapPost("/api/admin/invites", async (HttpRequest request, HttpContext context, AuthService authService, ServerRuntime runtime) =>
{
    var payload = await request.ReadFromJsonAsync<AdminCreateInviteRequest>() ?? new AdminCreateInviteRequest();
    var currentUser = GetRequiredUser(context);
    var invite = authService.CreateInvite(payload.ExpiresInDays, currentUser);
    return Results.Ok(new
    {
        invite = new
        {
            token = invite.Token,
            createdAt = invite.CreatedAt,
            expiresAt = invite.ExpiresAt,
            isRevoked = invite.IsRevoked,
            isUsed = invite.IsUsed,
            inviteUrl = $"{runtime.CurrentBaseUrl(currentPort)}/?invite={invite.Token}"
        }
    });
});

app.MapDelete("/api/admin/invites/{token}", (string token, AuthService authService) =>
{
    authService.RevokeInvite(token);
    return Results.Ok(new { revoked = true });
});

app.MapGet("/api/campaigns", (HttpRequest request, PatreonRepository repository) =>
{
    try
    {
        var sortBy = request.Query["sort_by"].FirstOrDefault() ?? "a-z";
        var limit = ParsePageSize(request.Query["n"].FirstOrDefault(), 20);
        var offset = GetOffset(request.Query["p"].FirstOrDefault(), limit);
        var result = repository.GetCampaigns(sortBy, limit, offset);
        return Results.Ok(new { campaigns = result.Items, total = result.Total });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Failed to load campaigns",
            detail: $"{ex.Message} Database path: {repository.DatabasePath}",
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

app.MapGet("/api/campaigns/{id}", (string id, HttpRequest request, PatreonRepository repository) =>
{
    var withCounts = bool.TryParse(request.Query["with_counts"].FirstOrDefault(), out var value) && value;
    var campaign = repository.GetCampaign(id, withCounts);
    return campaign is null ? Results.NotFound() : Results.Ok(campaign);
});

app.MapGet("/api/campaigns/{id}/posts", (string id, HttpRequest request, PatreonRepository repository) =>
{
    var limit = ParsePageSize(request.Query["n"].FirstOrDefault(), 20);
    var offset = GetOffset(request.Query["p"].FirstOrDefault(), limit);
    var isViewable = ParseNullableBool(request.Query["is_viewable"].FirstOrDefault());
    var result = repository.GetContentList(
        id,
        "post",
        isViewable,
        request.Query["date_published"].FirstOrDefault(),
        request.Query["search"].FirstOrDefault(),
        request.Query["sort_by"].FirstOrDefault() ?? "latest",
        limit,
        offset);
    return Results.Ok(new { items = result.Items, total = result.Total });
});

app.MapGet("/api/campaigns/{id}/products", (string id, HttpRequest request, PatreonRepository repository) =>
{
    var limit = ParsePageSize(request.Query["n"].FirstOrDefault(), 20);
    var offset = GetOffset(request.Query["p"].FirstOrDefault(), limit);
    var isViewable = ParseNullableBool(request.Query["is_viewable"].FirstOrDefault());
    var result = repository.GetContentList(
        id,
        "product",
        isViewable,
        request.Query["date_published"].FirstOrDefault(),
        request.Query["search"].FirstOrDefault(),
        request.Query["sort_by"].FirstOrDefault() ?? "latest",
        limit,
        offset);
    return Results.Ok(new { items = result.Items, total = result.Total });
});

app.MapGet("/api/posts/{id}", (string id, PatreonRepository repository) =>
{
    var post = repository.GetPost(id);
    return post is null
        ? Results.NotFound()
        : Results.Ok(new { post, previous = (object?)null, next = (object?)null });
});

app.MapGet("/api/products/{id}", (string id, PatreonRepository repository) =>
{
    var product = repository.GetProduct(id);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapGet("/api/collections/{id}", (string id, PatreonRepository repository) =>
{
    var collection = repository.GetCollection(id);
    return collection is null ? Results.NotFound() : Results.Ok(collection);
});

app.MapGet("/api/collections/{id}/posts", (string id, HttpRequest request, PatreonRepository repository) =>
{
    var limit = ParsePageSize(request.Query["n"].FirstOrDefault(), 20);
    var offset = GetOffset(request.Query["p"].FirstOrDefault(), limit);
    var result = repository.GetCollectionPosts(
        id,
        request.Query["sort_by"].FirstOrDefault() ?? "latest",
        limit,
        offset);
    return Results.Ok(new { items = result.Items, total = result.Total });
});

app.MapGet("/api/campaigns/{id}/collections", (string id, HttpRequest request, PatreonRepository repository) =>
{
    var limit = ParsePageSize(request.Query["n"].FirstOrDefault(), 20);
    var offset = GetOffset(request.Query["p"].FirstOrDefault(), limit);
    var result = repository.GetCollections(
        id,
        request.Query["search"].FirstOrDefault(),
        request.Query["sort_by"].FirstOrDefault() ?? "last_updated",
        limit,
        offset);
    return Results.Ok(new { collections = result.Items, total = result.Total });
});

app.MapGet("/api/campaigns/{id}/post_tags", (string id, PatreonRepository repository) =>
{
    var result = repository.GetPostTags(id);
    return Results.Ok(new { items = result.Items, total = result.Total });
});

app.MapGet("/api/settings/browse", (BrowseSettingsStore store) =>
{
    return Results.Ok(store.Get());
});

app.MapGet("/api/settings/browse/options", (BrowseSettingsStore store) =>
{
    return Results.Ok(store.GetOptions());
});

app.MapPost("/api/settings/browse", async (HttpRequest request, BrowseSettingsStore store) =>
{
    var settings = await request.ReadFromJsonAsync<BrowseSettings>() ?? new BrowseSettings();
    return Results.Ok(store.Save(settings));
});

app.MapGet("/media/{id}", (string id, HttpRequest request, PatreonRepository repository) =>
{
    var thumbnail = !string.IsNullOrWhiteSpace(request.Query["t"].FirstOrDefault());
    var download = !string.IsNullOrWhiteSpace(request.Query["dl"].FirstOrDefault());
    var requestedName = request.Query["name"].FirstOrDefault();
    var fileInfo = repository.GetMediaFileInfo(id, thumbnail);
    if (fileInfo is null || string.IsNullOrWhiteSpace(fileInfo.Path) || !File.Exists(fileInfo.Path))
    {
        return Results.NotFound();
    }

    var downloadName = NormalizeDownloadName(requestedName, fileInfo.Path);

    return download
        ? Results.File(fileInfo.Path, fileInfo.ContentType, fileDownloadName: downloadName, enableRangeProcessing: true)
        : Results.File(fileInfo.Path, fileInfo.ContentType, enableRangeProcessing: true);
});

app.MapGet("/", () => Results.Content(InternalSite.IndexHtml, "text/html; charset=utf-8"));
app.MapFallback(() => Results.Redirect("/"));

app.Run();

static int ParsePageSize(string? value, int fallback)
{
    if (!int.TryParse(value, out var parsed))
    {
        return fallback;
    }

    return Math.Clamp(parsed, 1, 200);
}

static int GetOffset(string? pageValue, int limit)
{
    if (!int.TryParse(pageValue, out var page) || page < 2)
    {
        return 0;
    }

    return (page - 1) * limit;
}

static bool? ParseNullableBool(string? value)
{
    return bool.TryParse(value, out var parsed) ? parsed : null;
}

static AuthAccount? GetCurrentUser(HttpContext context)
{
    return context.Items[AuthUserItemKey] as AuthAccount;
}

static AuthAccount GetRequiredUser(HttpContext context)
{
    return GetCurrentUser(context) ?? throw new InvalidOperationException("Authentication is required.");
}

static void SetAuthCookie(HttpResponse response, string token, bool secure)
{
    response.Cookies.Append(AuthService.AuthCookieName, token, new CookieOptions
    {
        HttpOnly = true,
        SameSite = SameSiteMode.Lax,
        Secure = secure,
        Path = "/",
        Expires = DateTimeOffset.UtcNow.AddDays(30)
    });
}

static string NormalizeDownloadName(string? requestedName, string filePath)
{
    var fallback = Path.GetFileName(filePath);
    if (string.IsNullOrWhiteSpace(requestedName))
    {
        return fallback;
    }

    var cleaned = Path.GetFileName(requestedName.Trim());
    if (string.IsNullOrWhiteSpace(cleaned))
    {
        return fallback;
    }

    if (string.IsNullOrWhiteSpace(Path.GetExtension(cleaned)))
    {
        var extension = Path.GetExtension(filePath);
        if (!string.IsNullOrWhiteSpace(extension))
        {
            cleaned += extension;
        }
    }

    return cleaned;
}

static bool RequiresAuthentication(PathString path)
{
    var value = path.Value ?? string.Empty;
    if (value == "/")
    {
        return false;
    }

    if (value.StartsWith("/api/server/runtime", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("/api/auth/status", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("/api/auth/bootstrap", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("/api/auth/login", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("/api/auth/logout", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("/api/auth/register", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("/api/auth/invites/", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    return value.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ||
           value.StartsWith("/media/", StringComparison.OrdinalIgnoreCase);
}

static bool RequiresAdmin(PathString path)
{
    var value = path.Value ?? string.Empty;
    return value.StartsWith("/api/settings/", StringComparison.OrdinalIgnoreCase) ||
           value.StartsWith("/api/admin/", StringComparison.OrdinalIgnoreCase);
}

static bool IsBootstrapSetupRequest(PathString path, ServerRuntime runtime, AuthService authService)
{
    var value = path.Value ?? string.Empty;
    return value.StartsWith("/api/server/setup", StringComparison.OrdinalIgnoreCase) &&
           (!runtime.IsConfigured || !authService.HasUsers());
}
