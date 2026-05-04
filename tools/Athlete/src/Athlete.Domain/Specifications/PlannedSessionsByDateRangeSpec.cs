using Ardalis.Specification;
using Athlete.Domain.TrainingPlans;

namespace Athlete.Domain.Specifications;

/// <summary>
/// Specification to find planned sessions within a date range for an athlete.
/// </summary>
public sealed class PlannedSessionsByDateRangeSpec : Specification<PlannedSession>
{
    public PlannedSessionsByDateRangeSpec(Guid trainingPlanId, DateOnly startDate, DateOnly endDate)
    {
        Query
            .Where(s => s.TrainingPlanId == trainingPlanId 
                     && s.Date >= startDate 
                     && s.Date <= endDate)
            .OrderBy(s => s.Date);
    }
}
