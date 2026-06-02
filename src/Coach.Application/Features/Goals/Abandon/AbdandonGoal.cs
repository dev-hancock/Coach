using MediatR;
using Ardalis.Specification;
using Coach.Domain.Goals;
using Coach.Domain.Repositories;
using ErrorOr;

namespace Coach.Application.Features.Goals.Abandon;

public sealed record AbandonTrainingGoalRequest(Guid GoalId) : IRequest<ErrorOr<Success>>;

internal sealed class AbandonTrainingGoalHandler(IRepository<TrainingGoal> goals) 
    : IRequestHandler<AbandonTrainingGoalRequest, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        AbandonTrainingGoalRequest request, 
        CancellationToken cancellationToken)
    {
        var spec = new SingleResultSpecification<TrainingGoal>();
        spec.Query.Where(g => g.Id == request.GoalId);

        var goal = await goals.FirstOrDefaultAsync(spec, cancellationToken);

        if (goal is null)
        {
            return Error.NotFound("Goal.NotFound", $"Training goal with ID {request.GoalId} not found.");
        }

        goal.Abandon();

        await goals.UpdateAsync(goal, cancellationToken);
        await goals.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}

