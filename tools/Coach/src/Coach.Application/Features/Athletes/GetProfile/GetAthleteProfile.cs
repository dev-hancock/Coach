using Coach.Application.Abstractions.Identity;
using Coach.Domain.Athletes;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.GetProfile;

/// <summary>
/// Query to get an athlete's profile.
/// </summary>
public sealed record GetAthleteProfileRequest(UserContext Context) : IRequest<ErrorOr<AthleteProfileResponse>>;

/// <summary>
/// Response containing athlete profile details.
/// </summary>
public sealed record AthleteProfileResponse(
    Guid Id,
    Guid UserId,
    string Name,
    ExperienceLevel ExperienceLevel,
    UnitType PreferredUnits,
    int TrainingDaysPerWeek,
    DayOfWeek? PreferredLongRunDay,
    decimal CurrentWeeklyDistanceKm,
    decimal TypicalLongRunDistanceKm,
    string? Notes);

/// <summary>
/// Handler for retrieving an athlete's profile.
/// </summary>
internal sealed class GetAthleteProfileHandler(IReadRepository<Athlete> athletes) 
    : IRequestHandler<GetAthleteProfileRequest, ErrorOr<AthleteProfileResponse>>
{
    public async Task<ErrorOr<AthleteProfileResponse>> Handle(
        GetAthleteProfileRequest request, 
        CancellationToken cancellationToken)
    {
        if (!request.Context.AthleteId.HasValue)
        {
            return Error.NotFound(
                "Athlete.NotLinked", 
                "No athlete profile exists for this user.");
        }

        var athlete = await athletes.FirstOrDefaultAsync(
            new AthleteByIdSpec(request.Context.AthleteId.Value), 
            cancellationToken);

        if (athlete is null)
        {
            return Error.NotFound(
                "Athlete.NotFound", 
                $"Athlete with ID {request.Context.AthleteId.Value} not found.");
        }

        return new AthleteProfileResponse(
            athlete.Id,
            athlete.UserId,
            athlete.Name,
            athlete.Experience,
            athlete.Unit,
            athlete.TrainingDaysPerWeek,
            athlete.PreferredLongRunDay,
            athlete.CurrentWeeklyDistance.Kilometers,
            athlete.TypicalLongRunDistance.Kilometers,
            athlete.Notes);
    }
}
