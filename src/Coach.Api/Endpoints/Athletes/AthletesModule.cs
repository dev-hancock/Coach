using Coach.Api.Extensions;
using Coach.Application.Features.Athletes.GetProfile;
using Coach.Application.Features.Athletes.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Coach.Api.Endpoints.Athletes.Contracts;

namespace Coach.Api.Endpoints.Athletes;

/// <summary>
/// Module for registering all athlete-related endpoints.
/// </summary>
public static class AthletesModule
{
    public static IEndpointRouteBuilder MapAthletes(this IEndpointRouteBuilder routes)
    {
        var api = routes.MapGroup("/api/athletes")
            .WithTags("Athletes");

        api.MapGet("/me", GetMyProfile)
            .WithName("GetMyAthleteProfile")
            .WithSummary("Get current user's athlete profile")
            .WithDescription("Retrieves the athlete profile for the authenticated user")
            .RequireAuthorization()
            .Produces<GetAthleteResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        api.MapPut("/me", UpdateMyProfile)
            .WithName("UpdateMyAthleteProfile")
            .WithSummary("Update current user's athlete profile")
            .WithDescription("Updates the athlete profile for the authenticated user")
            .RequireAuthorization()
            .Produces<UpdateAthleteResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return routes;
    }

    private static Task<IResult> GetMyProfile(
        ClaimsPrincipal user,
        ISender sender,
        CancellationToken cancellationToken)
    {
        return sender.Send(new GetAthleteRequest(user.GetUserContext()), cancellationToken).ToOkAsync();
    }

    private static Task<IResult> UpdateMyProfile(
        ClaimsPrincipal user,
        [FromBody] UpdateAthleteDto dto,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var request = new UpdateAthleteRequest(
            user.GetUserContext(),
            dto.ExperienceLevel,
            dto.PreferredUnits,
            dto.TrainingDaysPerWeek,
            dto.PreferredLongRunDay,
            dto.CurrentWeeklyDistanceKm,
            dto.TypicalLongRunDistanceKm,
            dto.Notes);

        return sender.Send(request, cancellationToken).ToOkAsync();
    }
}