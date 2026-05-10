using MediatR;
using ErrorOr;
using Coach.Domain.Gear;

namespace Coach.Application.Features.Gear.RegisterEquipment;

public sealed record RegisterEquipmentRequest(
    Guid AthleteId,
    EquipmentType Type,
    string Name,
    string? Brand = null,
    string? Model = null,
    DateOnly? FirstUsedOn = null,
    decimal? RetireAfterKm = null) : IRequest<ErrorOr<RegisterEquipmentResponse>>;
