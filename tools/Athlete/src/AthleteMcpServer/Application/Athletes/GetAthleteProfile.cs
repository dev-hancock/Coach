using MediatR;
using AthleteMcpServer.Domain.Athletes;
using AthleteMcpServer.Domain.Repositories;
using AthleteMcpServer.Domain.Specifications;

namespace AthleteMcpServer.Application.Athletes;

/// <summary>
/// Query to get an athlete's profile.
/// </summary>
public sealed record GetAthleteProfileRequest(Guid AthleteId) : IRequest<AthleteProfileResponse>;

/// <summary>
/// Response containing athlete profile details.
/// </summary>
public sealed record AthleteProfileResponse(
    Guid Id,
    string Name,
    ExperienceLevel ExperienceLevel,
    UnitSystem PreferredUnits,
    int TrainingDaysPerWeek,
    DayOfWeek? PreferredLongRunDay,
    decimal CurrentWeeklyDistanceKm,
    decimal TypicalLongRunDistanceKm,
    string? Notes);

/// <summary>
/// Handler for retrieving an athlete's profile.
/// </summary>
internal sealed class GetAthleteProfileHandler(IReadRepository<Athlete> athletes) : IRequestHandler<GetAthleteProfileRequest, AthleteProfileResponse>
{
    public async Task<AthleteProfileResponse> Handle(GetAthleteProfileRequest request, CancellationToken cancellationToken)
    {
        var athlete = await athletes.FirstOrDefaultAsync(
            new AthleteByIdSpec(request.AthleteId), 
            cancellationToken)
            ?? throw new InvalidOperationException($"Athlete with ID {request.AthleteId} not found.");

        return new AthleteProfileResponse(
            athlete.Id,
            athlete.Name,
            athlete.ExperienceLevel,
            athlete.PreferredUnits,
            athlete.TrainingDaysPerWeek,
            athlete.PreferredLongRunDay,
            athlete.CurrentWeeklyDistanceKm,
            athlete.TypicalLongRunDistanceKm,
            athlete.Notes);
    }
}
