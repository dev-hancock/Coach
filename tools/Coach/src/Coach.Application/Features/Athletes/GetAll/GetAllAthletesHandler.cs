using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.GetAll;

internal sealed class GetAllAthletesHandler 
    : IRequestHandler<GetAllAthletesRequest, ErrorOr<GetAllAthletesResponse>>
{
    private readonly IAthleteRepository _repository;

    public GetAllAthletesHandler(IAthleteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<GetAllAthletesResponse>> Handle(
        GetAllAthletesRequest request, 
        CancellationToken cancellationToken)
    {
        var athletes = await _repository.ListAsync(cancellationToken);

        var dtos = athletes.Select(a => new AthleteDto(
            a.Id,
            a.Name,
            a.Experience,
            a.Unit,
            a.TrainingDaysPerWeek,
            a.PreferredLongRunDay,
            a.CurrentWeeklyDistance.Kilometers,
            a.TypicalLongRunDistance.Kilometers,
            a.Notes
        )).ToList();

        return new GetAllAthletesResponse(dtos);
    }
}
