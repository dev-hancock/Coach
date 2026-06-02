using Ardalis.Specification;
using Coach.Domain.Activities;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find recent completed runs for an athlete.
/// </summary>
public sealed class RecentCompletedRunsSpec : Specification<Activity>
{
    public RecentCompletedRunsSpec(Guid athleteId, int count = 10)
    {
        Query
            .Where(r => r.AthleteId == athleteId)
            .OrderByDescending(r => r.StartedAt)
            .Take(count);
    }
}