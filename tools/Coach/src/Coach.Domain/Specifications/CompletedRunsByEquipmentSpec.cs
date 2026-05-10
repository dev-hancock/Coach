using Ardalis.Specification;
using Coach.Domain.Activities;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find completed runs by equipment.
///     Useful for tracking equipment mileage.
/// </summary>
public sealed class CompletedRunsByEquipmentSpec : Specification<Activity>
{
    public CompletedRunsByEquipmentSpec(Guid equipmentId)
    {
        Query
            .Where(r => r.EquipmentId == equipmentId)
            .OrderByDescending(r => r.StartedAt);
    }
}