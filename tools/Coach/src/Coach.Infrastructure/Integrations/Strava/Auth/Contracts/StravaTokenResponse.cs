using System.Text.Json.Serialization;

namespace Coach.Infrastructure.Integrations.Strava.Auth.Contracts;

public record StravaTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("refresh_token")] string RefreshToken,
    [property: JsonPropertyName("expires_at")] long ExpiresAt);