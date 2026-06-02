using MediatR;
using ErrorOr;

namespace Coach.Application.Features.Gear.GetById;

public sealed record GetEquipmentByIdRequest(Guid EquipmentId) : IRequest<ErrorOr<GetEquipmentByIdResponse>>;
