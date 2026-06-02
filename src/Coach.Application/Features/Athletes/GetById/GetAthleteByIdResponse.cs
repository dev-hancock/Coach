using Coach.Domain.Athletes;

namespace Coach.Application.Features.Athletes.GetById;

public record GetAthleteByIdResponse(
    Guid Id,
    string Name,
    ExperienceLevel ExperienceLevel,
    UnitType PreferredUnits,
    int TrainingDaysPerWeek,
    DayOfWeek? PreferredLongRunDay,
    decimal CurrentWeeklyDistanceKm,
    decimal TypicalLongRunDistanceKm,
    string? Notes
);
