namespace Athlete.Domain.Coaching;

public sealed class CoachDecision
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid AthleteId { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public CoachDecisionType DecisionType { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public string? Recommendation { get; private set; }

    private CoachDecision()
    {
    }

    public CoachDecision(
        Guid athleteId,
        CoachDecisionType decisionType,
        string reason,
        string? recommendation = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        AthleteId = athleteId;
        DecisionType = decisionType;
        Reason = reason;
        Recommendation = recommendation;
    }
}
