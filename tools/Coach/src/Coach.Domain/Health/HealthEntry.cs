using Coach.Domain.Common;

namespace Coach.Domain.Health;

/// <summary>
///     Health entry aggregate root tracking general athlete wellness.
/// </summary>
public sealed class HealthEntry : AggregateRoot
{
    private HealthEntry()
    {
    }

    public HealthEntry(
        Guid athleteId,
        DateOnly date,
        HealthEntryType entryType,
        Severity severity,
        string? notes = null,
        string? bodyLocation = null,
        bool affectedRunning = false)
    {
        AthleteId = athleteId;
        Date = date;
        EntryType = entryType;
        Severity = severity;
        Notes = notes;
        BodyLocation = bodyLocation;
        AffectedRunning = affectedRunning;

        RequiresFollowUp =
            severity is Severity.High or Severity.Severe ||
            entryType is HealthEntryType.Pain;
    }

    public Guid AthleteId { get; private set; }

    public DateOnly Date { get; private set; }

    public HealthEntryType EntryType { get; private set; }

    public Severity Severity { get; }

    public string? BodyLocation { get; private set; }

    public string? Notes { get; private set; }

    public bool AffectedRunning { get; private set; }

    public bool RequiresFollowUp { get; private set; }

    public void MarkRequiresFollowUp()
    {
        RequiresFollowUp = true;
    }

    public void MarkResolved()
    {
        RequiresFollowUp = false;
    }
}