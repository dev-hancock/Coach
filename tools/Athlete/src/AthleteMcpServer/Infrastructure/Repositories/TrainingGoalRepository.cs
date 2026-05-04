using AthleteMcpServer.Data;
using AthleteMcpServer.Domain.Goals;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of TrainingGoal repository.
/// </summary>
public sealed class TrainingGoalRepository : EfRepository<TrainingGoal>, ITrainingGoalRepository
{
    public TrainingGoalRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
