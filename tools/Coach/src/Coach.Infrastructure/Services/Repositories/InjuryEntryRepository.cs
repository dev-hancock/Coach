using Coach.Domain.Injuries;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

public sealed class InjuryEntryRepository(AthleteDbContext dbContext)
    : EfRepository<InjuryEntry>(dbContext), IInjuryEntryRepository
{
}
