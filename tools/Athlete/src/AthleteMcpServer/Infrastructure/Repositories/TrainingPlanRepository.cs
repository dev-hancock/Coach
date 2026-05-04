using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.TrainingPlans;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of TrainingPlan repository.
/// </summary>
public sealed class TrainingPlanRepository : EfRepository<TrainingPlan>, ITrainingPlanRepository
{
    public TrainingPlanRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
