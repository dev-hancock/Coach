using MediatR;
using AthleteMcpServer.Domain.Goals;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Application.Goals;

/// <summary>
/// Command to create a new training goal.
/// </summary>
public sealed record CreateTrainingGoalRequest(
    Guid AthleteId,
    RaceDistance Distance,
    DateOnly TargetDate,
    TimeSpan? TargetTime = null,
    string? Description = null) : IRequest<CreateTrainingGoalResponse>;

/// <summary>
/// Response containing the created goal's ID.
/// </summary>
public sealed record CreateTrainingGoalResponse(Guid GoalId);

/// <summary>
/// Handler for creating a training goal.
/// </summary>
internal sealed class CreateTrainingGoalHandler(ITrainingGoalRepository goals) : IRequestHandler<CreateTrainingGoalRequest, CreateTrainingGoalResponse>
{
    public async Task<CreateTrainingGoalResponse> Handle(CreateTrainingGoalRequest request, CancellationToken cancellationToken)
    {
        var goal = new TrainingGoal(
            request.AthleteId,
            request.Distance,
            request.TargetDate,
            request.TargetTime,
            request.Description);

        await goals.AddAsync(goal, cancellationToken);
        await goals.SaveChangesAsync(cancellationToken);

        return new CreateTrainingGoalResponse(goal.Id);
    }
}
