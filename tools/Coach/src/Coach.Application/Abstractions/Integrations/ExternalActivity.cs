using Coach.Domain.Activities;
using Coach.Domain.Common;

namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Generic activity data from external providers.
/// Mapped to domain value objects (Distance, ActivityType).
/// </summary>
public sealed record ExternalActivity(
    string ExternalId,
    string Name,
    ActivityType Type,
    DateTime StartDate,
    TimeSpan Duration,
    Distance Distance,
    int? AverageHeartRate,
    int? MaxHeartRate,
    int? Calories,
    float? ElevationGain);
