using Coach.Domain.Common;

namespace Coach.Domain.Integrations;

/// <summary>
/// Domain event raised when a user successfully connects their Strava account.
/// Triggers background activity sync workflow.
/// </summary>
public sealed record StravaConnectedDomainEvent : DomainEvent
{
    public required Guid UserId { get; init; }

    public required Guid AthleteId { get; init; }

    public required long StravaAthleteId { get; init; }

    public required DateTime ConnectedAt { get; init; }
}
