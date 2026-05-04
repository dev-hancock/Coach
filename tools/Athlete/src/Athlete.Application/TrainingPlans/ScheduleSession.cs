using MediatR;
using Athlete.Domain.TrainingPlans;
using Athlete.Domain.Repositories;
using Athlete.Domain.Specifications;

namespace Athlete.Application.TrainingPlans;

/// <summary>
/// Command to schedule a new session in a training plan.
/// </summary>
public sealed record ScheduleSessionRequest(
    Guid TrainingPlanId,
    DateOnly Date,
    SessionType SessionType,
    SessionIntensity Intensity,
    decimal? TargetDistanceKm = null,
    TimeSpan? TargetDuration = null,
    TimeSpan? TargetPacePerKm = null,
    string? Description = null) : IRequest<ScheduleSessionResponse>;

/// <summary>
/// Response containing the scheduled session's ID.
/// </summary>
public sealed record ScheduleSessionResponse(Guid SessionId);

/// <summary>
/// Handler for scheduling a session.
/// </summary>
internal sealed class ScheduleSessionHandler(ITrainingPlanRepository plans) : IRequestHandler<ScheduleSessionRequest, ScheduleSessionResponse>
{
    public async Task<ScheduleSessionResponse> Handle(ScheduleSessionRequest request, CancellationToken cancellationToken)
    {
        var plan = await plans.FirstOrDefaultAsync(
            new TrainingPlanByIdWithSessionsSpec(request.TrainingPlanId),
            cancellationToken)
            ?? throw new InvalidOperationException($"Training plan with ID {request.TrainingPlanId} not found.");

        var session = plan.ScheduleSession(
            request.Date,
            request.SessionType,
            request.Intensity,
            request.TargetDistanceKm,
            request.TargetDuration,
            request.TargetPacePerKm,
            request.Description);

        await plans.UpdateAsync(plan, cancellationToken);
        await plans.SaveChangesAsync(cancellationToken);

        return new ScheduleSessionResponse(session.Id);
    }
}
