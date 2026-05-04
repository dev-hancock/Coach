using Ardalis.Specification;
using AthleteMcpServer.Domain.Activities;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find recent completed runs for an athlete.
/// </summary>
public sealed class RecentCompletedRunsSpec : Specification<CompletedRun>
{
    public RecentCompletedRunsSpec(Guid athleteId, int count = 10)
    {
        Query
            .Where(r => r.AthleteId == athleteId)
            .OrderByDescending(r => r.StartedAt)
            .Take(count);
    }
}
