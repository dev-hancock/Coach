using Ardalis.Specification;
using Athlete.Domain.Coaching;

namespace Athlete.Domain.Specifications;

/// <summary>
/// Specification to find recent coach decisions for an athlete.
/// </summary>
public sealed class RecentCoachDecisionsSpec : Specification<CoachDecision>
{
    public RecentCoachDecisionsSpec(Guid athleteId, int count = 20)
    {
        Query
            .Where(d => d.AthleteId == athleteId)
            .OrderByDescending(d => d.CreatedAt)
            .Take(count);
    }
}
