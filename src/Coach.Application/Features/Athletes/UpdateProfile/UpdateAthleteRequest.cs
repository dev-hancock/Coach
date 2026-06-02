using Coach.Application.Abstractions.Identity;
using Coach.Domain.Athletes;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.UpdateProfile;

/// <summary>
/// Command to update an athlete's profile.
/// </summary>
public sealed record UpdateAthleteRequest(
    UserContext Context,
    ExperienceLevel ExperienceLevel,
    UnitType PreferredUnits,
    int TrainingDaysPerWeek,
    DayOfWeek? PreferredLongRunDay,
    decimal CurrentWeeklyDistanceKm,
    decimal TypicalLongRunDistanceKm,
    string? Notes = null) : IRequest<ErrorOr<UpdateAthleteResponse>>;
