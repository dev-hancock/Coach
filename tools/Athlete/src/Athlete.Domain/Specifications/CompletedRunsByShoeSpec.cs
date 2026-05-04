using Ardalis.Specification;
using Athlete.Domain.Activities;

namespace Athlete.Domain.Specifications;

/// <summary>
/// Specification to find completed runs by shoe.
/// Useful for tracking shoe mileage.
/// </summary>
public sealed class CompletedRunsByShoeSpec : Specification<CompletedRun>
{
    public CompletedRunsByShoeSpec(Guid shoeId)
    {
        Query
            .Where(r => r.ShoeId == shoeId)
            .OrderByDescending(r => r.StartedAt);
    }
}
