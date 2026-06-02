using Coach.Domain.Common;

namespace Coach.Domain.Activities;

/// <summary>
///     Aggregate root for completed athletic activities (run, ride, swim, etc.).
///     Sport-agnostic design allows optional metrics based on activity type.
/// </summary>
public sealed class Activity : AggregateRoot
{
    private Activity()
    {
    }

    /// <summary>
    ///     Create a new activity with core metrics.
    ///     Additional metrics can be added via AddMetrics methods.
    /// </summary>
    public Activity(
        Guid athleteId,
        ActivityType type,
        DateTimeOffset startedAt,
        TimeSpan duration,
        decimal? distanceKm = null,
        int? ratePerceivedExertion = null,
        ActivitySource source = ActivitySource.Manual)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new InvalidOperationException("Activity duration must be greater than zero.");
        }

        if (distanceKm is <= 0)
        {
            throw new InvalidOperationException("Activity distance must be greater than zero if provided.");
        }

        if (ratePerceivedExertion is < 1 or > 10)
        {
            throw new InvalidOperationException("RPE must be between 1 and 10.");
        }

        AthleteId = athleteId;
        Type = type;
        StartedAt = startedAt;
        Duration = duration;
        DistanceKm = distanceKm;
        RatePerceivedExertion = ratePerceivedExertion;
        Source = source;

        // Calculate pace/speed metrics using value object
        Metrics = ActivityMetrics.Calculate(type, distanceKm, duration);
    }

    public Guid AthleteId { get; private set; }

    public ActivityType Type { get; private set; }

    public Guid? PlannedSessionId { get; private set; }

    public Guid? EquipmentId { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    // Core metrics - applicable to most activities
    public decimal? DistanceKm { get; private set; }

    public TimeSpan Duration { get; private set; }

    public TimeSpan? MovingTime { get; private set; }

    // Sport-agnostic metrics - not all activities have all metrics
    public int? AverageHeartRate { get; private set; }

    public int? MaxHeartRate { get; private set; }

    public int? AverageCadence { get; private set; }

    public decimal? ElevationGainMeters { get; private set; }

    public decimal? ElevationLossMeters { get; private set; }

    public int? RatePerceivedExertion { get; private set; }

    // Calculated performance metrics - encapsulated in value object
    public ActivityMetrics? Metrics { get; private set; }

    // Power metrics (cycling, rowing)
    public int? AveragePowerWatts { get; private set; }

    public int? NormalizedPowerWatts { get; private set; }

    // Swimming-specific
    public int? AverageStrokeRate { get; private set; }

    public int? Swolf { get; private set; }

    public int? PoolLengthMeters { get; private set; }

    // Strength/functional fitness
    public int? TotalReps { get; private set; }

    public decimal? TotalWeightKg { get; private set; }

    // Import metadata
    public ActivitySource Source { get; private set; } = ActivitySource.Manual;

    public string? ExternalActivityId { get; private set; }

    public string? Notes { get; private set; }

    public void AttachToPlannedSession(Guid plannedSessionId)
    {
        PlannedSessionId = plannedSessionId;
    }

    public void AssignEquipment(Guid equipmentId)
    {
        EquipmentId = equipmentId;
    }

    /// <summary>
    ///     Add general metrics applicable to most endurance activities.
    /// </summary>
    public void AddGeneralMetrics(
        TimeSpan? movingTime,
        int? averageHeartRate,
        int? maxHeartRate,
        int? averageCadence,
        decimal? elevationGainMeters,
        decimal? elevationLossMeters = null)
    {
        MovingTime = movingTime;
        AverageHeartRate = averageHeartRate;
        MaxHeartRate = maxHeartRate;
        AverageCadence = averageCadence;
        ElevationGainMeters = elevationGainMeters;
        ElevationLossMeters = elevationLossMeters;
    }

    /// <summary>
    ///     Add cycling-specific power metrics.
    /// </summary>
    public void AddPowerMetrics(int? averagePowerWatts, int? normalizedPowerWatts = null)
    {
        AveragePowerWatts = averagePowerWatts;
        NormalizedPowerWatts = normalizedPowerWatts;
    }

    /// <summary>
    ///     Add swimming-specific metrics.
    /// </summary>
    public void AddSwimMetrics(
        int? averageStrokeRate = null,
        int? swolf = null,
        int? poolLengthMeters = null)
    {
        AverageStrokeRate = averageStrokeRate;
        Swolf = swolf;
        PoolLengthMeters = poolLengthMeters;
    }

    /// <summary>
    ///     Add strength/functional fitness metrics.
    /// </summary>
    public void AddStrengthMetrics(int? totalReps = null, decimal? totalWeightKg = null)
    {
        TotalReps = totalReps;
        TotalWeightKg = totalWeightKg;
    }

    public void AddExternalReference(ActivitySource source, string externalActivityId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(externalActivityId);

        Source = source;
        ExternalActivityId = externalActivityId;
    }

    public void AddNotes(string notes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(notes);
        Notes = notes;
    }
}