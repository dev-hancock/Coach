using Coach.Domain.Coaching;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

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
