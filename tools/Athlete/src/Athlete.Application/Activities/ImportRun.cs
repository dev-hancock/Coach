using MediatR;
using Athlete.Domain.Activities;
using Athlete.Domain.Repositories;

namespace Athlete.Application.Activities;

/// <summary>
/// Command to import a run from a FIT file or external source.
/// </summary>
public sealed record ImportFitRunRequest(
    Guid AthleteId,
    DateTimeOffset StartedAt,
    decimal DistanceKm,
    TimeSpan Duration,
    TimeSpan? MovingTime,
    int? AverageHeartRate,
    int? MaxHeartRate,
    int? AverageCadence,
    decimal? ElevationGainMeters,
    RunSource Source,
    string ExternalActivityId,
    Guid? ShoeId = null) : IRequest<ImportFitRunResponse>;

/// <summary>
/// Response containing the imported run's ID.
/// </summary>
public sealed record ImportFitRunResponse(Guid RunId);

/// <summary>
/// Handler for importing a run from external sources.
/// </summary>
internal sealed class ImportFitRunHandler(ICompletedRunRepository runs) : IRequestHandler<ImportFitRunRequest, ImportFitRunResponse>
{
    public async Task<ImportFitRunResponse> Handle(ImportFitRunRequest request, CancellationToken cancellationToken)
    {
        var run = new CompletedRun(
            request.AthleteId,
            request.StartedAt,
            request.DistanceKm,
            request.Duration,
            source: request.Source);

        run.AddExternalReference(request.Source, request.ExternalActivityId);

        run.AddMetrics(
            request.MovingTime,
            request.AverageHeartRate,
            request.MaxHeartRate,
            request.AverageCadence,
            request.ElevationGainMeters);

        if (request.ShoeId.HasValue)
        {
            run.AssignShoe(request.ShoeId.Value);
        }

        await runs.AddAsync(run, cancellationToken);
        await runs.SaveChangesAsync(cancellationToken);

        return new ImportFitRunResponse(run.Id);
    }
}
