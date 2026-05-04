using Athlete.Infrastructure.Data;
using Athlete.Domain.Health;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of HealthEntry repository.
/// </summary>
public sealed class HealthEntryRepository : EfRepository<HealthEntry>, IHealthEntryRepository
{
    public HealthEntryRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
