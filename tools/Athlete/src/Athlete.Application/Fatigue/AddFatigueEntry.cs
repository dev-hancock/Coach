using Athlete.Domain.Fatigue;
using Athlete.Domain.Health;
using Athlete.Domain.Repositories;
using MediatR;

namespace Athlete.Application.Fatigue;

public sealed record AddFatigueEntryRequest(
    Guid AthleteId,
    DateOnly Date,
    FatigueLevel Level,
    Severity Severity,
    int? RatePerceivedExertion = null,
    string? Notes = null,
    bool AffectsTraining = false) : IRequest<AddFatigueEntryResponse>;

public sealed record AddFatigueEntryResponse(Guid FatigueEntryId);

internal sealed class AddFatigueEntryHandler(IFatigueEntryRepository fatigue)
    : IRequestHandler<AddFatigueEntryRequest, AddFatigueEntryResponse>
{
    public async Task<AddFatigueEntryResponse> Handle(
        AddFatigueEntryRequest request,
        CancellationToken cancellationToken)
    {
        var entry = new FatigueEntry(
            request.AthleteId,
            request.Date,
            request.Level,
            request.Severity,
            request.RatePerceivedExertion,
            request.Notes,
            request.AffectsTraining);

        await fatigue.AddAsync(entry, cancellationToken);
        await fatigue.SaveChangesAsync(cancellationToken);

        return new AddFatigueEntryResponse(entry.Id);
    }
}
