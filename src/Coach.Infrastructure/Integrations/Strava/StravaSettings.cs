namespace Coach.Infrastructure.Integrations.Strava;

/// <summary>
/// Strava API configuration settings.
/// </summary>
public sealed class StravaSettings
{
    public const string SectionName = "Strava";

    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public required string RedirectUri { get; init; }

    public required string WebhookVerifyToken { get; init; }
}
