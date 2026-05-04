using Athlete.Infrastructure.Data;
using Athlete.Domain.Goals;
using Athlete.Domain.Repositories;

namespace Athlete.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of TrainingGoal repository.
/// </summary>
public sealed class TrainingGoalRepository : EfRepository<TrainingGoal>, ITrainingGoalRepository
{
    public TrainingGoalRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
