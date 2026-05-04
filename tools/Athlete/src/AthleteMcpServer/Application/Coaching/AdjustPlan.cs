using MediatR;
using AthleteMcpServer.Domain.Coaching;
using AthleteMcpServer.Domain.Activities;
using AthleteMcpServer.Domain.TrainingPlans;
using AthleteMcpServer.Domain.Repositories;
using AthleteMcpServer.Domain.Specifications;
using Ardalis.Specification;

namespace AthleteMcpServer.Application.Coaching;

/// <summary>
/// Command to adjust training plan after analyzing a completed run.
/// </summary>
public sealed record AdjustPlanAfterRunRequest(
    Guid AthleteId,
    Guid RunId,
    Guid TrainingPlanId) : IRequest<AdjustPlanAfterRunResponse>;

/// <summary>
/// Response containing adjustment recommendations.
/// </summary>
public sealed record AdjustPlanAfterRunResponse(
    Guid? DecisionId,
    bool AdjustmentNeeded,
    string Analysis,
    IReadOnlyList<string> SuggestedChanges);

/// <summary>
/// Handler for adjusting a plan after a run.
/// </summary>
internal sealed class AdjustPlanAfterRunHandler(
    IReadRepository<CompletedRun> runs,
    IReadRepository<TrainingPlan> plans,
    IRepository<CoachDecision> decisions) : IRequestHandler<AdjustPlanAfterRunRequest, AdjustPlanAfterRunResponse>
{
    public async Task<AdjustPlanAfterRunResponse> Handle(AdjustPlanAfterRunRequest request, CancellationToken cancellationToken)
    {
        // Get the run
        var runSpec = new SingleResultSpecification<CompletedRun>();
        runSpec.Query.Where(r => r.Id == request.RunId);

        var run = await runs.FirstOrDefaultAsync(runSpec, cancellationToken)
            ?? throw new InvalidOperationException($"Run with ID {request.RunId} not found.");

        // Get the plan
        var plan = await plans.FirstOrDefaultAsync(
            new TrainingPlanByIdWithSessionsSpec(request.TrainingPlanId),
            cancellationToken)
            ?? throw new InvalidOperationException($"Training plan with ID {request.TrainingPlanId} not found.");

        var suggestedChanges = new List<string>();
        var adjustmentNeeded = false;

        // Analyze RPE
        if (run.RatePerceivedExertion.HasValue)
        {
            if (run.RatePerceivedExertion >= 9)
            {
                adjustmentNeeded = true;
                suggestedChanges.Add("High RPE detected - consider adding recovery day");
                suggestedChanges.Add("Reduce intensity of next workout");
            }
            else if (run.RatePerceivedExertion <= 3 && run.DistanceKm > 10)
            {
                suggestedChanges.Add("Low RPE on long run suggests good fitness - consider progressive overload");
            }
        }

        // Analyze pace vs distance
        var paceSeconds = run.AveragePacePerKm?.TotalSeconds ?? 0;
        if (run.DistanceKm > 20 && paceSeconds > 0 && paceSeconds < 300) // < 5:00/km
        {
            adjustmentNeeded = true;
            suggestedChanges.Add("Fast pace on long run may indicate overexertion - monitor recovery");
        }

        var analysis = $"Run analysis: {run.DistanceKm:F1} km at {run.AveragePacePerKm:mm\\:ss}/km" +
                      (run.RatePerceivedExertion.HasValue ? $", RPE {run.RatePerceivedExertion}" : "");

        CoachDecision? decision = null;
        if (adjustmentNeeded)
        {
            decision = new CoachDecision(
                request.AthleteId,
                CoachDecisionType.PlanAdjustment,
                analysis,
                string.Join("; ", suggestedChanges));

            await decisions.AddAsync(decision, cancellationToken);
            await decisions.SaveChangesAsync(cancellationToken);
        }

        return new AdjustPlanAfterRunResponse(
            decision?.Id,
            adjustmentNeeded,
            analysis,
            suggestedChanges);
    }
}
