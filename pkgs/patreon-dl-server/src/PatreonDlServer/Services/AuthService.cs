using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using PatreonDlServer.Models;

namespace PatreonDlServer.Services;

public sealed class AuthService
{
    public const string AuthCookieName = "patreon_dl_server_auth";

    private readonly ServerRuntime _runtime;

    public AuthService(ServerRuntime runtime)
    {
        _runtime = runtime;
        EnsureSchema();
    }

    public string DatabasePath => Path.Combine(_runtime.Config.StorageDirectory, "accounts.sqlite");

    public bool HasUsers()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM users;";
        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    public AuthAccount CreateBootstrapAdmin(AuthBootstrapRequest request)
    {
        if (HasUsers())
        {
            throw new InvalidOperationException("Bootstrap is only available before any accounts exist.");
        }

        return CreateUserInternal(request.UserName, request.Password, true, createdByUserId: null);
    }

    public (AuthAccount User, string SessionToken) Login(string userName, string password)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT user_id, user_name, display_name, password_hash, password_salt, is_admin, created_at
            FROM users
            WHERE lower(user_name) = lower($userName);
            """;
        command.Parameters.AddWithValue("$userName", NormalizeUserName(userName));

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            throw new InvalidOperationException("Invalid username or password.");
        }

        var passwordHash = reader.GetString(3);
        var passwordSalt = reader.GetString(4);
        if (!AuthPasswordHasher.VerifyPassword(password, passwordHash, passwordSalt))
        {
            throw new InvalidOperationException("Invalid username or password.");
        }

        var user = new AuthAccount
        {
            Id = reader.GetInt64(0),
            UserName = reader.GetString(1),
            IsAdmin = reader.GetInt64(5) == 1,
            CreatedAt = reader.GetString(6)
        };

        var sessionToken = CreateSession(connection, user.Id);
        return (user, sessionToken);
    }

    public void Logout(string token)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM sessions WHERE token = $token;";
        command.Parameters.AddWithValue("$token", token);
        command.ExecuteNonQuery();
    }

    public AuthAccount? GetUserBySessionToken(string token)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT users.user_id, users.user_name, users.display_name, users.is_admin, users.created_at
            FROM sessions
            INNER JOIN users ON users.user_id = sessions.user_id
            WHERE sessions.token = $token
              AND sessions.expires_at > $now;
            """;
        command.Parameters.AddWithValue("$token", token);
        command.Parameters.AddWithValue("$now", NowIso());

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new AuthAccount
        {
            Id = reader.GetInt64(0),
            UserName = reader.GetString(1),
            IsAdmin = reader.GetInt64(3) == 1,
            CreatedAt = reader.GetString(4)
        };
    }

    public AuthInvite ValidateInvite(string token)
    {
        var invite = GetInviteByToken(token);
        if (invite is null || invite.IsRevoked || invite.IsUsed)
        {
            throw new InvalidOperationException("Invite link is invalid.");
        }

        if (!string.IsNullOrWhiteSpace(invite.ExpiresAt) &&
            DateTimeOffset.Parse(invite.ExpiresAt) <= DateTimeOffset.UtcNow)
        {
            throw new InvalidOperationException("Invite link has expired.");
        }

        return invite;
    }

    public AuthAccount RegisterWithInvite(AuthRegisterInviteRequest request)
    {
        var invite = ValidateInvite(request.Token);
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        var user = CreateUserInternal(
            request.UserName,
            request.Password,
            false,
            createdByUserId: null,
            connection,
            transaction);

        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            UPDATE invites
            SET used_by_user_id = $userId,
                used_at = $usedAt
            WHERE token = $token;
            """;
        command.Parameters.AddWithValue("$userId", user.Id);
        command.Parameters.AddWithValue("$usedAt", NowIso());
        command.Parameters.AddWithValue("$token", invite.Token);
        command.ExecuteNonQuery();

        transaction.Commit();
        return user;
    }

    public AuthAccount CreateUser(AdminCreateUserRequest request, AuthAccount adminUser)
    {
        return CreateUserInternal(request.UserName, request.Password, request.IsAdmin, adminUser.Id);
    }

    public AuthAccount UpdateUser(long userId, AdminUpdateUserRequest request, AuthAccount actingAdmin)
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        if (!request.IsAdmin && IsLastAdmin(connection, userId))
        {
            throw new InvalidOperationException("You cannot remove admin access from the last admin account.");
        }

        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            UPDATE users
            SET display_name = user_name,
                is_admin = $isAdmin,
                updated_at = $updatedAt
            WHERE user_id = $userId;
            """;
        command.Parameters.AddWithValue("$isAdmin", request.IsAdmin ? 1 : 0);
        command.Parameters.AddWithValue("$updatedAt", NowIso());
        command.Parameters.AddWithValue("$userId", userId);
        if (command.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("User was not found.");
        }

        transaction.Commit();
        return GetUserById(userId) ?? throw new InvalidOperationException("User was not found.");
    }

    public void DeleteUser(long userId, AuthAccount actingAdmin)
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        if (IsLastAdmin(connection, userId))
        {
            throw new InvalidOperationException("You cannot remove the last admin account.");
        }

        using (var deleteSessions = connection.CreateCommand())
        {
            deleteSessions.Transaction = transaction;
            deleteSessions.CommandText = "DELETE FROM sessions WHERE user_id = $userId;";
            deleteSessions.Parameters.AddWithValue("$userId", userId);
            deleteSessions.ExecuteNonQuery();
        }

        using (var deleteUser = connection.CreateCommand())
        {
            deleteUser.Transaction = transaction;
            deleteUser.CommandText = "DELETE FROM users WHERE user_id = $userId;";
            deleteUser.Parameters.AddWithValue("$userId", userId);
            if (deleteUser.ExecuteNonQuery() == 0)
            {
                throw new InvalidOperationException("User was not found.");
            }
        }

        transaction.Commit();
    }

    public AuthInvite CreateInvite(int? expiresInDays, AuthAccount createdBy)
    {
        using var connection = OpenConnection();
        var token = CreateToken();
        var createdAt = NowIso();
        var effectiveDays = expiresInDays.GetValueOrDefault();
        var expiresAt = effectiveDays > 0
            ? DateTimeOffset.UtcNow.AddDays(effectiveDays).ToString("O")
            : null;

        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO invites (token, created_by_user_id, created_at, expires_at)
            VALUES ($token, $createdByUserId, $createdAt, $expiresAt);
            """;
        command.Parameters.AddWithValue("$token", token);
        command.Parameters.AddWithValue("$createdByUserId", createdBy.Id);
        command.Parameters.AddWithValue("$createdAt", createdAt);
        command.Parameters.AddWithValue("$expiresAt", (object?)expiresAt ?? DBNull.Value);
        command.ExecuteNonQuery();

        return GetInviteByToken(token) ?? throw new InvalidOperationException("Invite could not be created.");
    }

    public void RevokeInvite(string token)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE invites
            SET revoked_at = $revokedAt
            WHERE token = $token
              AND revoked_at IS NULL;
            """;
        command.Parameters.AddWithValue("$token", token);
        command.Parameters.AddWithValue("$revokedAt", NowIso());
        if (command.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("Invite was not found.");
        }
    }

    public void DeleteInvite(string token)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            DELETE FROM invites
            WHERE token = $token;
            """;
        command.Parameters.AddWithValue("$token", token);
        if (command.ExecuteNonQuery() == 0)
        {
            throw new InvalidOperationException("Invite was not found.");
        }
    }

    public IReadOnlyList<AuthAccount> GetUsers()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT user_id, user_name, display_name, is_admin, created_at
            FROM users
            ORDER BY lower(user_name) ASC;
            """;

        var items = new List<AuthAccount>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            items.Add(new AuthAccount
            {
                Id = reader.GetInt64(0),
                UserName = reader.GetString(1),
                IsAdmin = reader.GetInt64(3) == 1,
                CreatedAt = reader.GetString(4)
            });
        }

        return items;
    }

    public IReadOnlyList<AuthInvite> GetInvites()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT invites.token,
                   invites.created_at,
                   invites.expires_at,
                   invites.revoked_at,
                   invites.used_at,
                   creator.user_name AS created_by_user_name,
                   used_by.user_name AS used_by_user_name
            FROM invites
            LEFT JOIN users creator ON creator.user_id = invites.created_by_user_id
            LEFT JOIN users used_by ON used_by.user_id = invites.used_by_user_id
            ORDER BY invites.created_at DESC;
            """;

        var items = new List<AuthInvite>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            items.Add(new AuthInvite
            {
                Token = reader.GetString(0),
                CreatedAt = reader.GetString(1),
                ExpiresAt = reader.IsDBNull(2) ? null : reader.GetString(2),
                IsRevoked = !reader.IsDBNull(3),
                IsUsed = !reader.IsDBNull(4),
                UsedAt = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedByUserName = reader.IsDBNull(5) ? null : reader.GetString(5),
                UsedByUserName = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }

        return items;
    }

    private AuthAccount CreateUserInternal(
        string userName,
        string password,
        bool isAdmin,
        long? createdByUserId,
        SqliteConnection? existingConnection = null,
        SqliteTransaction? transaction = null)
    {
        var normalizedUserName = NormalizeUserName(userName);
        if (string.IsNullOrWhiteSpace(normalizedUserName))
        {
            throw new InvalidOperationException("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            throw new InvalidOperationException("Password must be at least 8 characters long.");
        }

        if (password.Any(char.IsWhiteSpace))
        {
            throw new InvalidOperationException("Password cannot contain spaces or other whitespace.");
        }

        var (passwordHash, passwordSalt) = AuthPasswordHasher.HashPassword(password);
        var createdAt = NowIso();

        var connection = existingConnection ?? OpenConnection();
        try
        {
            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = """
                INSERT INTO users (user_name, display_name, password_hash, password_salt, is_admin, created_at, updated_at)
                VALUES ($userName, $displayName, $passwordHash, $passwordSalt, $isAdmin, $createdAt, $createdAt)
                RETURNING user_id;
                """;
            command.Parameters.AddWithValue("$userName", normalizedUserName);
            command.Parameters.AddWithValue("$displayName", normalizedUserName);
            command.Parameters.AddWithValue("$passwordHash", passwordHash);
            command.Parameters.AddWithValue("$passwordSalt", passwordSalt);
            command.Parameters.AddWithValue("$isAdmin", isAdmin ? 1 : 0);
            command.Parameters.AddWithValue("$createdAt", createdAt);

            long userId;
            try
            {
                userId = Convert.ToInt64(command.ExecuteScalar());
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                throw new InvalidOperationException("That username is already in use.");
            }

            return new AuthAccount
            {
                Id = userId,
                UserName = normalizedUserName,
                IsAdmin = isAdmin,
                CreatedAt = createdAt
            };
        }
        finally
        {
            if (existingConnection is null)
            {
                connection.Dispose();
            }
        }
    }

    private string CreateSession(SqliteConnection connection, long userId)
    {
        var token = CreateToken();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO sessions (token, user_id, created_at, expires_at)
            VALUES ($token, $userId, $createdAt, $expiresAt);
            """;
        command.Parameters.AddWithValue("$token", token);
        command.Parameters.AddWithValue("$userId", userId);
        command.Parameters.AddWithValue("$createdAt", NowIso());
        command.Parameters.AddWithValue("$expiresAt", DateTimeOffset.UtcNow.AddDays(30).ToString("O"));
        command.ExecuteNonQuery();
        return token;
    }

    private AuthAccount? GetUserById(long userId)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT user_id, user_name, display_name, is_admin, created_at
            FROM users
            WHERE user_id = $userId;
            """;
        command.Parameters.AddWithValue("$userId", userId);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new AuthAccount
        {
            Id = reader.GetInt64(0),
            UserName = reader.GetString(1),
            IsAdmin = reader.GetInt64(3) == 1,
            CreatedAt = reader.GetString(4)
        };
    }

    private AuthInvite? GetInviteByToken(string token)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT invites.token,
                   invites.created_at,
                   invites.expires_at,
                   invites.revoked_at,
                   invites.used_at,
                   creator.user_name AS created_by_user_name,
                   used_by.user_name AS used_by_user_name
            FROM invites
            LEFT JOIN users creator ON creator.user_id = invites.created_by_user_id
            LEFT JOIN users used_by ON used_by.user_id = invites.used_by_user_id
            WHERE invites.token = $token;
            """;
        command.Parameters.AddWithValue("$token", token);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new AuthInvite
        {
            Token = reader.GetString(0),
            CreatedAt = reader.GetString(1),
            ExpiresAt = reader.IsDBNull(2) ? null : reader.GetString(2),
            IsRevoked = !reader.IsDBNull(3),
            IsUsed = !reader.IsDBNull(4),
            UsedAt = reader.IsDBNull(4) ? null : reader.GetString(4),
            CreatedByUserName = reader.IsDBNull(5) ? null : reader.GetString(5),
            UsedByUserName = reader.IsDBNull(6) ? null : reader.GetString(6)
        };
    }

    private bool IsLastAdmin(SqliteConnection connection, long userId)
    {
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT COUNT(*)
            FROM users
            WHERE is_admin = 1;
            """;
        var adminCount = Convert.ToInt32(command.ExecuteScalar());
        if (adminCount > 1)
        {
            return false;
        }

        using var userCommand = connection.CreateCommand();
        userCommand.CommandText = "SELECT is_admin FROM users WHERE user_id = $userId;";
        userCommand.Parameters.AddWithValue("$userId", userId);
        var scalar = userCommand.ExecuteScalar();
        return scalar is not null && Convert.ToInt32(scalar) == 1;
    }

    private void EnsureSchema()
    {
        Directory.CreateDirectory(_runtime.Config.StorageDirectory);
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS users (
              user_id INTEGER PRIMARY KEY AUTOINCREMENT,
              user_name TEXT NOT NULL UNIQUE,
              display_name TEXT NOT NULL,
              password_hash TEXT NOT NULL,
              password_salt TEXT NOT NULL,
              is_admin INTEGER NOT NULL DEFAULT 0,
              created_at TEXT NOT NULL,
              updated_at TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS sessions (
              token TEXT PRIMARY KEY,
              user_id INTEGER NOT NULL,
              created_at TEXT NOT NULL,
              expires_at TEXT NOT NULL,
              FOREIGN KEY(user_id) REFERENCES users(user_id) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS invites (
              token TEXT PRIMARY KEY,
              created_by_user_id INTEGER,
              created_at TEXT NOT NULL,
              expires_at TEXT,
              revoked_at TEXT,
              used_by_user_id INTEGER,
              used_at TEXT,
              FOREIGN KEY(created_by_user_id) REFERENCES users(user_id) ON DELETE SET NULL,
              FOREIGN KEY(used_by_user_id) REFERENCES users(user_id) ON DELETE SET NULL
            );

            CREATE INDEX IF NOT EXISTS idx_sessions_user_id ON sessions (user_id);
            CREATE INDEX IF NOT EXISTS idx_sessions_expires_at ON sessions (expires_at);
            CREATE INDEX IF NOT EXISTS idx_invites_created_at ON invites (created_at);
            """;
        command.ExecuteNonQuery();

        using var cleanup = connection.CreateCommand();
        cleanup.CommandText = "DELETE FROM sessions WHERE expires_at <= $now;";
        cleanup.Parameters.AddWithValue("$now", NowIso());
        cleanup.ExecuteNonQuery();
    }

    private SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = DatabasePath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            ForeignKeys = true
        }.ToString());
        connection.Open();
        return connection;
    }

    private static string NormalizeUserName(string value)
    {
        return value.Trim();
    }

    private static string NowIso()
    {
        return DateTimeOffset.UtcNow.ToString("O");
    }

    private static string CreateToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(24)).ToLowerInvariant();
    }
}
