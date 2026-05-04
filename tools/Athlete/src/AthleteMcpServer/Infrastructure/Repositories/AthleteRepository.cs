using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Athletes;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

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
