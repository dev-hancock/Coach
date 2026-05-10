using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;
using Coach.Domain.Athletes;

namespace Coach.Infrastructure.Services.Repositories;

/// <summary>
/// EF Core implementation of Athlete repository.
/// </summary>
public sealed class AthleteRepository : EfRepository<Athlete>, IAthleteRepository
{
    public AthleteRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }

    // Add custom Athlete-specific methods here if needed
}



