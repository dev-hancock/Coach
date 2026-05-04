using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Coaching;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

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
