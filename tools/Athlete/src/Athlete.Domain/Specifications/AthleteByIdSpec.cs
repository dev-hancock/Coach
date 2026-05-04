using Ardalis.Specification;
using Athlete.Domain.Athletes;

namespace Athlete.Domain.Specifications;

/// <summary>
/// Specification to find an athlete by their unique ID.
/// </summary>
public sealed class AthleteByIdSpec : SingleResultSpecification<Athlete>
{
    public AthleteByIdSpec(Guid athleteId)
    {
        Query.Where(a => a.Id == athleteId);
    }
}

