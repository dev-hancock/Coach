using Coach.Domain.Health;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;
using MediatR;

namespace Coach.Application.Features.Health.GetHealthSummary;

/// <summary>
/// Query to get a health summary for an athlete.
/// </summary>
public sealed record GetHealthSummaryRequest(
    Guid AthleteId,
    int DaysBack = 30) : IRequest<HealthSummaryResponse>;

/// <summary>
/// Response containing health summary data.
/// </summary>
public sealed record HealthSummaryResponse(
    IReadOnlyList<HealthEntryDto> RecentEntries,
    IReadOnlyList<HealthEntryDto> EntriesRequiringFollowUp,
    int TotalEntriesInPeriod,
    int HighSeverityCount);

/// <summary>
/// DTO for health entry.
/// </summary>
public sealed record HealthEntryDto(
    Guid Id,
    DateOnly Date,
    HealthEntryType EntryType,
    Severity Severity,
    string? BodyLocation,
    string? Notes,
    bool AffectedRunning,
    bool RequiresFollowUp);

/// <summary>
/// Handler for getting health summary.
/// </summary>
internal sealed class GetHealthSummaryHandler(IReadRepository<HealthEntry> repository) : IRequestHandler<GetHealthSummaryRequest, HealthSummaryResponse>
{
    public async Task<HealthSummaryResponse> Handle(GetHealthSummaryRequest request, CancellationToken cancellationToken)
    {
        // Get recent entries
        var recentEntries = await repository.ListAsync(
            new RecentHealthEntriesSpec(request.AthleteId, request.DaysBack),
            cancellationToken);

        // Get entries requiring follow-up
        var followUpEntries = await repository.ListAsync(
            new HealthEntriesRequiringFollowUpSpec(request.AthleteId),
            cancellationToken);

        // Get high severity entries
        var highSeverityEntries = await repository.ListAsync(
            new HealthEntriesBySeveritySpec(request.AthleteId, Severity.High, request.DaysBack),
            cancellationToken);

        var recentDtos = recentEntries.Select(MapToDto).ToList();
        var followUpDtos = followUpEntries.Select(MapToDto).ToList();

        return new HealthSummaryResponse(
            recentDtos,
            followUpDtos,
            recentEntries.Count,
            highSeverityEntries.Count);
    }

    private static HealthEntryDto MapToDto(HealthEntry entry)
    {
        return new HealthEntryDto(
            entry.Id,
            entry.Date,
            entry.EntryType,
            entry.Severity,
            entry.BodyLocation,
            entry.Notes,
            entry.AffectedRunning,
            entry.RequiresFollowUp);
    }
}
