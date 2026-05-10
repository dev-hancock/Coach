using Coach.Domain.Goals;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

/// <summary>
/// EF Core implementation of TrainingGoal repository.
/// </summary>
public sealed class TrainingGoalRepository : EfRepository<TrainingGoal>, ITrainingGoalRepository
{
    public TrainingGoalRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
