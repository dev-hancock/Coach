using Coach.Domain.Integrations;
using ErrorOr;

namespace Coach.Application.Abstractions.Athletes;

/// <summary>
/// Service for onboarding athletes from external integrations.
/// </summary>
public interface IAthleteOnboardingService
{
    /// <summary>
    /// Create or update athlete profile from integration provider data.
    /// </summary>
    Task<ErrorOr<Guid>> OnboardFromIntegrationAsync(
        Guid userId,
        IntegrationType integrationType,
        AthleteOnboardingData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create or update athlete profile from Strava integration data.
    /// </summary>
    [Obsolete("Use OnboardFromIntegrationAsync instead. This will be removed in a future version.")]
    Task<ErrorOr<Guid>> OnboardFromStravaAsync(
        Guid userId,
        StravaOnboardingData data,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Generic data required to onboard an athlete from any integration provider.
/// </summary>
public sealed record AthleteOnboardingData(
    string ExternalAthleteId,
    string FirstName,
    string LastName,
    string? Country,
    string? State,
    string? City,
    string? Sex,
    float? Weight,
    string? MeasurementPreference);

/// <summary>
/// Data required to onboard an athlete from Strava.
/// </summary>
[Obsolete("Use AthleteOnboardingData instead. This will be removed in a future version.")]
public sealed record StravaOnboardingData(
    long StravaAthleteId,
    string FirstName,
    string LastName,
    string? Country,
    string? State,
    string? City,
    string? Sex,
    float? Weight,
    string? MeasurementPreference);
