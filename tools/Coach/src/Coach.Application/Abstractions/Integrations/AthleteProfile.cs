namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Generic athlete profile data from external providers.
/// Contains only provider-neutral fields.
/// </summary>
public sealed record AthleteProfile(
    long ExternalAthleteId,
    string FirstName,
    string LastName,
    string? Country,
    string? State,
    string? City,
    string? Sex,
    float? Weight,
    string? MeasurementPreference); // "feet" or "meters"
