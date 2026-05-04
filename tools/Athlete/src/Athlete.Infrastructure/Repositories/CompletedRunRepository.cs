using Athlete.Infrastructure.Data;
using Athlete.Domain.Activities;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of CompletedRun repository.
/// </summary>
public sealed class CompletedRunRepository : EfRepository<CompletedRun>, ICompletedRunRepository
{
    public CompletedRunRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
