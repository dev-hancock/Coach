using Coach.Domain.Integrations;
using ErrorOr;

namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Application service for managing integration connections.
/// Implemented per provider in Infrastructure layer (e.g., StravaService).
/// </summary>
public interface IIntegrationService
{
    /// <summary>
    /// The integration type this service handles.
    /// </summary>
    IntegrationType Type { get; }

    /// <summary>
    /// Connects a user's account to the integration using an OAuth authorization code.
    /// </summary>
    Task<ErrorOr<IntegrationConnectionResult>> ConnectAsync(
        Guid userId,
        string authorizationCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects a user's integration and revokes access.
    /// </summary>
    Task<ErrorOr<Success>> DisconnectAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Streams activities for sync from the integration provider.
    /// </summary>
    IAsyncEnumerable<ErrorOr<ActivityInfo>> SyncActivitiesAsync(
        Guid userId,
        DateTimeOffset? since = null,
        CancellationToken cancellationToken = default);
}
