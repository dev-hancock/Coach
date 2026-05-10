using Coach.Domain.Common;

namespace Coach.Domain.Athletes;

/// <summary>
///     Athlete aggregate root representing a training program participant.
/// </summary>
public sealed class Athlete : AggregateRoot
{
    private Athlete()
    {
    }

    public Athlete(
        Guid userId,
        string name,
        ExperienceLevel experience = ExperienceLevel.Beginner,
        UnitType unit = UnitType.Metric)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        UserId = userId;
        Name = name;
        Experience = experience;
        Unit = unit;
    }

    /// <summary>
    /// Reference to the authentication user.
    /// </summary>
    public Guid UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public ExperienceLevel Experience { get; private set; } = ExperienceLevel.Beginner;

    public UnitType Unit { get; private set; } = UnitType.Metric;

    public int TrainingDaysPerWeek { get; private set; }

    public DayOfWeek? PreferredLongRunDay { get; private set; }

    public Distance CurrentWeeklyDistance { get; private set; } = Distance.Zero;

    public Distance TypicalLongRunDistance { get; private set; } = Distance.Zero;

    public string? Notes { get; private set; }

    public void UpdateProfile(
        ExperienceLevel experienceLevel,
        UnitType unit,
        int trainingDaysPerWeek,
        DayOfWeek? preferredLongRunDay,
        Distance currentWeeklyDistance,
        Distance typicalLongRunDistance,
        string? notes = null)
    {
        if (trainingDaysPerWeek is < 1 or > 7)
        {
            throw new InvalidOperationException("Training days per week must be between 1 and 7.");
        }

        Experience = experienceLevel;
        Unit = unit;
        TrainingDaysPerWeek = trainingDaysPerWeek;
        PreferredLongRunDay = preferredLongRunDay;
        CurrentWeeklyDistance = currentWeeklyDistance;
        TypicalLongRunDistance = typicalLongRunDistance;
        Notes = notes;
    }
}