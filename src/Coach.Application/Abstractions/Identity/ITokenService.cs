using ErrorOr;

namespace Coach.Application.Abstractions.Identity;

/// <summary>
/// Service for generating and managing JWT tokens.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Create a JWT access token and refresh token for a user.
    /// </summary>
    /// <param name="user">The user to create tokens for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tuple containing the access token, refresh token, and expiration time.</returns>
    Task<ErrorOr<TokenResult>> CreateTokenAsync(
        IUser user,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh an access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>New access and refresh tokens.</returns>
    Task<ErrorOr<TokenResult>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoke a refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to revoke.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success or error.</returns>
    Task<ErrorOr<Success>> RevokeTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result containing JWT tokens and expiration information.
/// </summary>
/// <param name="AccessToken">The JWT access token.</param>
/// <param name="RefreshToken">The refresh token.</param>
/// <param name="ExpiresAt">When the access token expires.</param>
public sealed record TokenResult(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);
