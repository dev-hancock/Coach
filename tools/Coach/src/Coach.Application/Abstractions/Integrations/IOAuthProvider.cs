using Coach.Domain.Integrations;
using ErrorOr;

namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Port for OAuth operations with external activity providers.
/// Implementations handle provider-specific OAuth flows (Strava, Garmin, Coros, etc.).
/// </summary>
public interface IOAuthProvider
{
    /// <summary>
    /// Identifies which integration type this provider handles.
    /// Used for runtime provider resolution via dependency injection.
    /// </summary>
    IntegrationType ProviderType { get; }

    /// <summary>
    /// Exchange OAuth authorization code for access/refresh tokens.
    /// </summary>
    Task<ErrorOr<OAuthTokens>> ExchangeCodeAsync(
        string authorizationCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh an expired access token using refresh token.
    /// </summary>
    Task<ErrorOr<OAuthTokens>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoke access token with the provider.
    /// </summary>
    Task<ErrorOr<Success>> RevokeAccessAsync(
        string accessToken,
        CancellationToken cancellationToken = default);
}
