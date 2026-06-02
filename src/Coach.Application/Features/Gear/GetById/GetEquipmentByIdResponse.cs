using Coach.Domain.Gear;

namespace Coach.Application.Features.Gear.GetById;

public sealed record GetEquipmentByIdResponse(
    Guid Id,
    Guid AthleteId,
    EquipmentType Type,
    string Name,
    string? Brand,
    string? Model,
    DateOnly? FirstUsedOn,
    decimal? RetireAfterKm,
    decimal DistanceLoggedKm,
    bool IsRetired);
