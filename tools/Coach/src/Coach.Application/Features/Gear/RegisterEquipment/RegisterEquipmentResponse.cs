using Coach.Domain.Gear;

namespace Coach.Application.Features.Gear.RegisterEquipment;

public sealed record RegisterEquipmentResponse(
    Guid Id,
    Guid AthleteId,
    EquipmentType Type,
    string Name,
    string? Brand,
    string? Model,
    DateOnly? FirstUsedOn,
    decimal? RetireAfterKm);
