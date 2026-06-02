using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using Coach.Domain.Training;
using MediatR;

namespace Coach.Application.Features.TrainingPlans.MoveSession;

/// <summary>
/// Command to move a planned session to a new date.
/// </summary>
public sealed record MoveSessionRequest(
    Guid TrainingPlanId,
    Guid SessionId,
    DateOnly NewDate) : IRequest;

/// <summary>
/// Handler for moving a session.
/// </summary>
internal sealed class MoveSessionHandler(IRepository<TrainingPlan> plans) : IRequestHandler<MoveSessionRequest>
{
    public async Task Handle(MoveSessionRequest request, CancellationToken cancellationToken)
    {
        var plan = await plans.FirstOrDefaultAsync(
            new TrainingPlanByIdWithSessionsSpec(request.TrainingPlanId),
            cancellationToken)
            ?? throw new InvalidOperationException($"Training plan with ID {request.TrainingPlanId} not found.");

        plan.MoveSession(request.SessionId, request.NewDate);

        await plans.UpdateAsync(plan, cancellationToken);
        await plans.SaveChangesAsync(cancellationToken);
    }
}
