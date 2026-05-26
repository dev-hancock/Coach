using System.Text.Json.Serialization;

namespace Coach.Infrastructure.Integrations.Strava.Contracts;

/// <summary>
/// Strava athlete API response.
/// </summary>
public record StravaAthleteResponse(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("firstname")] string FirstName,
    [property: JsonPropertyName("lastname")] string LastName,
    [property: JsonPropertyName("country")] string? Country,
    [property: JsonPropertyName("state")] string? State,
    [property: JsonPropertyName("city")] string? City,
    [property: JsonPropertyName("sex")] string? Sex,
    [property: JsonPropertyName("weight")] float? Weight,
    [property: JsonPropertyName("measurement_preference")] string? MeasurementPreference);
