using Ardalis.Specification;
using Coach.Domain.Training;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find a training plan by ID with all its sessions.
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