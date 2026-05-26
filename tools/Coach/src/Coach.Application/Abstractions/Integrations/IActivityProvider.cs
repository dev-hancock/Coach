using Coach.Domain.Integrations;
using ErrorOr;

namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Port for fetching activities from external providers.
/// </summary>
public interface IActivityProvider
{
    /// <summary>
    /// Identifies which integration type this provider handles.
    /// </summary>
    IntegrationType ProviderType { get; }

    /// <summary>
    /// Fetch activities from provider.
    /// Supports pagination and incremental sync via 'since' parameter.
    /// </summary>
    Task<ErrorOr<IReadOnlyList<ExternalActivity>>> GetActivitiesAsync(
        string accessToken,
        DateTime? since = null,
        int pageSize = 30,
        CancellationToken cancellationToken = default);
}
