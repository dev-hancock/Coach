using MediatR;
using AthleteMcpServer.Domain.Athletes;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Application.Athletes;

/// <summary>
/// Command to create a new athlete.
/// </summary>
public sealed record CreateAthleteRequest(string Name) : IRequest<CreateAthleteResponse>;

/// <summary>
/// Response containing the created athlete's details.
/// </summary>
public sealed record CreateAthleteResponse(Guid AthleteId, string Name);

/// <summary>
/// Handler for creating a new athlete.
/// </summary>
internal sealed class CreateAthleteHandler(IRepository<Athlete> athletes) : IRequestHandler<CreateAthleteRequest, CreateAthleteResponse>
{
    public async Task<CreateAthleteResponse> Handle(CreateAthleteRequest request, CancellationToken cancellationToken)
    {
        var athlete = new Athlete(request.Name);

        await athletes.AddAsync(athlete, cancellationToken);
        await athletes.SaveChangesAsync(cancellationToken);

        return new CreateAthleteResponse(athlete.Id, athlete.Name);
    }
}

