namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Result of a successful integration connection.
/// </summary>
public sealed record IntegrationConnectionResult(
    Guid AthleteId,
    string ExternalId,
    DateTimeOffset ConnectedAt);
