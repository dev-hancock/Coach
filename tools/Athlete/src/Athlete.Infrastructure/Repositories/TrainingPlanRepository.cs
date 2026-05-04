using Athlete.Infrastructure.Data;
using Athlete.Domain.TrainingPlans;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of TrainingPlan repository.
/// </summary>
public sealed class TrainingPlanRepository : EfRepository<TrainingPlan>, ITrainingPlanRepository
{
    public TrainingPlanRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
