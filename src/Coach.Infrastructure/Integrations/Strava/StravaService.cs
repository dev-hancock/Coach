using Coach.Application.Abstractions.Athletes;
using Coach.Application.Abstractions.Integrations;
using Coach.Domain.Integrations;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications.Integrations;
using ErrorOr;
using IntegrationType = Coach.Domain.Integrations.IntegrationType;

namespace Coach.Infrastructure.Integrations.Strava;

/// <summary>
/// Strava implementation of IIntegrationService.
/// Uses domain repository with Ardalis.Specification for queries.
/// </summary>
internal sealed class StravaService(
    IStravaApi stravaApi,
    IRepository<Integration> connectionRepository,
    IAthleteOnboardingService athleteOnboarding,
    StravaSettings settings) : IIntegrationService
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

        // 3. Save or update connection using domain entity & repository
        var spec = new GetIntegrationConnectionByUserAndTypeSpec(userId, IntegrationType.Strava);
        var existing = await connectionRepository.FirstOrDefaultAsync(spec, cancellationToken);

        Integration connection;
        if (existing is not null)
        {
            // Update existing connection
            existing.UpdateTokens(tokens.AccessToken, tokens.RefreshToken, tokens.ExpiresAt);
            await connectionRepository.UpdateAsync(existing, cancellationToken);
            connection = existing;
        }
        else
        {
            // Create new connection
            connection = new Integration(
                userId,
                IntegrationType.Strava,
                profile.ExternalAthleteId.ToString(),
                tokens.AccessToken,
                tokens.RefreshToken,
                tokens.ExpiresAt);

            await connectionRepository.AddAsync(connection, cancellationToken);
        }

        // 4. Onboard athlete (creates/updates athlete and raises domain event)
        var onboardingData = new AthleteOnboardingData(
            ExternalAthleteId: profile.ExternalAthleteId.ToString(),
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
            ExternalId: connection.ExternalId,
            ConnectedAt: connection.ConnectedAt);
    }

    public async Task<ErrorOr<Success>> DisconnectAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // Find connection using specification
        var spec = new GetIntegrationConnectionByUserAndTypeSpec(userId, IntegrationType.Strava);
        var connection = await connectionRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (connection is null)
        {
            return Error.NotFound("Integration.NotConnected", "Strava account not connected.");
        }

        // Revoke access on Strava
        var revokeResult = await RevokeAccessAsync(connection.AccessToken, cancellationToken);
        if (revokeResult.IsError)
        {
            return revokeResult.Errors;
        }

        // Remove local connection using repository
        await connectionRepository.DeleteAsync(connection, cancellationToken);
        return Result.Success;
    }

    public async IAsyncEnumerable<ErrorOr<ActivityInfo>> SyncActivitiesAsync(
        Guid userId,
        DateTimeOffset? since = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Find connection using specification
        var spec = new GetIntegrationConnectionByUserAndTypeSpec(userId, IntegrationType.Strava);
        var connection = await connectionRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (connection is null)
        {
            yield return Error.NotFound("Integration.NotConnected", "Strava account not connected.");
            yield break;
        }

        // Refresh token if expired
        if (connection.IsTokenExpired())
        {
            var refreshResult = await RefreshTokenAsync(connection.RefreshToken, cancellationToken);
            if (refreshResult.IsError)
            {
                yield return refreshResult.Errors;
                yield break;
            }

            var newTokens = refreshResult.Value;
            connection.UpdateTokens(newTokens.AccessToken, newTokens.RefreshToken, newTokens.ExpiresAt);
            await connectionRepository.UpdateAsync(connection, cancellationToken);
        }

        // Fetch activities from Strava API
        var sinceTimestamp = since?.ToUnixTimeSeconds() ?? 0;
        var activities = await stravaApi.GetActivitiesAsync(
            $"Bearer {connection.AccessToken}",
            after: sinceTimestamp);

        foreach (var activity in activities)
        {
            yield return new ActivityInfo(
                ExternalId: activity.Id.ToString(),
                Name: activity.Name,
                Type: activity.Type,
                StartDate: activity.StartDate,
                DurationSeconds: activity.MovingTime,
                DistanceMeters: (double)activity.Distance,
                ElevationGainMeters: null, // Not available in this response
                MaxHeartRate: activity.MaxHeartrate.HasValue ? (double)activity.MaxHeartrate.Value : null,
                AverageHeartRate: activity.AverageHeartrate.HasValue ? (double)activity.AverageHeartrate.Value : null,
                MaxSpeed: null, // Not available in this response
                AverageSpeed: null); // Not available in this response
        }

        // Record sync timestamp
        connection.RecordSync();
        await connectionRepository.UpdateAsync(connection, cancellationToken);
    }

    private async Task<ErrorOr<OAuthTokens>> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
    {
        try
        {
            var request = new Coach.Infrastructure.Integrations.Strava.Auth.Contracts.ExchangeCodeRequest(
                ClientId: settings.ClientId,
                ClientSecret: settings.ClientSecret,
                Code: code,
                GrantType: "authorization_code");

            var response = await stravaApi.ExchangeCodeAsync(request);
            return new OAuthTokens(
                AccessToken: response.AccessToken,
                RefreshToken: response.RefreshToken,
                ExpiresAt: DateTimeOffset.FromUnixTimeSeconds(response.ExpiresAt));
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
            var request = new Coach.Infrastructure.Integrations.Strava.Auth.Contracts.TokenRefreshRequest(
                ClientId: settings.ClientId,
                ClientSecret: settings.ClientSecret,
                RefreshToken: refreshToken,
                GrantType: "refresh_token");

            var response = await stravaApi.RefreshTokenAsync(request);
            return new OAuthTokens(
                AccessToken: response.AccessToken,
                RefreshToken: response.RefreshToken,
                ExpiresAt: DateTimeOffset.FromUnixTimeSeconds(response.ExpiresAt));
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
            await stravaApi.DeauthorizeAsync($"Bearer {accessToken}");
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
            var response = await stravaApi.GetAthleteAsync($"Bearer {accessToken}");
            return new AthleteProfile(
                ExternalAthleteId: response.Id,
                FirstName: response.FirstName,
                LastName: response.LastName,
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
