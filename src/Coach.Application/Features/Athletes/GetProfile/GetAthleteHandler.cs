using Coach.Domain.Athletes;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.GetProfile;

/// <summary>
/// Handler for retrieving an athlete's profile.
/// </summary>
internal sealed class GetAthleteHandler(Ardalis.Specification.IReadRepositoryBase<Athlete> athletes) 
    : IRequestHandler<GetAthleteRequest, ErrorOr<GetAthleteResponse>>
{
    public async Task<ErrorOr<GetAthleteResponse>> Handle(
        GetAthleteRequest request, 
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

        return new GetAthleteResponse(
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
