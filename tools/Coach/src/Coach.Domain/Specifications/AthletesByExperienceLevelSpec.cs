using Ardalis.Specification;
using Coach.Domain.Athletes;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find athletes by experience level.
/// </summary>
public sealed class AthletesByExperienceLevelSpec : Specification<Athlete>
{
    public AthletesByExperienceLevelSpec(ExperienceLevel experienceLevel)
    {
        Query.Where(a => a.Experience == experienceLevel);
    }
}