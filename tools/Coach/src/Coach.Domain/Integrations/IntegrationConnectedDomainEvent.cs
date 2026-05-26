using Coach.Domain.Common;

namespace Coach.Domain.Integrations;

/// <summary>
/// Domain event raised when an integration is connected to an athlete.
/// This triggers background activity sync and other integration setup tasks.
/// </summary>
public sealed record IntegrationConnectedDomainEvent : DomainEvent
{
    public required Guid UserId { get; init; }
    public required Guid AthleteId { get; init; }
    public required IntegrationType IntegrationType { get; init; }
    public required string ExternalAthleteId { get; init; }
    public required DateTimeOffset ConnectedAt { get; init; }
}
