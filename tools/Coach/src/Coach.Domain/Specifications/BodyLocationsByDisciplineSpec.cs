using Ardalis.Specification;
using Coach.Domain.Injuries;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find body locations relevant to a specific discipline.
///     Includes locations marked for all disciplines plus discipline-specific ones.
/// </summary>
public sealed class BodyLocationsByDisciplineSpec : Specification<BodyLocation>
{
    public BodyLocationsByDisciplineSpec(Discipline discipline, bool onlyCommon = false)
    {
        Query
            .Where(bl => bl.Discipline == Discipline.All || bl.Discipline == discipline);

        if (onlyCommon)
        {
            Query.Where(bl => bl.IsCommon);
        }

        Query
            .OrderBy(bl => bl.Region)
            .ThenBy(bl => bl.Name);
    }
}