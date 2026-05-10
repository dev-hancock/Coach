using Coach.Domain.Gear;
using Coach.Infrastructure.Data;
using Coach.Domain.Repositories;

namespace Coach.Infrastructure.Services.Repositories;

/// <summary>
/// EF Core implementation of Equipment repository.
/// </summary>
public sealed class EquipmentRepository : EfRepository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
