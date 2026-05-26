using Coach.Domain.Athletes;
using Coach.Domain.Common;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.UpdateProfile;

/// <summary>
/// Handler for updating an athlete's profile.
/// </summary>
internal sealed class UpdateAthleteHandler(IRepository<Athlete> athletes) 
    : IRequestHandler<UpdateAthleteRequest, ErrorOr<UpdateAthleteResponse>>
{
    public async Task<ErrorOr<UpdateAthleteResponse>> Handle(
        UpdateAthleteRequest request, 
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

        athlete.Update(
            request.ExperienceLevel,
            request.PreferredUnits,
            request.TrainingDaysPerWeek,
            request.PreferredLongRunDay,
            Distance.FromKilometers(request.CurrentWeeklyDistanceKm),
            Distance.FromKilometers(request.TypicalLongRunDistanceKm),
            request.Notes);

        await athletes.UpdateAsync(athlete, cancellationToken);
        await athletes.SaveChangesAsync(cancellationToken);

        return new UpdateAthleteResponse(
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
