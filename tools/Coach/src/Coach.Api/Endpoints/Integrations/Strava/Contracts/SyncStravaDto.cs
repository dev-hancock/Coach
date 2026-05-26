namespace Coach.Api.Endpoints.Integrations.Strava.Contracts;

/// <summary>
/// Request DTO for sync endpoint.
/// </summary>
public sealed record SyncStravaDto(DateTime? Since = null, int? PageSize = null);