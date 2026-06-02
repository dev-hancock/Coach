namespace Coach.Application.Abstractions.Integrations;

public sealed record Connection(
    long ExternalId,
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt,
    DateTimeOffset ConnectedAt,
    DateTimeOffset? LastSyncedAt);
