using Coach.Domain.Integrations;

namespace Coach.Application.Features.Integrations.OAuth;

/// <summary>
/// Response after successfully connecting an integration.
/// </summary>
public sealed record ConnectIntegrationResponse(
    IntegrationType IntegrationType,
    long ExternalAthleteId,
    string AthleteName,
    DateTimeOffset ConnectedAt,
    Guid AthleteId);


public sealed record ConnectOAuthResponse(
    IntegrationType Type,
    long ExternalAthleteId,
    string AthleteName,
    DateTimeOffset ConnectedAt,
    Guid AthleteId);
