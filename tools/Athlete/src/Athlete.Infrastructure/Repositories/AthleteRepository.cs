using Athlete.Infrastructure.Data;
using Athlete.Domain.Athletes;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of Athlete repository.
/// </summary>
public sealed class AthleteRepository : EfReadRepository<Athlete>, IAthleteRepository
{
    public AthleteRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }

    // Add custom Athlete-specific methods here if needed
}
