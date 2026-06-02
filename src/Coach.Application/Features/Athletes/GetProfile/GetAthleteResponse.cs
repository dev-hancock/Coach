using Coach.Domain.Athletes;

namespace Coach.Application.Features.Athletes.GetProfile;

/// <summary>
/// Response containing athlete profile details.
/// </summary>
public sealed record GetAthleteResponse(
    Guid Id,
    Guid UserId,
    string Name,
    ExperienceLevel ExperienceLevel,
    UnitType PreferredUnits,
    int TrainingDaysPerWeek,
    DayOfWeek? PreferredLongRunDay,
    decimal CurrentWeeklyDistanceKm,
    decimal TypicalLongRunDistanceKm,
    string? Notes);
