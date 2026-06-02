using Coach.Domain.Integrations;
using ErrorOr;

namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Port for fetching athlete profile from external providers.
/// </summary>
public interface IAthleteProfileProvider
{
    /// <summary>
    /// Identifies which integration type this provider handles.
    /// </summary>
    IntegrationType ProviderType { get; }

    /// <summary>
    /// Get athlete profile using access token.
    /// </summary>
    Task<ErrorOr<AthleteProfile>> GetProfileAsync(
        string accessToken,
        CancellationToken cancellationToken = default);
}
