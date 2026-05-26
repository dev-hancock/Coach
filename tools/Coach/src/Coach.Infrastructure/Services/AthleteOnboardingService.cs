using Coach.Application.Abstractions.Athletes;
using Coach.Domain.Integrations;
using Coach.Domain.Athletes;
using Coach.Domain.Common;
using Coach.Domain.Repositories;
using Ardalis.Specification;
using ErrorOr;

namespace Coach.Infrastructure.Services;

/// <summary>
/// Service for onboarding athletes from external integrations.
/// </summary>
public sealed class AthleteOnboardingService(IAthleteRepository athleteRepository) 
    : IAthleteOnboardingService
{
    public async Task<ErrorOr<Guid>> OnboardFromIntegrationAsync(
        Guid userId,
        IntegrationType integrationType,
        AthleteOnboardingData data,
        CancellationToken cancellationToken = default)
    {
        // Check if athlete already exists for this user
        var spec = new GetAthleteByUserIdSpec(userId);
        var existingAthlete = await athleteRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (existingAthlete is not null)
        {
            // Update existing athlete with integration data
            UpdateAthleteFromIntegration(existingAthlete, data);

            // Raise domain event for integration connection
            existingAthlete.OnIntegrationConnected(
                integrationType,
                data.ExternalAthleteId,
                DateTimeOffset.UtcNow);

            await athleteRepository.UpdateAsync(existingAthlete, cancellationToken);
            return existingAthlete.Id;
        }

        // Create new athlete from integration data
        var athlete = CreateAthleteFromIntegration(userId, data);

        // Raise domain event for integration connection
        athlete.OnIntegrationConnected(
            integrationType,
            data.ExternalAthleteId,
            DateTimeOffset.UtcNow);

        await athleteRepository.AddAsync(athlete, cancellationToken);

        return athlete.Id;
    }

    [Obsolete("Use OnboardFromIntegrationAsync instead")]
    public async Task<ErrorOr<Guid>> OnboardFromStravaAsync(
        Guid userId,
        StravaOnboardingData data,
        CancellationToken cancellationToken = default)
    {
        var genericData = new AthleteOnboardingData(
            ExternalAthleteId: data.StravaAthleteId,
            FirstName: data.FirstName,
            LastName: data.LastName,
            Country: data.Country,
            State: data.State,
            City: data.City,
            Sex: data.Sex,
            Weight: data.Weight,
            MeasurementPreference: data.MeasurementPreference);

        return await OnboardFromIntegrationAsync(
            userId,
            IntegrationType.Strava,
            genericData,
            cancellationToken);
    }

    private static Athlete CreateAthleteFromIntegration(Guid userId, AthleteOnboardingData data)
    {
        var name = $"{data.FirstName} {data.LastName}".Trim();
        var unit = MapMeasurementPreferenceToUnit(data.MeasurementPreference);

        return new Athlete(
            userId,
            name,
            ExperienceLevel.Beginner, // Default - user can update later
            unit);
    }

    private static void UpdateAthleteFromIntegration(Athlete athlete, AthleteOnboardingData data)
    {
        // Update unit preference if available
        if (data.MeasurementPreference is not null)
        {
            var unit = MapMeasurementPreferenceToUnit(data.MeasurementPreference);

            // Only update if athlete hasn't customized their profile yet
            // (You might want to add a flag to Athlete to track this)
            athlete.Update(
                athlete.Experience,
                unit,
                athlete.TrainingDaysPerWeek,
                athlete.PreferredLongRunDay,
                athlete.CurrentWeeklyDistance,
                athlete.TypicalLongRunDistance,
                athlete.Notes);
        }
    }

    private static UnitType MapMeasurementPreferenceToUnit(string? preference)
    {
        return preference?.ToLowerInvariant() switch
        {
            "feet" => UnitType.Imperial,
            "meters" => UnitType.Metric,
            _ => UnitType.Metric // Default to metric
        };
    }
}

/// <summary>
/// Specification to find athlete by user ID.
/// </summary>
internal sealed class GetAthleteByUserIdSpec : Specification<Athlete>
{
    public GetAthleteByUserIdSpec(Guid userId)
    {
        Query.Where(a => a.UserId == userId);
    }
}
