namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Generic OAuth token data returned by any provider.
/// Immutable record prevents accidental mutation.
/// </summary>
public sealed record OAuthTokens(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt);
