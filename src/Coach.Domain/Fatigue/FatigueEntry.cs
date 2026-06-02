using Coach.Domain.Common;
using Coach.Domain.Health;

namespace Coach.Domain.Fatigue;

/// <summary>
///     Fatigue entry aggregate root tracking accumulated training stress and tiredness.
/// </summary>
public sealed class FatigueEntry : AggregateRoot
{
    private FatigueEntry()
    {
    }

    public FatigueEntry(
        Guid athleteId,
        DateOnly date,
        FatigueLevel level,
        Severity severity,
        int? ratePerceivedExertion = null,
        string? notes = null,
        bool affectsTraining = false)
    {
        AthleteId = athleteId;
        Date = date;
        Level = level;
        Severity = severity;
        RatePerceivedExertion = ratePerceivedExertion;
        Notes = notes;
        AffectsTraining = affectsTraining;

        // High or extreme fatigue requires rest
        RequiresRest = level is FatigueLevel.High or FatigueLevel.Extreme ||
                       severity is Severity.High or Severity.Severe;
    }

    public Guid AthleteId { get; private set; }

    public DateOnly Date { get; private set; }

    public FatigueLevel Level { get; private set; }

    public Severity Severity { get; private set; }

    /// <summary>
    ///     Rate of Perceived Exertion (1-10 scale)
    /// </summary>
    public int? RatePerceivedExertion { get; private set; }

    public string? Notes { get; private set; }

    /// <summary>
    ///     Indicates if fatigue is affecting ability to complete planned training.
    /// </summary>
    public bool AffectsTraining { get; private set; }

    public bool RequiresRest { get; private set; }

    public void MarkRequiresRest()
    {
        RequiresRest = true;
    }

    public void MarkRecovered()
    {
        RequiresRest = false;
    }
}