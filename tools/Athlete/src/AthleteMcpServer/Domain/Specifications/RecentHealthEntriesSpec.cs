using Ardalis.Specification;
using AthleteMcpServer.Domain.Health;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find recent health entries for an athlete.
/// </summary>
public sealed class RecentHealthEntriesSpec : Specification<HealthEntry>
{
    public RecentHealthEntriesSpec(Guid athleteId, int daysBack = 30)
    {
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-daysBack));

        Query
            .Where(r => r.AthleteId == athleteId && r.Date >= startDate)
            .OrderByDescending(r => r.Date);
    }
}
