using Ardalis.Specification;
using AthleteMcpServer.Domain.Equipment;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find active (non-retired) shoes for an athlete.
/// </summary>
public sealed class ActiveShoesByAthleteSpec : Specification<Shoe>
{
    public ActiveShoesByAthleteSpec(Guid athleteId)
    {
        Query
            .Where(s => s.AthleteId == athleteId && !s.IsRetired)
            .OrderBy(s => s.Name);
    }
}
