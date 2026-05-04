using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Injuries;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

public sealed class InjuryEntryRepository(AthleteDbContext dbContext)
    : EfRepository<InjuryEntry>(dbContext), IInjuryEntryRepository
{
}
