using Coach.Domain.Repositories;
using Coach.Domain.Training;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

/// <summary>
/// EF Core implementation of TrainingPlan repository.
/// </summary>
public sealed class TrainingPlanRepository : EfRepository<TrainingPlan>, ITrainingPlanRepository
{
    public TrainingPlanRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
