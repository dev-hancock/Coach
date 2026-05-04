using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Equipment;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of Shoe repository.
/// </summary>
public sealed class ShoeRepository : EfRepository<Shoe>, IShoeRepository
{
    public ShoeRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
