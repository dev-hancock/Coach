using Ardalis.Specification;
using AthleteMcpServer.Domain.Health;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find active health entries requiring follow-up.
/// </summary>
public sealed class HealthEntriesRequiringFollowUpSpec : Specification<HealthEntry>
{
    public HealthEntriesRequiringFollowUpSpec(Guid athleteId)
    {
        Query
            .Where(r => r.AthleteId == athleteId && r.RequiresFollowUp)
            .OrderByDescending(r => r.Date);
    }
}
