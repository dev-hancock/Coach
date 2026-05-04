using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Recovery;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

public sealed class RecoveryEntryRepository(AthleteDbContext dbContext)
    : EfRepository<RecoveryEntry>(dbContext), IRecoveryEntryRepository
{
}
