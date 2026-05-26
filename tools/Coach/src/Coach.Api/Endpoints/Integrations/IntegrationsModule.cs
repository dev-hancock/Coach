using Coach.Api.Endpoints.Integrations.Strava;

namespace Coach.Api.Endpoints.Integrations;

/// <summary>
/// Module for registering all integration endpoints (Strava, Garmin, etc.).
/// </summary>
public static class IntegrationsModule
{
    public static IEndpointRouteBuilder MapIntegrations(this IEndpointRouteBuilder routes)
    {
        var api = routes.MapGroup("/api/integrations")
            .WithTags("Integrations");

        // Map integration-specific modules
        api.MapStrava();

        return routes;
    }
}
