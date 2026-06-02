using MediatR;
using ErrorOr;

namespace Coach.Application.Features.Gear.AddMileage;

public sealed record AddMileageRequest(
    Guid EquipmentId,
    decimal DistanceKm) : IRequest<ErrorOr<Success>>;
