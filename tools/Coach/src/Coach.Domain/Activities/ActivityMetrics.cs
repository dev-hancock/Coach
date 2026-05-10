using Coach.Domain.Common;

namespace Coach.Domain.Activities;

/// <summary>
///     Value object representing calculated activity performance metrics (pace or speed).
///     Encapsulates the logic for calculating sport-specific performance measures.
/// </summary>
public sealed record ActivityMetrics : ValueObject
{
    private ActivityMetrics(TimeSpan? pacePerKm, decimal? speedKph)
    {
        PacePerKm = pacePerKm;
        SpeedKph = speedKph;
    }

    /// <summary>
    ///     Average pace per kilometer (for running/swimming).
    ///     Null if activity type uses speed instead.
    /// </summary>
    public TimeSpan? PacePerKm { get; init; }

    /// <summary>
    ///     Average speed in km/h (for cycling).
    ///     Null if activity type uses pace instead.
    /// </summary>
    public decimal? SpeedKph { get; init; }

    /// <summary>
    ///     Calculate metrics based on activity type, distance, and duration.
    ///     Returns null if distance is not available.
    /// </summary>
    public static ActivityMetrics? Calculate(
        ActivityType activityType,
        decimal? distanceKm,
        TimeSpan duration)
    {
        if (!distanceKm.HasValue || distanceKm.Value <= 0)
        {
            return null;
        }

        if (duration <= TimeSpan.Zero)
        {
            return null;
        }

        return activityType switch
        {
            ActivityType.Run or ActivityType.Swim =>
                new ActivityMetrics(
                    TimeSpan.FromSeconds(duration.TotalSeconds / (double)distanceKm.Value),
                    null),

            ActivityType.Ride =>
                new ActivityMetrics(
                    null,
                    distanceKm.Value / (decimal)duration.TotalHours),

            _ => null // Strength, Workout, Other don't have pace/speed
        };
    }

    /// <summary>
    ///     Create metrics with explicit pace (running/swimming).
    /// </summary>
    public static ActivityMetrics FromPace(TimeSpan pacePerKm)
    {
        if (pacePerKm <= TimeSpan.Zero)
        {
            throw new ArgumentException("Pace must be greater than zero.", nameof(pacePerKm));
        }

        return new ActivityMetrics(pacePerKm, null);
    }

    /// <summary>
    ///     Create metrics with explicit speed (cycling).
    /// </summary>
    public static ActivityMetrics FromSpeed(decimal speedKph)
    {
        if (speedKph <= 0)
        {
            throw new ArgumentException("Speed must be greater than zero.", nameof(speedKph));
        }

        return new ActivityMetrics(null, speedKph);
    }
}