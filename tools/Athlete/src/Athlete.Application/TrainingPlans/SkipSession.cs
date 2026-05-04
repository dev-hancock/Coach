using MediatR;
using Athlete.Domain.TrainingPlans;
using Athlete.Domain.Repositories;
using Athlete.Domain.Specifications;

namespace Athlete.Application.TrainingPlans;

/// <summary>
/// Command to skip a planned session.
/// </summary>
public sealed record SkipSessionRequest(
    Guid TrainingPlanId,
    Guid SessionId,
    string? Reason = null) : IRequest;

/// <summary>
/// Handler for skipping a session.
/// </summary>
internal sealed class SkipSessionHandler(ITrainingPlanRepository plans) : IRequestHandler<SkipSessionRequest>
{
    public async Task Handle(SkipSessionRequest request, CancellationToken cancellationToken)
    {
        var plan = await plans.FirstOrDefaultAsync(
            new TrainingPlanByIdWithSessionsSpec(request.TrainingPlanId),
            cancellationToken)
            ?? throw new InvalidOperationException($"Training plan with ID {request.TrainingPlanId} not found.");

        plan.SkipSession(request.SessionId, request.Reason);

        await plans.UpdateAsync(plan, cancellationToken);
        await plans.SaveChangesAsync(cancellationToken);
    }
}
