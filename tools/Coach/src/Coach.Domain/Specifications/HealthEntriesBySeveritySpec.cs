using Ardalis.Specification;
using Coach.Domain.Health;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find health entries by severity and type.
///     Useful for identifying patterns or risks.
/// </summary>
public sealed class HealthEntriesBySeveritySpec : Specification<HealthEntry>
{
    public HealthEntriesBySeveritySpec(Guid athleteId, Severity minimumSeverity, int daysBack = 90)
    {
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-daysBack));

        Query
            .Where(r => r.AthleteId == athleteId
                        && r.Date >= startDate
                        && r.Severity >= minimumSeverity)
            .OrderByDescending(r => r.Date);
    }
}