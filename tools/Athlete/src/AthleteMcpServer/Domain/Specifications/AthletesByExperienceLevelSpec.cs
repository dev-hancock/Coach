using Ardalis.Specification;
using AthleteMcpServer.Domain.Athletes;

namespace AthleteMcpServer.Domain.Specifications;

/// <summary>
/// Specification to find athletes by experience level.
/// </summary>
public sealed class AthletesByExperienceLevelSpec : Specification<Athlete>
{
    public AthletesByExperienceLevelSpec(ExperienceLevel experienceLevel)
    {
        Query.Where(a => a.ExperienceLevel == experienceLevel);
    }
}
