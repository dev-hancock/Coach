using Coach.Domain.Gear;
using MediatR;
using ErrorOr;

namespace Coach.Application.Features.Gear.GetAllEquipment;

public sealed record GetAllEquipmentRequest(
    Guid AthleteId,
    EquipmentType? Type = null) : IRequest<ErrorOr<GetAllEquipmentResponse>>;
