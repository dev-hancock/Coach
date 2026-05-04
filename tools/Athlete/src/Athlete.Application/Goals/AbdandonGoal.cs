using MediatR;
using Athlete.Domain.Goals;
using Athlete.Domain.Repositories;
using Ardalis.Specification;

namespace Athlete.Application.Goals;

/// <summary>
/// Command to abandon a training goal.
/// </summary>
public sealed record AbandonTrainingGoalRequest(Guid GoalId) : IRequest;

/// <summary>
/// Handler for abandoning a training goal.
/// </summary>
internal sealed class AbandonTrainingGoalHandler(ITrainingGoalRepository goals) : IRequestHandler<AbandonTrainingGoalRequest>
{
    public async Task Handle(AbandonTrainingGoalRequest request, CancellationToken cancellationToken)
    {
        var spec = new SingleResultSpecification<TrainingGoal>();
        spec.Query.Where(g => g.Id == request.GoalId);

        var goal = await goals.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new InvalidOperationException($"Training goal with ID {request.GoalId} not found.");

        goal.Abandon();

        await goals.UpdateAsync(goal, cancellationToken);
        await goals.SaveChangesAsync(cancellationToken);
    }
}
