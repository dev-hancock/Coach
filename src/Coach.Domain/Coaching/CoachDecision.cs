using Coach.Domain.Common;

namespace Coach.Domain.Coaching;

/// <summary>
///     Coach decision aggregate root representing automated coaching recommendations.
/// </summary>
public sealed class CoachDecision : AggregateRoot
{
    private CoachDecision()
    {
    }

    public CoachDecision(
        Guid athleteId,
        DecisionType decisionType,
        string reason,
        string? recommendation = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);

        AthleteId = athleteId;
        DecisionType = decisionType;
        Reason = reason;
        Recommendation = recommendation;
    }

    public Guid AthleteId { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public DecisionType DecisionType { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public string? Recommendation { get; private set; }
}