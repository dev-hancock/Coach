using Coach.Domain.Athletes;

namespace Coach.Api.Endpoints.Athletes.Contracts;

public sealed record UpdateAthleteDto(
    ExperienceLevel ExperienceLevel,
    UnitType PreferredUnits,
    int TrainingDaysPerWeek,
    DayOfWeek? PreferredLongRunDay,
    decimal CurrentWeeklyDistanceKm,
    decimal TypicalLongRunDistanceKm,
    string? Notes = null);