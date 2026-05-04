using Ardalis.Specification;
using AthleteMcpServer.Domain.Goals;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find upcoming training goals within a date range.
/// </summary>
public sealed class UpcomingGoalsSpec : Specification<TrainingGoal>
{
    public UpcomingGoalsSpec(Guid athleteId, DateOnly startDate, DateOnly endDate)
    {
        Query
            .Where(g => g.AthleteId == athleteId 
                     && g.Status == GoalStatus.Active
                     && g.TargetDate >= startDate 
                     && g.TargetDate <= endDate)
            .OrderBy(g => g.TargetDate);
    }
}
