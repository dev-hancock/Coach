namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Generic activity information from external integration for syncing.
/// </summary>
public sealed record ActivityInfo(
    string ExternalId,
    string Name,
    string Type,
    DateTimeOffset StartDate,
    int DurationSeconds,
    double DistanceMeters,
    double? ElevationGainMeters = null,
    double? MaxHeartRate = null,
    double? AverageHeartRate = null,
    double? MaxSpeed = null,
    double? AverageSpeed = null);
