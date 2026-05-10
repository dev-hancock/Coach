using Coach.Domain.Health;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

/// <summary>
/// EF Core implementation of HealthEntry repository.
/// </summary>
public sealed class HealthEntryRepository : EfRepository<HealthEntry>, IHealthEntryRepository
{
    public HealthEntryRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
