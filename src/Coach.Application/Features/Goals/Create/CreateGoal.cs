using Coach.Domain.Goals;
using Coach.Domain.Repositories;
using MediatR;
using ErrorOr;

namespace Coach.Application.Features.Goals.Create;

public sealed record CreateTrainingGoalRequest(
    Guid AthleteId,
    RaceDistance Distance,
    DateOnly TargetDate,
    TimeSpan? TargetTime = null,
    string? Description = null) : IRequest<ErrorOr<CreateTrainingGoalResponse>>;

public sealed record CreateTrainingGoalResponse(Guid GoalId);

internal sealed class CreateTrainingGoalHandler(IRepository<TrainingGoal> goals) 
    : IRequestHandler<CreateTrainingGoalRequest, ErrorOr<CreateTrainingGoalResponse>>
{
    public async Task<ErrorOr<CreateTrainingGoalResponse>> Handle(
        CreateTrainingGoalRequest request, 
        CancellationToken cancellationToken)
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

