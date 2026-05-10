using Coach.Domain.Common;

namespace Coach.Domain.Goals;

/// <summary>
///     Training goal aggregate root representing an athlete's race or fitness target.
/// </summary>
public sealed class TrainingGoal : AggregateRoot
{
    private TrainingGoal()
    {
    }

    public TrainingGoal(
        Guid athleteId,
        RaceDistance distance,
        DateOnly targetDate,
        TimeSpan? targetTime = null,
        string? description = null)
    {
        AthleteId = athleteId;
        Distance = distance;
        TargetDate = targetDate;
        TargetTime = targetTime;
        Description = description;
    }

    public Guid AthleteId { get; private set; }

    public RaceDistance Distance { get; private set; }

    public DateOnly TargetDate { get; private set; }

    public TimeSpan? TargetTime { get; private set; }

    public string? Description { get; private set; }

    public GoalStatus Status { get; private set; } = GoalStatus.Active;

    public void ChangeTarget(DateOnly targetDate, TimeSpan? targetTime)
    {
        if (Status != GoalStatus.Active)
        {
            throw new InvalidOperationException("Only active goals can be changed.");
        }

        TargetDate = targetDate;
        TargetTime = targetTime;
    }

    public void Complete()
    {
        Status = GoalStatus.Completed;
    }

    public void Abandon()
    {
        Status = GoalStatus.Abandoned;
    }
}

