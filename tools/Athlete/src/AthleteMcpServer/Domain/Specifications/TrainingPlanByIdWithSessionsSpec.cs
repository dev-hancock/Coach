using Ardalis.Specification;
using AthleteMcpServer.Domain.TrainingPlans;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find a training plan by ID with all its sessions.
/// </summary>
public sealed class TrainingPlanByIdWithSessionsSpec : Specification<TrainingPlan>
{
    public TrainingPlanByIdWithSessionsSpec(Guid planId)
    {
        Query
            .Where(p => p.Id == planId)
            .Include(p => p.Sessions);
    }
}
