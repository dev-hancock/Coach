using Coach.Domain.Athletes;
using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.GetById;

internal sealed class GetAthleteByIdHandler(IRepository<Athlete> athletes)
    : IRequestHandler<GetAthleteByIdRequest, ErrorOr<GetAthleteByIdResponse>>
{
    public async Task<ErrorOr<GetAthleteByIdResponse>> Handle(
        GetAthleteByIdRequest request, 
        CancellationToken cancellationToken)
    {
        var athlete = await athletes.GetByIdAsync(request.Id, cancellationToken);

        if (athlete is null)
        {
            return Error.NotFound(
                "Athlete.NotFound", 
                $"Athlete with ID {request.Id} was not found");
        }

        return new GetAthleteByIdResponse(
            athlete.Id,
            athlete.Name,
            athlete.Experience,
            athlete.Unit,
            athlete.TrainingDaysPerWeek,
            athlete.PreferredLongRunDay,
            athlete.CurrentWeeklyDistance.Kilometers,
            athlete.TypicalLongRunDistance.Kilometers,
            athlete.Notes
        );
    }
}
