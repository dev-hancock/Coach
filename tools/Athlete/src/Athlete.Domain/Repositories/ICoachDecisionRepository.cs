using Athlete.Domain.Coaching;

namespace Athlete.Domain.Repositories;

/// <summary>
/// Repository interface for CoachDecision entity.
/// </summary>
public interface ICoachDecisionRepository : IReadRepository<CoachDecision>
{
    // CoachDecision is append-only (immutable), so only read operations
}
