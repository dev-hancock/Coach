using Ardalis.Specification;
using Coach.Domain.Coaching;

namespace Coach.Domain.Specifications;

/// <summary>
///     Specification to find coach decisions by type within a date range.
/// </summary>
public sealed class CoachDecisionsByTypeSpec : Specification<CoachDecision>
{
    public CoachDecisionsByTypeSpec(Guid athleteId, DecisionType decisionType, DateTimeOffset? since = null)
    {
        var startDate = since ?? DateTimeOffset.UtcNow.AddMonths(-3);

        Query
            .Where(d => d.AthleteId == athleteId
                        && d.DecisionType == decisionType
                        && d.CreatedAt >= startDate)
            .OrderByDescending(d => d.CreatedAt);
    }
}