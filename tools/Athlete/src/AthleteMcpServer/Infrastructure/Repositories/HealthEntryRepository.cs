using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Health;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of HealthEntry repository.
/// </summary>
public sealed class HealthEntryRepository : EfRepository<HealthEntry>, IHealthEntryRepository
{
    public HealthEntryRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
