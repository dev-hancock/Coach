using MediatR;
using AthleteMcpServer.Domain.Goals;
using AthleteMcpServer.Domain.Repositories;
using Ardalis.Specification;

namespace AthleteMcpServer.Application.Goals;

/// <summary>
/// Command to mark a training goal as completed.
/// </summary>
public sealed record CompleteTrainingGoalRequest(Guid GoalId) : IRequest;

/// <summary>
/// Handler for completing a training goal.
/// </summary>
internal sealed class CompleteTrainingGoalHandler(ITrainingGoalRepository goals) : IRequestHandler<CompleteTrainingGoalRequest>
{
    public async Task Handle(CompleteTrainingGoalRequest request, CancellationToken cancellationToken)
    {
        var spec = new SingleResultSpecification<TrainingGoal>();
        spec.Query.Where(g => g.Id == request.GoalId);

        var goal = await goals.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new InvalidOperationException($"Training goal with ID {request.GoalId} not found.");

        goal.Complete();

        await goals.UpdateAsync(goal, cancellationToken);
        await goals.SaveChangesAsync(cancellationToken);
    }
}
