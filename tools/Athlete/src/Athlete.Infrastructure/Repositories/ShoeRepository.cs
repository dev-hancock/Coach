using Athlete.Infrastructure.Data;
using Athlete.Domain.Equipment;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of Shoe repository.
/// </summary>
public sealed class ShoeRepository : EfRepository<Shoe>, IShoeRepository
{
    public ShoeRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
