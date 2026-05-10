using Coach.Domain.Coaching;

namespace Coach.Domain.Repositories;

/// <summary>
///     Repository interface for CoachDecision entity.
/// </summary>
public interface ICoachDecisionRepository : IReadRepository<CoachDecision>
{
    // CoachDecision is append-only (immutable), so only read operations
}