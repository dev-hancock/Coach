using Ardalis.Specification;
using AthleteMcpServer.Domain.TrainingPlans;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find incomplete planned sessions (not completed or skipped).
/// </summary>
public sealed class IncompletePlannedSessionsSpec : Specification<PlannedSession>
{
    public IncompletePlannedSessionsSpec(Guid trainingPlanId)
    {
        Query
            .Where(s => s.TrainingPlanId == trainingPlanId 
                     && s.Status != PlannedSessionStatus.Completed
                     && s.Status != PlannedSessionStatus.Skipped)
            .OrderBy(s => s.Date);
    }
}
