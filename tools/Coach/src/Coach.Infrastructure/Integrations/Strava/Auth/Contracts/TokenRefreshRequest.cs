using Refit;

namespace Coach.Infrastructure.Integrations.Strava.Auth.Contracts;

public record TokenRefreshRequest(
    [property: AliasAs("client_id")] string ClientId,
    [property: AliasAs("client_secret")] string ClientSecret,
    [property: AliasAs("refresh_token")] string RefreshToken,
    [property: AliasAs("grant_type")] string GrantType = "refresh_token");