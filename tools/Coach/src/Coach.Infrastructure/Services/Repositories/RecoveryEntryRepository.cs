using Coach.Domain.Recovery;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

public sealed class RecoveryEntryRepository(AthleteDbContext dbContext)
    : EfRepository<RecoveryEntry>(dbContext), IRecoveryEntryRepository
{
}
