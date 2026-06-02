using Coach.Infrastructure.Integrations.Strava.Auth.Contracts;
using Coach.Infrastructure.Integrations.Strava.Contracts;
using Refit;

namespace Coach.Infrastructure.Integrations.Strava;

/// <summary>
/// Refit interface for Strava OAuth endpoints.
/// </summary>
public interface IStravaApi
{
    /// <summary>
    /// Exchange authorization code for access and refresh tokens.
    /// </summary>
    [Post("/oauth/token")]
    Task<StravaTokenResponse> ExchangeCodeAsync(
        [Body(BodySerializationMethod.UrlEncoded)] ExchangeCodeRequest request);

    /// <summary>
    /// Refresh an expired access token.
    /// </summary>
    [Post("/oauth/token")]
    Task<StravaTokenResponse> RefreshTokenAsync(
        [Body(BodySerializationMethod.UrlEncoded)] TokenRefreshRequest request);

    /// <summary>
    /// Revoke access to the application.
    /// </summary>
    [Post("/oauth/deauthorize")]
    Task DeauthorizeAsync(
        [Header("Authorization")] string authorization);

    /// <summary>
    /// Get the authenticated athlete.
    /// </summary>
    [Get("/api/v3/athlete")]
    Task<StravaAthleteResponse> GetAthleteAsync(
        [Header("Authorization")] string authorization);

    /// <summary>
    /// List athlete activities.
    /// </summary>
    [Get("/api/v3/athlete/activities")]
    Task<List<StravaActivityResponse>> GetActivitiesAsync(
        [Header("Authorization")] string authorization,
        [Query] long? after = null,
        [Query("per_page")] int? perPage = null);

    /// <summary>
    /// Get detailed activity by ID.
    /// </summary>
    [Get("/api/v3/activities/{id}")]
    Task<StravaActivityResponse> GetActivityAsync(
        [Header("Authorization")] string authorization,
        long id);
}
