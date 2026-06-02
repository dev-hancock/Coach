using Coach.Domain.Integrations;

namespace Coach.Infrastructure.Identity;

/// <summary>
/// Represents a connection to an external integration service
/// </summary>
public sealed class Integration
{
    /// <summary>
    /// Gets or sets the unique identifier for this integration connection.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID this integration belongs to.
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the type of integration
    /// </summary>
    public required IntegrationType Type { get; set; }

    /// <summary>
    /// Gets or sets the external athlete/user ID in the integration platform.
    /// </summary>
    public required long ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the OAuth access token for API calls.
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the OAuth refresh token for renewing access.
    /// </summary>
    public required string RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets when the access token expires.
    /// </summary>
    public required DateTime TokenExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets when this integration was first connected.
    /// </summary>
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets when activities were last synced from this integration.
    /// </summary>
    public DateTime? LastSyncedAt { get; set; }

    /// <summary>
    /// Gets or sets additional metadata about this integration (JSON).
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Navigation property to the user.
    /// </summary>
    public User? User { get; set; }
}
