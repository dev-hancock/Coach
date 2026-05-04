using Ardalis.Specification;
using AthleteMcpServer.Domain.TrainingPlans;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find active training plans for an athlete.
/// Includes all planned sessions.
/// </summary>
public sealed class ActiveTrainingPlansByAthleteSpec : Specification<TrainingPlan>
{
    public ActiveTrainingPlansByAthleteSpec(Guid athleteId)
    {
        Query
            .Where(p => p.AthleteId == athleteId && p.IsActive)
            .Include(p => p.Sessions)
            .OrderByDescending(p => p.StartDate);
    }
}
