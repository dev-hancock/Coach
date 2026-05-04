using Athlete.Domain.Health;

namespace Athlete.Domain.Injuries;

/// <summary>
/// Represents an injury or pain tracking entry for an athlete.
/// Tracks onset, location, severity, and impact on training.
/// </summary>
public sealed class InjuryEntry
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid AthleteId { get; private set; }

    public DateOnly OnsetDate { get; private set; }

    public InjuryType Type { get; private set; }

    public InjuryStatus Status { get; private set; }

    public BodyLocation Location { get; private set; }

    public Severity Severity { get; private set; }

    /// <summary>
    /// Pain level on a 1-10 scale.
    /// </summary>
    public int? PainLevel { get; private set; }

    public string? Notes { get; private set; }

    /// <summary>
    /// Indicates if the injury prevents running.
    /// </summary>
    public bool PreventsRunning { get; private set; }

    public bool RequiresFollowUp { get; private set; }

    /// <summary>
    /// Estimated recovery date if known.
    /// </summary>
    public DateOnly? EstimatedRecoveryDate { get; private set; }

    private InjuryEntry()
    {
    }

    public InjuryEntry(
        Guid athleteId,
        DateOnly onsetDate,
        InjuryType type,
        BodyLocation location,
        Severity severity,
        int? painLevel = null,
        string? notes = null,
        bool preventsRunning = false,
        DateOnly? estimatedRecoveryDate = null)
    {
        AthleteId = athleteId;
        OnsetDate = onsetDate;
        Type = type;
        Status = InjuryStatus.Active;
        Location = location;
        Severity = severity;
        PainLevel = painLevel;
        Notes = notes;
        PreventsRunning = preventsRunning;
        EstimatedRecoveryDate = estimatedRecoveryDate;

        // Moderate to severe injuries require follow-up
        RequiresFollowUp = severity is Severity.Moderate or Severity.High or Severity.Severe ||
                          preventsRunning;
    }

    public void UpdateStatus(InjuryStatus newStatus)
    {
        Status = newStatus;

        if (newStatus == InjuryStatus.Recovered)
        {
            RequiresFollowUp = false;
        }
    }

    public void UpdateSeverity(Severity newSeverity, int? newPainLevel = null)
    {
        Severity = newSeverity;
        if (newPainLevel.HasValue)
        {
            PainLevel = newPainLevel;
        }

        // Re-evaluate follow-up requirement
        RequiresFollowUp = newSeverity is Severity.Moderate or Severity.High or Severity.Severe ||
                          PreventsRunning;
    }

    public void MarkPreventsRunning()
    {
        PreventsRunning = true;
        RequiresFollowUp = true;
    }

    public void MarkAllowsRunning()
    {
        PreventsRunning = false;
    }
}
