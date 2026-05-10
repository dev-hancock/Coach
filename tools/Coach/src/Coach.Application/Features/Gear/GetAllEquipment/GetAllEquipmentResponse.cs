using Coach.Domain.Gear;

namespace Coach.Application.Features.Gear.GetAllEquipment;

public sealed record GetAllEquipmentResponse(IEnumerable<EquipmentDto> Equipment);

public sealed record EquipmentDto(
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
