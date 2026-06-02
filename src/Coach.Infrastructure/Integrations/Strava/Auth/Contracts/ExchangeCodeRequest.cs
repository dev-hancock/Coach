using Refit;

namespace Coach.Infrastructure.Integrations.Strava.Auth.Contracts;

public record ExchangeCodeRequest(
    [property: AliasAs("client_id")] string ClientId,
    [property: AliasAs("client_secret")] string ClientSecret,
    [property: AliasAs("code")] string Code,
    [property: AliasAs("grant_type")] string GrantType = "authorization_code");