using Coach.Domain.Entities;
using Coach.Domain.Repositories;
using Coach.Infrastructure.Data;

namespace Coach.Infrastructure.Services.Repositories;

public class WorkoutPlanRepository : EfRepository<WorkoutPlan>, IWorkoutPlanRepository
{
    public WorkoutPlanRepository(AthleteDbContext dbContext) : base(dbContext)
    {
    }
}
