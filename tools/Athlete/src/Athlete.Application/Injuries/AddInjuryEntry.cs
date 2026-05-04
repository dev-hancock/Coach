using Athlete.Domain.Injuries;
using Athlete.Domain.Health;
using Athlete.Domain.Repositories;
using MediatR;

namespace Athlete.Application.Injuries;

public sealed record AddInjuryEntryRequest(
    Guid AthleteId,
    DateOnly OnsetDate,
    InjuryType Type,
    BodyLocation Location,
    Severity Severity,
    int? PainLevel = null,
    string? Notes = null,
    bool PreventsRunning = false,
    DateOnly? EstimatedRecoveryDate = null) : IRequest<AddInjuryEntryResponse>;

public sealed record AddInjuryEntryResponse(Guid InjuryEntryId);

internal sealed class AddInjuryEntryHandler(IInjuryEntryRepository injuries)
    : IRequestHandler<AddInjuryEntryRequest, AddInjuryEntryResponse>
{
    public async Task<AddInjuryEntryResponse> Handle(
        AddInjuryEntryRequest request,
        CancellationToken cancellationToken)
    {
        var entry = new InjuryEntry(
            request.AthleteId,
            request.OnsetDate,
            request.Type,
            request.Location,
            request.Severity,
            request.PainLevel,
            request.Notes,
            request.PreventsRunning,
            request.EstimatedRecoveryDate);

        await injuries.AddAsync(entry, cancellationToken);
        await injuries.SaveChangesAsync(cancellationToken);

        return new AddInjuryEntryResponse(entry.Id);
    }
}
