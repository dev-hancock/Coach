using Athlete.Infrastructure.Data;
using Athlete.Domain.Coaching;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of CoachDecision repository.
/// Read-only as CoachDecisions are immutable/append-only.
/// </summary>
public sealed class CoachDecisionRepository : EfReadRepository<CoachDecision>, ICoachDecisionRepository
{
    public CoachDecisionRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
