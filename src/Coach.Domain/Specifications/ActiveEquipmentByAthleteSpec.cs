using Ardalis.Specification;
using Coach.Domain.Gear;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find active (non-retired) equipment for an athlete.
/// </summary>
public sealed class ActiveEquipmentByAthleteSpec : Specification<Equipment>
{
    public ActiveEquipmentByAthleteSpec(Guid athleteId)
    {
        Query
            .Where(e => e.AthleteId == athleteId && !e.IsRetired)
            .OrderBy(e => e.Type)
            .ThenBy(e => e.Name);
    }

    public ActiveEquipmentByAthleteSpec(Guid athleteId, EquipmentType type)
    {
        Query
            .Where(e => e.AthleteId == athleteId && e.Type == type && !e.IsRetired)
            .OrderBy(e => e.Name);
    }
}