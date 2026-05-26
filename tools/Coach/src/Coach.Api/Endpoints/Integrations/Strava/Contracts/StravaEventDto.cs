namespace Coach.Api.Endpoints.Integrations.Strava.Contracts;

/// <summary>
/// Strava webhook event DTO.
/// </summary>
public sealed record StravaEventDto(
    string ObjectType,
    long ObjectId,
    string AspectType,
    Dictionary<string, object>? Updates);