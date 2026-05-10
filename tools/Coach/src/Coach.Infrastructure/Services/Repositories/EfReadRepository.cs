using Ardalis.Specification.EntityFrameworkCore;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

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
