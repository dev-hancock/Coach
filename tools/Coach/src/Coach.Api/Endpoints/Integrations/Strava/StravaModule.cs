using Coach.Api.Endpoints.Integrations.Strava.Contracts;
using Coach.Api.Extensions;
using Coach.Application.Features.Integrations.Strava.Connect;
using Coach.Application.Features.Integrations.Strava.Disconnect;
using Coach.Application.Features.Integrations.Strava.Sync;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coach.Api.Endpoints.Integrations.Strava;

/// <summary>
/// Module for registering Strava integration endpoints.
/// </summary>
public static class StravaModule
{
    public static IEndpointRouteBuilder MapStrava(this IEndpointRouteBuilder routes)
    {
        var api = routes.MapGroup("/strava")
            .WithTags("Strava Integration");

        api.MapGet("/connect", ConnectStrava)
            .WithName("ConnectStrava")
            .WithSummary("Initiate Strava OAuth connection")
            .WithDescription("Redirects to Strava authorization page")
            .RequireAuthorization()
            .Produces(StatusCodes.Status302Found)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        api.MapGet("/callback", StravaCallback)
            .WithName("StravaCallback")
            .WithSummary("Handle Strava OAuth callback")
            .WithDescription("Processes the OAuth callback from Strava and stores connection")
            .AllowAnonymous()
            .Produces(StatusCodes.Status302Found)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        api.MapPost("/disconnect", DisconnectStrava)
            .WithName("DisconnectStrava")
            .WithSummary("Disconnect Strava integration")
            .WithDescription("Removes Strava connection and deletes imported activities")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        api.MapPost("/sync", SyncStrava)
            .WithName("SyncStrava")
            .WithSummary("Sync activities from Strava")
            .WithDescription("Imports new activities from Strava")
            .RequireAuthorization()
            .Produces<SyncStravaResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        api.MapPost("/webhook", StravaWebhook)
            .WithName("StravaWebhook")
            .WithSummary("Handle Strava webhook events")
            .WithDescription("Processes webhook events from Strava (deauthorization, activity updates)")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK);

        return routes;
    }

    private static IResult ConnectStrava(
        ClaimsPrincipal user,
        IConfiguration config)
    {
        if (!user.IsAuthenticated())
            return Results.Unauthorized();

        var userId = user.GetUserId();

        var clientId = config["Strava:ClientId"];
        var redirectUri = config["Strava:RedirectUri"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(redirectUri))
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Configuration error",
                detail: "Strava integration is not configured.");

        // Generate state token (user ID encoded for CSRF protection)
        var state = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userId.ToString()));

        var authUrl =
            $"https://www.strava.com/oauth/authorize?" +
            $"client_id={clientId}&" +
            $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
            $"response_type=code&" +
            $"scope=activity:read_all&" +
            $"state={state}";

        return Results.Redirect(authUrl);
    }

    private static async Task<IResult> StravaCallback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        [FromQuery] string? error,
        ISender sender,
        CancellationToken cancellationToken)
    {
        // User denied authorization
        if (!string.IsNullOrEmpty(error))
            return Results.Redirect("/profile?strava=denied");

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            return Results.Redirect("/profile?strava=error");

        // Decode state to get user ID
        Guid userId;
        try
        {
            var stateBytes = Convert.FromBase64String(state);
            var userIdString = System.Text.Encoding.UTF8.GetString(stateBytes);
            userId = Guid.Parse(userIdString);
        }
        catch
        {
            return Results.Redirect("/profile?strava=invalid_state");
        }

        var request = new ConnectStravaRequest(userId, code);
        var result = await sender.Send(request, cancellationToken);
        return result.Match(
            success => Results.Redirect("/profile?strava=connected"),
            errors => Results.Redirect("/profile?strava=error"));
    }

    private static Task<IResult> DisconnectStrava(
        ClaimsPrincipal user,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var userId = user.GetUserId();

        var request = new DisconnectStravaRequest(userId);
        return sender.Send(request, cancellationToken).ToNoContentAsync();
    }

    private static Task<IResult> SyncStrava(
        ClaimsPrincipal user,
        [FromBody] SyncStravaDto? dto,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var userId = user.GetUserId();

        var request = new SyncStravaRequest(
            userId,
            dto?.Since,
            dto?.PageSize);

        return sender.Send(request, cancellationToken).ToOkAsync();
    }

    private static async Task<IResult> StravaWebhook(
        [FromQuery] string? hub_mode,
        [FromQuery] string? hub_verify_token,
        [FromQuery] string? hub_challenge,
        [FromBody] StravaEventDto? webhookEvent,
        ISender sender,
        IConfiguration config,
        CancellationToken cancellationToken)
    {
        // Subscription validation (GET request)
        if (hub_mode == "subscribe")
        {
            var verifyToken = config["Strava:WebhookVerifyToken"];
            if (hub_verify_token == verifyToken && !string.IsNullOrEmpty(hub_challenge))
            {
                return Results.Json(new { hub_challenge });
            }
            return Results.Unauthorized();
        }

        // Webhook event (POST request)
        if (webhookEvent is not null)
        {
            // For now, just acknowledge receipt
            // TODO: Process webhook events (deauthorization, new activities, etc.)
            return Results.Ok();
        }

        return Results.BadRequest();
    }
}