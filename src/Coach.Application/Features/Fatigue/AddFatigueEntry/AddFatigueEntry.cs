using Ardalis.Specification;
using Coach.Domain.Fatigue;
using Coach.Domain.Health;
using Coach.Domain.Repositories;
using MediatR;

namespace Coach.Application.Features.Fatigue.AddFatigueEntry;

public sealed record AddFatigueEntryRequest(
    Guid AthleteId,
    DateOnly Date,
    FatigueLevel Level,
    Severity Severity,
    int? RatePerceivedExertion = null,
    string? Notes = null,
    bool AffectsTraining = false) : IRequest<AddFatigueEntryResponse>;

public sealed record AddFatigueEntryResponse(Guid FatigueEntryId);

internal sealed class AddFatigueEntryHandler(IRepository<FatigueEntry> fatigue)
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
