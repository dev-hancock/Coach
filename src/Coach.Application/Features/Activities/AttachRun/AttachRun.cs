using Ardalis.Specification;
using Coach.Domain.Activities;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using Coach.Domain.Training;
using MediatR;

namespace Coach.Application.Features.Activities.AttachRun;

/// <summary>
/// Command to attach a completed run to a planned session.
/// </summary>
public sealed record AttachRunToSessionRequest(
    Guid RunId,
    Guid TrainingPlanId,
    Guid SessionId) : IRequest;

/// <summary>
/// Handler for attaching a completed run to a planned session.
/// </summary>
internal sealed class AttachRunToSessionHandler(
    IRepository<Activity> runs,
    IRepository<TrainingPlan> plans) : IRequestHandler<AttachRunToSessionRequest>
{
    public async Task Handle(AttachRunToSessionRequest request, CancellationToken cancellationToken)
    {
        // Get the run
        var runSpec = new SingleResultSpecification<Activity>();
        runSpec.Query.Where(r => r.Id == request.RunId);

        var run = await runs.FirstOrDefaultAsync(runSpec, cancellationToken)
            ?? throw new InvalidOperationException($"Completed run with ID {request.RunId} not found.");

        // Get the training plan with sessions
        var plan = await plans.FirstOrDefaultAsync(
            new TrainingPlanByIdWithSessionsSpec(request.TrainingPlanId),
            cancellationToken)
            ?? throw new InvalidOperationException($"Training plan with ID {request.TrainingPlanId} not found.");

        // Link the run to the session in the plan
        plan.LinkCompletedRun(request.SessionId, request.RunId);

        // Update the run to reference the session
        run.AttachToPlannedSession(request.SessionId);

        await plans.UpdateAsync(plan, cancellationToken);
        await runs.UpdateAsync(run, cancellationToken);
        await plans.SaveChangesAsync(cancellationToken);
        await runs.SaveChangesAsync(cancellationToken);
    }
}
