using Ardalis.Specification;
using Athlete.Domain.Goals;

namespace Athlete.Domain.Specifications;

/// <summary>
/// Specification to find active training goals for a specific athlete.
/// </summary>
public sealed class ActiveGoalsByAthleteSpec : Specification<TrainingGoal>
{
    public ActiveGoalsByAthleteSpec(Guid athleteId)
    {
        Query
            .Where(g => g.AthleteId == athleteId && g.Status == GoalStatus.Active)
            .OrderBy(g => g.TargetDate);
    }
}
