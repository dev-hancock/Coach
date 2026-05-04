using Athlete.Infrastructure.Data;
using Athlete.Domain.Recovery;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

public sealed class RecoveryEntryRepository(AthleteDbContext dbContext)
    : EfRepository<RecoveryEntry>(dbContext), IRecoveryEntryRepository
{
}
