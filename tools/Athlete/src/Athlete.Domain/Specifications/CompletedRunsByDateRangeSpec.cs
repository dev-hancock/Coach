using Ardalis.Specification;
using Athlete.Domain.Activities;

namespace Athlete.Domain.Specifications;

/// <summary>
/// Specification to find completed runs for an athlete within a date range.
/// </summary>
public sealed class CompletedRunsByDateRangeSpec : Specification<CompletedRun>
{
    public CompletedRunsByDateRangeSpec(Guid athleteId, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        Query
            .Where(r => r.AthleteId == athleteId 
                     && r.StartedAt >= startDate 
                     && r.StartedAt <= endDate)
            .OrderByDescending(r => r.StartedAt);
    }
}
