using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Activities;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of CompletedRun repository.
/// </summary>
public sealed class CompletedRunRepository : EfRepository<CompletedRun>, ICompletedRunRepository
{
    public CompletedRunRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
