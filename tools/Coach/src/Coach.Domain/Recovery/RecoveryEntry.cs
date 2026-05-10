using Coach.Domain.Common;
using Coach.Domain.Health;

namespace Coach.Domain.Recovery;

/// <summary>
///     Recovery entry aggregate root tracking sleep, nutrition, and recovery factors.
/// </summary>
public sealed class RecoveryEntry : AggregateRoot
{
    private RecoveryEntry()
    {
    }

    public RecoveryEntry(
        Guid athleteId,
        DateOnly date,
        RecoveryType type,
        RecoveryQuality quality,
        Severity severity,
        double? sleepHours = null,
        string? notes = null,
        bool affectsPerformance = false)
    {
        AthleteId = athleteId;
        Date = date;
        Type = type;
        Quality = quality;
        Severity = severity;
        SleepHours = sleepHours;
        Notes = notes;
        AffectsPerformance = affectsPerformance;

        // Poor quality or high severity requires attention
        RequiresAttention = quality == RecoveryQuality.Poor ||
                            severity is Severity.High or Severity.Severe ||
                            affectsPerformance;
    }

    public Guid AthleteId { get; private set; }

    public DateOnly Date { get; private set; }

    public RecoveryType Type { get; private set; }

    public RecoveryQuality Quality { get; private set; }

    public Severity Severity { get; }

    /// <summary>
    ///     Sleep hours for sleep-type entries.
    /// </summary>
    public double? SleepHours { get; private set; }

    public string? Notes { get; private set; }

    /// <summary>
    ///     Indicates if this recovery issue is affecting training performance.
    /// </summary>
    public bool AffectsPerformance { get; }

    public bool RequiresAttention { get; private set; }

    public void MarkRequiresAttention()
    {
        RequiresAttention = true;
    }

    public void MarkResolved()
    {
        RequiresAttention = false;
    }

    public void UpdateQuality(RecoveryQuality newQuality)
    {
        Quality = newQuality;

        // Re-evaluate attention requirement
        RequiresAttention = newQuality == RecoveryQuality.Poor ||
                            Severity is Severity.High or Severity.Severe ||
                            AffectsPerformance;
    }
}