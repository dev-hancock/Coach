using Ardalis.Specification.EntityFrameworkCore;
using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

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
