using Athlete.Domain.Health;

namespace Athlete.Domain.Recovery;

/// <summary>
/// Represents a recovery tracking entry for an athlete.
/// Tracks sleep, nutrition, illness, stress, and other recovery factors.
/// </summary>
public sealed class RecoveryEntry
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid AthleteId { get; private set; }

    public DateOnly Date { get; private set; }

    public RecoveryType Type { get; private set; }

    public RecoveryQuality Quality { get; private set; }

    public Severity Severity { get; private set; }

    /// <summary>
    /// Sleep hours for sleep-type entries.
    /// </summary>
    public double? SleepHours { get; private set; }

    public string? Notes { get; private set; }

    /// <summary>
    /// Indicates if this recovery issue is affecting training performance.
    /// </summary>
    public bool AffectsPerformance { get; private set; }

    public bool RequiresAttention { get; private set; }

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
