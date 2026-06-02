using Ardalis.Specification;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using Coach.Domain.Training;
using MediatR;

namespace Coach.Application.Features.TrainingPlans.SkipSession;

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
internal sealed class SkipSessionHandler(IRepository<TrainingPlan> plans) : IRequestHandler<SkipSessionRequest>
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
