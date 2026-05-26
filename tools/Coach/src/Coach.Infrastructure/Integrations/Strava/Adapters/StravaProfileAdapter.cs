using Coach.Application.Abstractions.Integrations;
using Coach.Domain.Integrations;
using ErrorOr;

namespace Coach.Infrastructure.Integrations.Strava.Adapters;

/// <summary>
/// Strava implementation of athlete profile provider port.
/// Maps Strava athlete response to generic athlete profile.
/// </summary>
internal sealed class StravaProfileAdapter(IStravaApi api) : IAthleteProfileProvider
{
    public IntegrationType ProviderType => IntegrationType.Strava;

    public async Task<ErrorOr<AthleteProfile>> GetProfileAsync(
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var athlete = await api.GetAthleteAsync($"Bearer {accessToken}");

            return new AthleteProfile(
                ExternalAthleteId: athlete.Id,
                FirstName: athlete.FirstName ?? "Unknown",
                LastName: athlete.LastName ?? "Unknown",
                Country: athlete.Country,
                State: athlete.State,
                City: athlete.City,
                Sex: athlete.Sex,
                Weight: athlete.Weight,
                MeasurementPreference: athlete.MeasurementPreference);
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.Profile.GetFailed",
                $"Failed to get athlete profile: {ex.Message}");
        }
    }
}
