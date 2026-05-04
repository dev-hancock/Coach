using Athlete.Domain.Recovery;
using Athlete.Domain.Health;
using Athlete.Domain.Repositories;
using MediatR;

namespace Athlete.Application.Recovery;

public sealed record AddRecoveryEntryRequest(
    Guid AthleteId,
    DateOnly Date,
    RecoveryType Type,
    RecoveryQuality Quality,
    Severity Severity,
    double? SleepHours = null,
    string? Notes = null,
    bool AffectsPerformance = false) : IRequest<AddRecoveryEntryResponse>;

public sealed record AddRecoveryEntryResponse(Guid RecoveryEntryId);

internal sealed class AddRecoveryEntryHandler(IRecoveryEntryRepository recovery)
    : IRequestHandler<AddRecoveryEntryRequest, AddRecoveryEntryResponse>
{
    public async Task<AddRecoveryEntryResponse> Handle(
        AddRecoveryEntryRequest request,
        CancellationToken cancellationToken)
    {
        var entry = new RecoveryEntry(
            request.AthleteId,
            request.Date,
            request.Type,
            request.Quality,
            request.Severity,
            request.SleepHours,
            request.Notes,
            request.AffectsPerformance);

        await recovery.AddAsync(entry, cancellationToken);
        await recovery.SaveChangesAsync(cancellationToken);

        return new AddRecoveryEntryResponse(entry.Id);
    }
}
