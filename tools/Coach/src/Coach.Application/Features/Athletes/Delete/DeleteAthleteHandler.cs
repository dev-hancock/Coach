using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.Delete;

internal sealed class DeleteAthleteHandler(IAthleteRepository athletes)
    : IRequestHandler<DeleteAthleteRequest, ErrorOr<DeleteAthleteResponse>>
{
    public async Task<ErrorOr<DeleteAthleteResponse>> Handle(
        DeleteAthleteRequest request, 
        CancellationToken cancellationToken)
    {
        var athlete = await athletes.GetByIdAsync(request.Id, cancellationToken);

        if (athlete is null)
        {
            return Error.NotFound(
                "Athlete.NotFound", 
                $"Athlete with ID {request.Id} was not found");
        }

        await athletes.DeleteAsync(athlete, cancellationToken);
        await athletes.SaveChangesAsync(cancellationToken);

        return new DeleteAthleteResponse(true);
    }
}
