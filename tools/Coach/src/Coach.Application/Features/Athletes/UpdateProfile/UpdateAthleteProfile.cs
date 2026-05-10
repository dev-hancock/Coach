using Coach.Application.Abstractions.Identity;
using Coach.Domain.Athletes;
using Coach.Domain.Common;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.UpdateProfile;

/// <summary>
/// Command to update an athlete's profile.
/// </summary>
public sealed record UpdateAthleteProfileRequest(
    UserContext Context,
    ExperienceLevel ExperienceLevel,
    UnitType PreferredUnits,
    int TrainingDaysPerWeek,
    DayOfWeek? PreferredLongRunDay,
    decimal CurrentWeeklyDistanceKm,
    decimal TypicalLongRunDistanceKm,
    string? Notes = null) : IRequest<ErrorOr<UpdateAthleteProfileResponse>>;

/// <summary>
/// Response containing the updated athlete profile.
/// </summary>
public sealed record UpdateAthleteProfileResponse(
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
/// Handler for updating an athlete's profile.
/// </summary>
internal sealed class UpdateAthleteProfileHandler(IRepository<Athlete> athletes) 
    : IRequestHandler<UpdateAthleteProfileRequest, ErrorOr<UpdateAthleteProfileResponse>>
{
    public async Task<ErrorOr<UpdateAthleteProfileResponse>> Handle(
        UpdateAthleteProfileRequest request, 
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

        athlete.UpdateProfile(
            request.ExperienceLevel,
            request.PreferredUnits,
            request.TrainingDaysPerWeek,
            request.PreferredLongRunDay,
            Distance.FromKilometers(request.CurrentWeeklyDistanceKm),
            Distance.FromKilometers(request.TypicalLongRunDistanceKm),
            request.Notes);

        await athletes.UpdateAsync(athlete, cancellationToken);
        await athletes.SaveChangesAsync(cancellationToken);

        return new UpdateAthleteProfileResponse(
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
