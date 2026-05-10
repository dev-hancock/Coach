using Coach.Domain.Athletes;

namespace Coach.Application.Features.Athletes.GetAll;

public record GetAllAthletesResponse(List<AthleteDto> Athletes);

public record AthleteDto(
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
