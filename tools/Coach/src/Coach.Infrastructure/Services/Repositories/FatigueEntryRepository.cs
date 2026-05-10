using Coach.Domain.Fatigue;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

public sealed class FatigueEntryRepository(AthleteDbContext dbContext)
    : EfRepository<FatigueEntry>(dbContext), IFatigueEntryRepository
{
}
