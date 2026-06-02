using Ardalis.Specification;
using Coach.Domain.Athletes;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find an athlete by name (case-insensitive).
/// </summary>
public sealed class AthleteByNameSpec : Specification<Athlete>
{
    public AthleteByNameSpec(string name)
    {
        Query.Where(a => a.Name.ToLower() == name.ToLower());
    }
}