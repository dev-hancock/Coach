using MediatR;
using Athlete.Domain.Coaching;
using Athlete.Domain.Activities;
using Athlete.Domain.TrainingPlans;
using Athlete.Domain.Health;
using Athlete.Domain.Repositories;
using Athlete.Domain.Specifications;

namespace Athlete.Application.Coaching;

/// <summary>
/// Command to generate a weekly review for an athlete.
/// </summary>
public sealed record GenerateWeeklyReviewRequest(
    Guid AthleteId,
    DateOnly WeekStartDate) : IRequest<GenerateWeeklyReviewResponse>;

/// <summary>
/// Response containing the weekly review.
/// </summary>
public sealed record GenerateWeeklyReviewResponse(
    Guid DecisionId,
    string Review,
    IReadOnlyList<string> KeyObservations,
    IReadOnlyList<string> Recommendations);

/// <summary>
/// Handler for generating a weekly review.
/// </summary>
internal sealed class GenerateWeeklyReviewHandler(
    IReadRepository<CompletedRun> runs,
    IReadRepository<HealthEntry> health,
    IRepository<CoachDecision> decisions) : IRequestHandler<GenerateWeeklyReviewRequest, GenerateWeeklyReviewResponse>
{
    public async Task<GenerateWeeklyReviewResponse> Handle(GenerateWeeklyReviewRequest request, CancellationToken cancellationToken)
    {
        var weekStart = request.WeekStartDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var weekEnd = request.WeekStartDate.AddDays(7).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

        // Get runs for the week
        var runsList = await runs.ListAsync(
            new CompletedRunsByDateRangeSpec(request.AthleteId, weekStart, weekEnd),
            cancellationToken);

        // Get health entries for the week
        var healthEntries = await health.ListAsync(
            new RecentHealthEntriesSpec(request.AthleteId, 7),
            cancellationToken);

        // Analyze the data
        var observations = new List<string>();
        var recommendations = new List<string>();

        // Calculate metrics
        var totalDistance = runsList.Sum(r => r.DistanceKm);
        var totalRuns = runsList.Count;
        var avgPace = runsList.Any() 
            ? TimeSpan.FromSeconds(runsList.Average(r => r.AveragePacePerKm?.TotalSeconds ?? 0))
            : TimeSpan.Zero;

        observations.Add($"Completed {totalRuns} runs totaling {totalDistance:F1} km");

        if (avgPace > TimeSpan.Zero)
        {
            observations.Add($"Average pace: {avgPace:mm\\:ss} per km");
        }

        // Check for injuries or high severity issues
        var highSeverityIssues = healthEntries
            .Where(e => e.Severity >= Severity.High)
            .ToList();

        if (highSeverityIssues.Any())
        {
            observations.Add($"âš ï¸ {highSeverityIssues.Count} high-severity health concerns reported");
            recommendations.Add("Consider reducing training volume and consulting with a healthcare professional");
        }

        // Check for rest
        if (totalRuns >= 6)
        {
            recommendations.Add("Ensure adequate rest - consider including at least one complete rest day");
        }

        // Build review text
        var review = $"Week of {request.WeekStartDate:yyyy-MM-dd}: " +
                    $"{totalRuns} runs, {totalDistance:F1} km total. " +
                    (highSeverityIssues.Any() 
                        ? "Notable health concerns require attention." 
                        : "No major health concerns reported.");

        var decision = new CoachDecision(
            request.AthleteId,
            CoachDecisionType.WeeklyReview,
            review,
            string.Join("; ", recommendations));

        await decisions.AddAsync(decision, cancellationToken);
        await decisions.SaveChangesAsync(cancellationToken);

        return new GenerateWeeklyReviewResponse(
            decision.Id,
            review,
            observations,
            recommendations);
    }
}
