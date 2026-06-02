using Ardalis.Specification;
using Coach.Domain.Gear;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find equipment nearing retirement based on logged distance.
/// </summary>
public sealed class EquipmentNearingRetirementSpec : Specification<Equipment>
{
    public EquipmentNearingRetirementSpec(Guid athleteId, decimal percentageThreshold = 0.8m)
    {
        Query
            .Where(e => e.AthleteId == athleteId
                        && !e.IsRetired
                        && e.RetireAfterKm.HasValue
                        && e.DistanceLoggedKm >= e.RetireAfterKm.Value * percentageThreshold)
            .OrderByDescending(e => e.DistanceLoggedKm);
    }
}