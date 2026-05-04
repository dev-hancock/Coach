using Athlete.Infrastructure.Data;
using Athlete.Domain.Injuries;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

public sealed class InjuryEntryRepository(AthleteDbContext dbContext)
    : EfRepository<InjuryEntry>(dbContext), IInjuryEntryRepository
{
}
