using Ardalis.Specification;
using Coach.Domain.Health;
using Coach.Domain.Injuries;
using Coach.Domain.Repositories;
using MediatR;

namespace Coach.Application.Features.Injuries.AddInjuryEntry;

public sealed record AddInjuryEntryRequest(
    Guid AthleteId,
    DateOnly OnsetDate,
    InjuryType Type,
    Guid BodyLocationId,
    Severity Severity,
    int? PainLevel = null,
    string? Notes = null,
    bool PreventsTraining = false,
    DateOnly? EstimatedRecoveryDate = null) : IRequest<AddInjuryEntryResponse>;

public sealed record AddInjuryEntryResponse(Guid InjuryEntryId);

internal sealed class AddInjuryEntryHandler(IRepository<InjuryEntry> injuries)
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
            request.BodyLocationId,
            request.Severity,
            request.PainLevel,
            request.Notes,
            request.PreventsTraining,
            request.EstimatedRecoveryDate);

        await injuries.AddAsync(entry, cancellationToken);
        await injuries.SaveChangesAsync(cancellationToken);

        return new AddInjuryEntryResponse(entry.Id);
    }
}
