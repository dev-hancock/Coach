using Coach.Application.Abstractions.Integrations;
using Coach.Application.Abstractions.Integrations.Strava;
using Coach.Infrastructure.Integrations.Strava.Auth.Contracts;
using ErrorOr;
using Microsoft.Extensions.Options;

namespace Coach.Infrastructure.Integrations.Strava;

/// <summary>
/// Implementation of Strava OAuth service using Refit.
/// </summary>
internal sealed class StravaAuthService(
    IStravaApi api,
    IOptions<StravaSettings> settings) : IStravaAuthService
{
    private readonly StravaSettings _settings = settings.Value;

    public async Task<ErrorOr<StravaTokenData>> ExchangeCodeAsync(
        string authorizationCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new ExchangeCodeRequest(
                _settings.ClientId,
                _settings.ClientSecret,
                authorizationCode);

            var response = await api.ExchangeCodeAsync(request);

            return new StravaTokenData(
                response.AccessToken,
                response.RefreshToken,
                DateTimeOffset.FromUnixTimeSeconds(response.ExpiresAt).UtcDateTime);
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.TokenExchange",
                $"Failed to exchange authorization code: {ex.Message}");
        }
    }

    public async Task<ErrorOr<StravaAthleteData>> GetAthleteAsync(
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await api.GetAthleteAsync($"Bearer {accessToken}");

            return new StravaAthleteData(
                response.Id,
                response.FirstName,
                response.LastName,
                response.Country,
                response.State,
                response.City,
                response.Sex,
                response.Weight,
                response.MeasurementPreference);
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.GetAthlete",
                $"Failed to get athlete: {ex.Message}");
        }
    }

    public async Task<ErrorOr<StravaTokenData>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new TokenRefreshRequest(
                _settings.ClientId,
                _settings.ClientSecret,
                refreshToken);

            var response = await api.RefreshTokenAsync(request);

            return new StravaTokenData(
                response.AccessToken,
                response.RefreshToken,
                DateTimeOffset.FromUnixTimeSeconds(response.ExpiresAt).UtcDateTime);
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.RefreshToken",
                $"Failed to refresh token: {ex.Message}");
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
                "Strava.Revoke",
                $"Failed to revoke access: {ex.Message}");
        }
    }
}
