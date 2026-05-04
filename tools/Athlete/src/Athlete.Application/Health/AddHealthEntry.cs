using MediatR;
using Athlete.Domain.Health;
using Athlete.Domain.Repositories;

namespace Athlete.Application.Health;

/// <summary>
/// Command to add a new health entry.
/// </summary>
public sealed record AddHealthEntryRequest(
    Guid AthleteId,
    DateOnly Date,
    HealthEntryType EntryType,
    Severity Severity,
    string? Notes = null,
    string? BodyLocation = null,
    bool AffectedRunning = false) : IRequest<AddHealthEntryResponse>;

/// <summary>
/// Response containing the created health entry's ID.
/// </summary>
public sealed record AddHealthEntryResponse(Guid EntryId);

/// <summary>
/// Handler for adding a health entry.
/// </summary>
internal sealed class AddHealthEntryHandler(IHealthEntryRepository repository) : IRequestHandler<AddHealthEntryRequest, AddHealthEntryResponse>
{
    public async Task<AddHealthEntryResponse> Handle(AddHealthEntryRequest request, CancellationToken cancellationToken)
    {
        var entry = new HealthEntry(
            request.AthleteId,
            request.Date,
            request.EntryType,
            request.Severity,
            request.Notes,
            request.BodyLocation,
            request.AffectedRunning);

        await repository.AddAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new AddHealthEntryResponse(entry.Id);
    }
}
