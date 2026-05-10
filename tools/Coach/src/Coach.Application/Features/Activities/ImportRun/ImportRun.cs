using Coach.Domain.Activities;
using Coach.Domain.Repositories;
using MediatR;

namespace Coach.Application.Features.Activities.ImportRun;

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
    ActivitySource Source,
    string ExternalActivityId,
    Guid? EquipmentId = null) : IRequest<ImportFitRunResponse>;

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
        var run = new Activity(
            request.AthleteId,
            ActivityType.Run,
            request.StartedAt,
            request.Duration,
            request.DistanceKm,
            source: request.Source);

        run.AddExternalReference(request.Source, request.ExternalActivityId);

        run.AddGeneralMetrics(
            request.MovingTime,
            request.AverageHeartRate,
            request.MaxHeartRate,
            request.AverageCadence,
            request.ElevationGainMeters);

        if (request.EquipmentId.HasValue)
        {
            run.AssignEquipment(request.EquipmentId.Value);
        }

        await runs.AddAsync(run, cancellationToken);
        await runs.SaveChangesAsync(cancellationToken);

        return new ImportFitRunResponse(run.Id);
    }
}
