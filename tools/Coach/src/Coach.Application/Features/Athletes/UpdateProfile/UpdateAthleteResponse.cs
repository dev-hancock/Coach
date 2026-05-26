using Coach.Domain.Athletes;

namespace Coach.Application.Features.Athletes.UpdateProfile;

/// <summary>
/// Response containing the updated athlete profile.
/// </summary>
public sealed record UpdateAthleteResponse(
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