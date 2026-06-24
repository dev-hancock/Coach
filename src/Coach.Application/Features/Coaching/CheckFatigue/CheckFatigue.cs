using Ardalis.Specification;
using Coach.Domain.Activities;
using Coach.Domain.Coaching;
using Coach.Domain.Health;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using MediatR;

namespace Coach.Application.Features.Coaching.CheckFatigue;

/// <summary>
/// Command to check for fatigue risk based on recent training and readiness.
/// </summary>
public sealed record CheckFatigueRiskRequest(
    Guid AthleteId,
    int DaysToAnalyze = 14) : IRequest<CheckFatigueRiskResponse>;

/// <summary>
/// Response containing fatigue risk assessment.
/// </summary>
public sealed record CheckFatigueRiskResponse(
    Guid? DecisionId,
    FatigueRiskLevel RiskLevel,
    string Assessment,
    IReadOnlyList<string> RiskFactors,
    IReadOnlyList<string> Recommendations);

/// <summary>
/// Fatigue risk levels.
/// </summary>
public enum FatigueRiskLevel
{
    Low,
    Moderate,
    High,
    Critical
}

/// <summary>
/// Handler for checking fatigue risk.
/// </summary>
internal sealed class CheckFatigueRiskHandler(
    IRepositoryBase<Activity> runs,
    IRepositoryBase<HealthEntry> health,
    IRepository<CoachDecision> decisions) : IRequestHandler<CheckFatigueRiskRequest, CheckFatigueRiskResponse>
{
    public async Task<CheckFatigueRiskResponse> Handle(CheckFatigueRiskRequest request, CancellationToken cancellationToken)
    {
        var startDate = DateTimeOffset.UtcNow.AddDays(-request.DaysToAnalyze);

        // Get recent runs
        var runsList = await runs.ListAsync(
            new RecentCompletedRunsSpec(request.AthleteId, 20),
            cancellationToken);

        var recentRuns = runsList.Where(r => r.StartedAt >= startDate).ToList();

        // Get recent health entries
        var healthEntries = await health.ListAsync(
            new RecentHealthEntriesSpec(request.AthleteId, request.DaysToAnalyze),
            cancellationToken);

        var riskFactors = new List<string>();
        var recommendations = new List<string>();
        var riskScore = 0;

        // Analyze training volume
        var totalDistance = recentRuns.Sum(r => r.DistanceKm);
        var avgDailyDistance = totalDistance / request.DaysToAnalyze;

        if (avgDailyDistance > 10)
        {
            riskScore += 2;
            riskFactors.Add($"High average daily volume: {avgDailyDistance:F1} km/day");
        }

        // Check for consecutive high-effort days
        var consecutiveHardDays = 0;
        var maxConsecutiveHardDays = 0;

        foreach (var run in recentRuns.OrderBy(r => r.StartedAt))
        {
            if (run.RatePerceivedExertion >= 7)
            {
                consecutiveHardDays++;
                maxConsecutiveHardDays = Math.Max(maxConsecutiveHardDays, consecutiveHardDays);
            }
            else
            {
                consecutiveHardDays = 0;
            }
        }

        if (maxConsecutiveHardDays >= 3)
        {
            riskScore += 3;
            riskFactors.Add($"{maxConsecutiveHardDays} consecutive high-effort days");
            recommendations.Add("Include more recovery runs between hard efforts");
        }

        // Check for health issues
        var fatigueEntries = healthEntries
            .Where(e => e.EntryType == HealthEntryType.Fatigue)
            .ToList();

        if (fatigueEntries.Any(e => e.Severity >= Severity.High))
        {
            riskScore += 4;
            riskFactors.Add("High-severity fatigue reported");
            recommendations.Add("Urgent: Consider taking 2-3 days complete rest");
        }
        else if (fatigueEntries.Count >= 3)
        {
            riskScore += 2;
            riskFactors.Add($"{fatigueEntries.Count} fatigue entries in {request.DaysToAnalyze} days");
        }

        // Check for pain/injury entries
        var painEntries = healthEntries
            .Where(e => e.EntryType == HealthEntryType.Pain)
            .ToList();

        if (painEntries.Any())
        {
            riskScore += 3;
            riskFactors.Add($"{painEntries.Count} pain concerns");
            recommendations.Add("Address pain before increasing training load");
        }

        // Determine risk level
        var riskLevel = riskScore switch
        {
            >= 7 => FatigueRiskLevel.Critical,
            >= 5 => FatigueRiskLevel.High,
            >= 3 => FatigueRiskLevel.Moderate,
            _ => FatigueRiskLevel.Low
        };

        if (riskLevel == FatigueRiskLevel.Low)
        {
            recommendations.Add("Training load appears sustainable - continue monitoring");
        }
        else if (riskLevel >= FatigueRiskLevel.High)
        {
            recommendations.Add("Reduce training volume by 30-50% this week");
            recommendations.Add("Prioritize sleep and recovery activities");
        }

        var assessment = $"Fatigue risk: {riskLevel}. " +
                        $"Risk score: {riskScore}/10. " +
                        $"Based on {recentRuns.Count} runs and {healthEntries.Count} health entries.";

        CoachDecision? decision = null;
        if (riskLevel >= FatigueRiskLevel.High)
        {
            decision = new CoachDecision(
                request.AthleteId,
                riskLevel == FatigueRiskLevel.Critical 
                    ? DecisionType.InjuryRisk 
                    : DecisionType.FatigueWarning,
                assessment,
                string.Join("; ", recommendations));

            await decisions.AddAsync(decision, cancellationToken);
            await decisions.SaveChangesAsync(cancellationToken);
        }

        return new CheckFatigueRiskResponse(
            decision?.Id,
            riskLevel,
            assessment,
            riskFactors,
            recommendations);
    }
}
