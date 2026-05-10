using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.Delete;

internal sealed class DeleteAthleteHandler 
    : IRequestHandler<DeleteAthleteRequest, ErrorOr<DeleteAthleteResponse>>
{
    private readonly IAthleteRepository _repository;

    public DeleteAthleteHandler(IAthleteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<DeleteAthleteResponse>> Handle(
        DeleteAthleteRequest request, 
        CancellationToken cancellationToken)
    {
        var athlete = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (athlete is null)
        {
            return Error.NotFound(
                "Athlete.NotFound", 
                $"Athlete with ID {request.Id} was not found");
        }

        await _repository.DeleteAsync(athlete, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new DeleteAthleteResponse(true);
    }
}
