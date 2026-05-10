using Coach.Domain.Activities;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

/// <summary>
/// EF Core implementation of Activity repository.
/// </summary>
public sealed class CompletedRunRepository : EfRepository<Activity>, ICompletedRunRepository
{
    public CompletedRunRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
