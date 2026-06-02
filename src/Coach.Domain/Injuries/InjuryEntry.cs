using Coach.Domain.Common;
using Coach.Domain.Health;

namespace Coach.Domain.Injuries;

/// <summary>
///     Injury entry aggregate root tracking athlete injuries and pain.
///     Tracks onset, location, severity, and impact on training across triathlon disciplines.
/// </summary>
public sealed class InjuryEntry : AggregateRoot
{
    private InjuryEntry()
    {
    }

    public InjuryEntry(
        Guid athleteId,
        DateOnly onsetDate,
        InjuryType type,
        Guid bodyLocationId,
        Severity severity,
        int? painLevel = null,
        string? notes = null,
        bool preventsTraining = false,
        DateOnly? estimatedRecoveryDate = null)
    {
        AthleteId = athleteId;
        OnsetDate = onsetDate;
        Type = type;
        Status = InjuryStatus.Active;
        BodyLocationId = bodyLocationId;
        Severity = severity;
        PainLevel = painLevel;
        Notes = notes;
        PreventsTraining = preventsTraining;
        EstimatedRecoveryDate = estimatedRecoveryDate;

        // Moderate to severe injuries require follow-up
        RequiresFollowUp = severity is Severity.Moderate or Severity.High or Severity.Severe ||
                           preventsTraining;
    }

    public Guid AthleteId { get; private set; }

    public DateOnly OnsetDate { get; private set; }

    public InjuryType Type { get; private set; }

    public InjuryStatus Status { get; private set; }

    /// <summary>
    ///     Reference to the body location where injury occurred.
    /// </summary>
    public Guid BodyLocationId { get; private set; }

    public Severity Severity { get; private set; }

    /// <summary>
    ///     Pain level on a 1-10 scale.
    /// </summary>
    public int? PainLevel { get; private set; }

    public string? Notes { get; private set; }

    /// <summary>
    ///     Indicates if the injury prevents training in specific or all disciplines.
    /// </summary>
    public bool PreventsTraining { get; private set; }

    public bool RequiresFollowUp { get; private set; }

    /// <summary>
    ///     Estimated recovery date if known.
    /// </summary>
    public DateOnly? EstimatedRecoveryDate { get; private set; }

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
                           PreventsTraining;
    }

    public void MarkPreventsTraining()
    {
        PreventsTraining = true;
        RequiresFollowUp = true;
    }

    public void MarkAllowsTraining()
    {
        PreventsTraining = false;
    }
}