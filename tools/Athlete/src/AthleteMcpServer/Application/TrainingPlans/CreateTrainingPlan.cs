using MediatR;
using AthleteMcpServer.Domain.TrainingPlans;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Application.TrainingPlans;

/// <summary>
/// Command to create a new training plan.
/// </summary>
public sealed record CreateTrainingPlanRequest(
    Guid AthleteId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int TrainingDaysPerWeek,
    decimal TargetWeeklyDistanceKm,
    Guid? GoalId = null) : IRequest<CreateTrainingPlanResponse>;

/// <summary>
/// Response containing the created training plan's ID.
/// </summary>
public sealed record CreateTrainingPlanResponse(Guid TrainingPlanId);

/// <summary>
/// Handler for creating a training plan.
/// </summary>
internal sealed class CreateTrainingPlanHandler(ITrainingPlanRepository plans) : IRequestHandler<CreateTrainingPlanRequest, CreateTrainingPlanResponse>
{
    public async Task<CreateTrainingPlanResponse> Handle(CreateTrainingPlanRequest request, CancellationToken cancellationToken)
    {
        var plan = new TrainingPlan(
            request.AthleteId,
            request.Name,
            request.StartDate,
            request.EndDate,
            request.TrainingDaysPerWeek,
            request.TargetWeeklyDistanceKm,
            request.GoalId);

        await plans.AddAsync(plan, cancellationToken);
        await plans.SaveChangesAsync(cancellationToken);

        return new CreateTrainingPlanResponse(plan.Id);
    }
}
