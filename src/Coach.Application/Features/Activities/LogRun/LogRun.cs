using Ardalis.Specification;
using Coach.Domain.Activities;
using Coach.Domain.Repositories;
using MediatR;

namespace Coach.Application.Features.Activities.LogRun;

/// <summary>
/// Command to log a completed run manually.
/// </summary>
public sealed record LogCompletedRunRequest(
    Guid AthleteId,
    DateTimeOffset StartedAt,
    decimal DistanceKm,
    TimeSpan Duration,
    int? RatePerceivedExertion = null,
    Guid? EquipmentId = null,
    string? Notes = null) : IRequest<LogCompletedRunResponse>;

/// <summary>
/// Response containing the logged run's ID.
/// </summary>
public sealed record LogCompletedRunResponse(Guid RunId);

/// <summary>
/// Handler for logging a completed run.
/// </summary>
internal sealed class LogCompletedRunHandler(IRepository<Activity> runs) : IRequestHandler<LogCompletedRunRequest, LogCompletedRunResponse>
{
    public async Task<LogCompletedRunResponse> Handle(LogCompletedRunRequest request, CancellationToken cancellationToken)
    {
        var run = new Activity(
            request.AthleteId,
            ActivityType.Run,
            request.StartedAt,
            request.Duration,
            request.DistanceKm,
            request.RatePerceivedExertion,
            ActivitySource.Manual);

        if (request.EquipmentId.HasValue)
        {
            run.AssignEquipment(request.EquipmentId.Value);
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
