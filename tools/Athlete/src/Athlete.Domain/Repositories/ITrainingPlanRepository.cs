using Athlete.Domain.TrainingPlans;

namespace Athlete.Domain.Repositories;

/// <summary>
/// Repository interface for TrainingPlan aggregate root.
/// </summary>
public interface ITrainingPlanRepository : IRepository<TrainingPlan>
{
}
