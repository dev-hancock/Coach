using Ardalis.Specification.EntityFrameworkCore;
using Athlete.Infrastructure.Data;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of read-only repository base.
/// Uses Ardalis.Specification for query building.
/// </summary>
public class EfReadRepository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public EfReadRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
