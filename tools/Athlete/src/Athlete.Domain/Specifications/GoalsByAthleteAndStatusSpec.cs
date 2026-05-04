using Ardalis.Specification;
using Athlete.Domain.Goals;

namespace Athlete.Domain.Specifications;

/// <summary>
/// Specification to find training goals by athlete and status.
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
