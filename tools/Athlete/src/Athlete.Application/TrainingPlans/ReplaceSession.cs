using MediatR;
using Athlete.Domain.TrainingPlans;
using Athlete.Domain.Repositories;
using Athlete.Domain.Specifications;

namespace Athlete.Application.TrainingPlans;

/// <summary>
/// Command to replace a planned session with different parameters.
/// </summary>
public sealed record ReplaceSessionRequest(
    Guid TrainingPlanId,
    Guid SessionId,
    SessionType SessionType,
    SessionIntensity Intensity,
    decimal? TargetDistanceKm,
    TimeSpan? TargetDuration,
    TimeSpan? TargetPacePerKm,
    string? Description) : IRequest;

/// <summary>
/// Handler for replacing a session.
/// </summary>
internal sealed class ReplaceSessionHandler(ITrainingPlanRepository plans) : IRequestHandler<ReplaceSessionRequest>
{
    public async Task Handle(ReplaceSessionRequest request, CancellationToken cancellationToken)
    {
        var plan = await plans.FirstOrDefaultAsync(
            new TrainingPlanByIdWithSessionsSpec(request.TrainingPlanId),
            cancellationToken)
            ?? throw new InvalidOperationException($"Training plan with ID {request.TrainingPlanId} not found.");

        plan.ReplaceSession(
            request.SessionId,
            request.SessionType,
            request.Intensity,
            request.TargetDistanceKm,
            request.TargetDuration,
            request.TargetPacePerKm,
            request.Description);

        await plans.UpdateAsync(plan, cancellationToken);
        await plans.SaveChangesAsync(cancellationToken);
    }
}
