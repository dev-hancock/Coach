using Athlete.Infrastructure.Data;
using Athlete.Domain.Fatigue;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

public sealed class FatigueEntryRepository(AthleteDbContext dbContext)
    : EfRepository<FatigueEntry>(dbContext), IFatigueEntryRepository
{
}
