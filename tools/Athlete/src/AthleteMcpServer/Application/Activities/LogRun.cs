using MediatR;
using AthleteMcpServer.Domain.Activities;
using AthleteMcpServer.Domain.Repositories;

namespace AthleteMcpServer.Application.Activities;

/// <summary>
/// Command to log a completed run manually.
/// </summary>
public sealed record LogCompletedRunRequest(
    Guid AthleteId,
    DateTimeOffset StartedAt,
    decimal DistanceKm,
    TimeSpan Duration,
    int? RatePerceivedExertion = null,
    Guid? ShoeId = null,
    string? Notes = null) : IRequest<LogCompletedRunResponse>;

/// <summary>
/// Response containing the logged run's ID.
/// </summary>
public sealed record LogCompletedRunResponse(Guid RunId);

/// <summary>
/// Handler for logging a completed run.
/// </summary>
internal sealed class LogCompletedRunHandler(ICompletedRunRepository runs) : IRequestHandler<LogCompletedRunRequest, LogCompletedRunResponse>
{
    public async Task<LogCompletedRunResponse> Handle(LogCompletedRunRequest request, CancellationToken cancellationToken)
    {
        var run = new CompletedRun(
            request.AthleteId,
            request.StartedAt,
            request.DistanceKm,
            request.Duration,
            request.RatePerceivedExertion,
            RunSource.Manual);

        if (request.ShoeId.HasValue)
        {
            run.AssignShoe(request.ShoeId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            run.AddNotes(request.Notes);
        }

        await runs.AddAsync(run, cancellationToken);
        await runs.SaveChangesAsync(cancellationToken);

        return new LogCompletedRunResponse(run.Id);
    }
}
