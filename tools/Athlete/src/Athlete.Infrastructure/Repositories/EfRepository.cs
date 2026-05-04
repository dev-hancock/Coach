using Ardalis.Specification.EntityFrameworkCore;
using Athlete.Infrastructure.Data;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of repository base with full CRUD operations.
/// Uses Ardalis.Specification for query building.
/// </summary>
public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public EfRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
