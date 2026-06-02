using Coach.Domain.Common;
using Coach.Domain.Integrations;

namespace Coach.Domain.Integrations;

/// <summary>
/// Domain entity representing an integration connection.
/// Tracks OAuth tokens, connection status, and sync metadata.
/// </summary>
public sealed class IntegrationConnection : Entity
{
    private IntegrationConnection() { } // EF

    public IntegrationConnection(
        Guid userId,
        IntegrationType type,
        string externalId,
        string accessToken,
        string refreshToken,
        DateTimeOffset expiresAt)
    {
        UserId = userId;
        Type = type;
        ExternalId = externalId;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
        ConnectedAt = DateTimeOffset.UtcNow;
    }

    public Guid UserId { get; private set; }
    public IntegrationType Type { get; private set; }
    public string ExternalId { get; private set; } = string.Empty;
    public string AccessToken { get; private set; } = string.Empty;
    public string RefreshToken { get; private set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset ConnectedAt { get; private set; }
    public DateTimeOffset? LastSyncedAt { get; private set; }

    public void UpdateTokens(string accessToken, string refreshToken, DateTimeOffset expiresAt)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
    }

    public void RecordSync()
    {
        LastSyncedAt = DateTimeOffset.UtcNow;
    }

    public bool IsTokenExpired() => ExpiresAt <= DateTimeOffset.UtcNow;
}
