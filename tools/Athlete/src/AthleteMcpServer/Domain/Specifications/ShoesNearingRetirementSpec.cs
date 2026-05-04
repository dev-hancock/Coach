using Ardalis.Specification;
using AthleteMcpServer.Domain.Equipment;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find shoes approaching retirement mileage.
/// </summary>
public sealed class ShoesNearingRetirementSpec : Specification<Shoe>
{
    public ShoesNearingRetirementSpec(Guid athleteId, decimal warningThresholdKm = 50)
    {
        Query
            .Where(s => s.AthleteId == athleteId 
                     && !s.IsRetired
                     && (s.RetireAfterKm - s.DistanceLoggedKm) <= warningThresholdKm)
            .OrderBy(s => s.RetireAfterKm - s.DistanceLoggedKm);
    }
}
