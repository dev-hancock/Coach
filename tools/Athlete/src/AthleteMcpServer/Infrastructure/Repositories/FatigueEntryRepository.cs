using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Fatigue;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

public sealed class FatigueEntryRepository(AthleteDbContext dbContext)
    : EfRepository<FatigueEntry>(dbContext), IFatigueEntryRepository
{
}
