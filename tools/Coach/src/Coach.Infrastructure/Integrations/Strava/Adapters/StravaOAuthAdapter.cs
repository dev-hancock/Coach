using Coach.Application.Abstractions.Integrations;
using Coach.Domain.Integrations;
using Coach.Infrastructure.Integrations.Strava.Auth.Contracts;
using ErrorOr;

namespace Coach.Infrastructure.Integrations.Strava.Adapters;

/// <summary>
/// Strava implementation of OAuth provider port.
/// Maps Strava-specific OAuth flow to generic OAuth abstraction.
/// </summary>
internal sealed class StravaOAuthAdapter(
    IStravaApi api,
    StravaSettings settings) : IOAuthProvider
{
    public IntegrationType ProviderType => IntegrationType.Strava;

    public async Task<ErrorOr<OAuthTokens>> ExchangeCodeAsync(
        string authorizationCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new ExchangeCodeRequest(
                ClientId: settings.ClientId,
                ClientSecret: settings.ClientSecret,
                Code: authorizationCode,
                GrantType: "authorization_code");

            var response = await api.ExchangeCodeAsync(request);

            return new OAuthTokens(
                AccessToken: response.AccessToken,
                RefreshToken: response.RefreshToken,
                ExpiresAt: DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn));
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.OAuth.ExchangeFailed",
                $"Failed to exchange authorization code: {ex.Message}");
        }
    }

    public async Task<ErrorOr<OAuthTokens>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new TokenRefreshRequest(
                ClientId: settings.ClientId,
                ClientSecret: settings.ClientSecret,
                RefreshToken: refreshToken,
                GrantType: "refresh_token");

            var response = await api.RefreshTokenAsync(request);

            return new OAuthTokens(
                AccessToken: response.AccessToken,
                RefreshToken: response.RefreshToken,
                ExpiresAt: DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn));
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.OAuth.RefreshFailed",
                $"Failed to refresh access token: {ex.Message}");
        }
    }

    public async Task<ErrorOr<Success>> RevokeAccessAsync(
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await api.DeauthorizeAsync($"Bearer {accessToken}");
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.OAuth.RevokeFailed",
                $"Failed to revoke access: {ex.Message}");
        }
    }
}
