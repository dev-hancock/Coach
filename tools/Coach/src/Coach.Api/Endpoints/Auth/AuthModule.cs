using Coach.Api.Extensions;
using Coach.Application.Features.Auth.Login;
using Coach.Application.Features.Auth.Logout;
using Coach.Application.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coach.Api.Endpoints.Auth;

public static class AuthModule
{
    public static IEndpointRouteBuilder MapCustomAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var api = routes.MapGroup("/api/auth")
            .WithTags("Authentication");

        api.MapPost("/register", Register)
            .WithName("Register")
            .WithSummary("Register a new user and create athlete profile")
            .WithDescription("Creates both a user account and linked athlete profile in one operation")
            .Produces<RegisterResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

        api.MapPost("/login", Login)
            .WithName("Login")
            .WithSummary("Authenticate user")
            .WithDescription("Authenticates a user with email and password")
            .Produces<LoginResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        api.MapPost("/logout", Logout)
            .RequireAuthorization()
            .WithName("Logout")
            .WithSummary("Sign out current user")
            .WithDescription("Signs out the currently authenticated user")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return routes;
    }

    private static Task<IResult> Register(
        [FromBody] RegisterRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        return sender.Send(request, cancellationToken).ToCreatedAsync(_ => "/api/athletes/me", response => response);
    }

    private static Task<IResult> Login(
        [FromBody] LoginRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        return sender.Send(request, cancellationToken).ToOkAsync();
    }

    private static Task<IResult> Logout(
        ISender sender,
        CancellationToken cancellationToken)
    {
        return sender.Send(new LogoutRequest(), cancellationToken).ToNoContentAsync();
    }
}
