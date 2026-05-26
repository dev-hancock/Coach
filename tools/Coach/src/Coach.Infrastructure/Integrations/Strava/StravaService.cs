using Coach.Application.Abstractions.Athletes;
using Coach.Application.Abstractions.Integrations;
using Coach.Domain.Integrations;
using ErrorOr;

namespace Coach.Infrastructure.Integrations.Strava;

/// <summary>
/// Strava implementation of IIntegrationService.
/// Composes OAuth adapter, profile fetching, and activity sync through DI.
/// </summary>
internal sealed class StravaService(
    IStravaApi stravaApi,
    IIntegrationConnectionRepository connectionRepository,
    IAthleteOnboardingService athleteOnboarding) : IIntegrationService
{
    public IntegrationType Type => IntegrationType.Strava;

    public async Task<ErrorOr<IntegrationConnectionResult>> ConnectAsync(
        Guid userId,
        string authorizationCode,
        CancellationToken cancellationToken = default)
    {
        // 1. Exchange code for tokens
        var tokenResult = await ExchangeCodeAsync(authorizationCode, cancellationToken);
        if (tokenResult.IsError)
        {
            return tokenResult.Errors;
        }

        var tokens = tokenResult.Value;

        // 2. Get athlete profile
        var profileResult = await GetProfileAsync(tokens.AccessToken, cancellationToken);
        if (profileResult.IsError)
        {
            return profileResult.Errors;
        }

        var profile = profileResult.Value;

        // 3. Save connection
        var connection = new Connection(
            ExternalId: profile.ExternalAthleteId,
            AccessToken: tokens.AccessToken,
            RefreshToken: tokens.RefreshToken,
            ExpiresAt: tokens.ExpiresAt,
            ConnectedAt: DateTimeOffset.UtcNow,
            LastSyncedAt: null);

        var saveResult = await connectionRepository.SaveAsync(
            userId,
            IntegrationType.Strava,
            connection,
            cancellationToken);

        if (saveResult.IsError)
        {
            return saveResult.Errors;
        }

        // 4. Onboard athlete (creates/updates athlete and raises domain event)
        var onboardingData = new AthleteOnboardingData(
            ExternalAthleteId: profile.ExternalAthleteId,
            FirstName: profile.FirstName,
            LastName: profile.LastName,
            Country: profile.Country,
            State: profile.State,
            City: profile.City,
            Sex: profile.Sex,
            Weight: profile.Weight,
            MeasurementPreference: profile.MeasurementPreference);

        var onboardResult = await athleteOnboarding.OnboardFromIntegrationAsync(
            userId,
            IntegrationType.Strava,
            onboardingData,
            cancellationToken);

        if (onboardResult.IsError)
        {
            return onboardResult.Errors;
        }

        return new IntegrationConnectionResult(
            AthleteId: onboardResult.Value,
            ExternalId: profile.ExternalAthleteId,
            ConnectedAt: connection.ConnectedAt);
    }

    public async Task<ErrorOr<Success>> DisconnectAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var connectionResult = await connectionRepository.GetAsync(userId, IntegrationType.Strava, cancellationToken);
        if (connectionResult.IsError)
        {
            return connectionResult.Errors;
        }

        var connection = connectionResult.Value;

        // Revoke access on Strava
        var revokeResult = await RevokeAccessAsync(connection.AccessToken, cancellationToken);
        if (revokeResult.IsError)
        {
            return revokeResult.Errors;
        }

        // Remove local connection
        return await connectionRepository.RemoveAsync(userId, IntegrationType.Strava, cancellationToken);
    }

    public async IAsyncEnumerable<ErrorOr<ActivityInfo>> SyncActivitiesAsync(
        Guid userId,
        DateTimeOffset? since = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var connectionResult = await connectionRepository.GetAsync(userId, IntegrationType.Strava, cancellationToken);
        if (connectionResult.IsError)
        {
            yield return connectionResult.Errors;
            yield break;
        }

        var connection = connectionResult.Value;

        // Refresh token if expired
        if (connection.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            var refreshResult = await RefreshTokenAsync(connection.RefreshToken, cancellationToken);
            if (refreshResult.IsError)
            {
                yield return refreshResult.Errors;
                yield break;
            }

            var newTokens = refreshResult.Value;
            connection = connection with
            {
                AccessToken = newTokens.AccessToken,
                RefreshToken = newTokens.RefreshToken,
                ExpiresAt = newTokens.ExpiresAt
            };

            var updateResult = await connectionRepository.SaveAsync(userId, IntegrationType.Strava, connection, cancellationToken);
            if (updateResult.IsError)
            {
                yield return updateResult.Errors;
                yield break;
            }
        }

        // Fetch activities
        var sinceTimestamp = since?.ToUnixTimeSeconds() ?? 0;
        var activities = await stravaApi.GetActivitiesAsync(
            connection.AccessToken,
            after: sinceTimestamp,
            cancellationToken: cancellationToken);

        foreach (var activity in activities)
        {
            yield return new ActivityInfo(
                ExternalId: activity.Id.ToString(),
                Name: activity.Name,
                Type: activity.Type,
                StartDate: activity.StartDate,
                DurationSeconds: activity.MovingTime,
                DistanceMeters: activity.Distance,
                ElevationGainMeters: activity.TotalElevationGain,
                MaxHeartRate: activity.MaxHeartrate,
                AverageHeartRate: activity.AverageHeartrate,
                MaxSpeed: activity.MaxSpeed,
                AverageSpeed: activity.AverageSpeed);
        }
    }

    private async Task<ErrorOr<OAuthTokens>> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
    {
        try
        {
            var response = await stravaApi.ExchangeCodeAsync(code, cancellationToken);
            return new OAuthTokens(
                AccessToken: response.AccessToken,
                RefreshToken: response.RefreshToken,
                ExpiresAt: DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn));
        }
        catch (Exception ex)
        {
            return Error.Failure("Strava.OAuthFailed", ex.Message);
        }
    }

    private async Task<ErrorOr<OAuthTokens>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        try
        {
            var response = await stravaApi.RefreshTokenAsync(refreshToken, cancellationToken);
            return new OAuthTokens(
                AccessToken: response.AccessToken,
                RefreshToken: response.RefreshToken,
                ExpiresAt: DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn));
        }
        catch (Exception ex)
        {
            return Error.Failure("Strava.TokenRefreshFailed", ex.Message);
        }
    }

    private async Task<ErrorOr<Success>> RevokeAccessAsync(string accessToken, CancellationToken cancellationToken)
    {
        try
        {
            await stravaApi.DeauthorizeAsync(accessToken, cancellationToken);
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Error.Failure("Strava.RevokeFailed", ex.Message);
        }
    }

    private async Task<ErrorOr<AthleteProfile>> GetProfileAsync(string accessToken, CancellationToken cancellationToken)
    {
        try
        {
            var response = await stravaApi.GetAthleteAsync(accessToken, cancellationToken);
            return new AthleteProfile(
                ExternalAthleteId: response.Id.ToString(),
                FirstName: response.Firstname,
                LastName: response.Lastname,
                Country: response.Country,
                State: response.State,
                City: response.City,
                Sex: response.Sex,
                Weight: response.Weight,
                MeasurementPreference: response.MeasurementPreference);
        }
        catch (Exception ex)
        {
            return Error.Failure("Strava.GetProfileFailed", ex.Message);
        }
    }
}
