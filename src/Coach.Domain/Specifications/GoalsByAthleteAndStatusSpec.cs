using Ardalis.Specification;
using Coach.Domain.Goals;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find training goals by athlete and status.
/// </summary>
public sealed class GoalsByAthleteAndStatusSpec : Specification<TrainingGoal>
{
    public GoalsByAthleteAndStatusSpec(Guid athleteId, GoalStatus status)
    {
        Query
            .Where(g => g.AthleteId == athleteId && g.Status == status)
            .OrderByDescending(g => g.TargetDate);
    }
}