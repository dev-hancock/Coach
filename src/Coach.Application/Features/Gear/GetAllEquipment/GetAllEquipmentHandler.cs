using Ardalis.Specification;
using MediatR;
using ErrorOr;
using Coach.Domain.Gear;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;

namespace Coach.Application.Features.Gear.GetAllEquipment;

internal sealed class GetAllEquipmentHandler(IRepository<Equipment> equipment)
    : IRequestHandler<GetAllEquipmentRequest, ErrorOr<GetAllEquipmentResponse>>
{
    public async Task<ErrorOr<GetAllEquipmentResponse>> Handle(GetAllEquipmentRequest request, CancellationToken cancellationToken)
    {
        var spec = request.Type.HasValue
            ? new ActiveEquipmentByAthleteSpec(request.AthleteId, request.Type.Value)
            : new ActiveEquipmentByAthleteSpec(request.AthleteId);

        var equipment1 = await equipment.ListAsync(spec, cancellationToken);

        var equipmentDtos = equipment1.Select(e => new EquipmentDto(
            e.Id,
            e.AthleteId,
            e.Type,
            e.Name,
            e.Brand,
            e.Model,
            e.FirstUsedOn,
            e.RetireAfterKm,
            e.DistanceLoggedKm,
            e.IsRetired));

        return new GetAllEquipmentResponse(equipmentDtos);
    }
}
