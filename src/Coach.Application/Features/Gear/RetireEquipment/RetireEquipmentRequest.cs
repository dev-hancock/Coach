using MediatR;
using ErrorOr;

namespace Coach.Application.Features.Gear.RetireEquipment;

public sealed record RetireEquipmentRequest(Guid EquipmentId) : IRequest<ErrorOr<Success>>;
