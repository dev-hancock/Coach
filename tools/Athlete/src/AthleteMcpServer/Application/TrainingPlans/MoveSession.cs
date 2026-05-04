using MediatR;
using AthleteMcpServer.Domain.TrainingPlans;
using AthleteMcpServer.Domain.Repositories;
using AthleteMcpServer.Domain.Specifications;

namespace AthleteMcpServer.Application.TrainingPlans;

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
internal sealed class MoveSessionHandler(ITrainingPlanRepository plans) : IRequestHandler<MoveSessionRequest>
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
