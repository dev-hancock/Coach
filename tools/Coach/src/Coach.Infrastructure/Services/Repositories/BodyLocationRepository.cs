using Coach.Domain.Injuries;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

/// <summary>
/// EF Core implementation of BodyLocation repository.
/// </summary>
public sealed class BodyLocationRepository : EfRepository<BodyLocation>, IBodyLocationRepository
{
    public BodyLocationRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
