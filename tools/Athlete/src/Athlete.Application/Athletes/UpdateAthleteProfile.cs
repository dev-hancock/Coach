using MediatR;
using Athlete.Domain.Athletes;
using Athlete.Domain.Repositories;
using Athlete.Domain.Specifications;

namespace Athlete.Application.Athletes;

/// <summary>
/// Command to update an athlete's profile.
/// </summary>
public sealed record UpdateAthleteProfileRequest(
    Guid AthleteId,
    ExperienceLevel ExperienceLevel,
    UnitSystem PreferredUnits,
    int TrainingDaysPerWeek,
    DayOfWeek? PreferredLongRunDay,
    decimal CurrentWeeklyDistanceKm,
    decimal TypicalLongRunDistanceKm,
    string? Notes = null) : IRequest;

/// <summary>
/// Handler for updating an athlete's profile.
/// </summary>
internal sealed class UpdateAthleteProfileHandler(IRepository<Athlete> athletes) : IRequestHandler<UpdateAthleteProfileRequest>
{
    public async Task Handle(UpdateAthleteProfileRequest request, CancellationToken cancellationToken)
    {
        var athlete = await athletes.FirstOrDefaultAsync(
            new AthleteByIdSpec(request.AthleteId), 
            cancellationToken)
            ?? throw new InvalidOperationException($"Athlete with ID {request.AthleteId} not found.");

        athlete.UpdateProfile(
            request.ExperienceLevel,
            request.PreferredUnits,
            request.TrainingDaysPerWeek,
            request.PreferredLongRunDay,
            request.CurrentWeeklyDistanceKm,
            request.TypicalLongRunDistanceKm,
            request.Notes);

        await athletes.UpdateAsync(athlete, cancellationToken);
        await athletes.SaveChangesAsync(cancellationToken);
    }
}
