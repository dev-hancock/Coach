using MediatR;
using ErrorOr;
using Coach.Domain.Gear;
using Coach.Domain.Repositories;
using Coach.Domain.Specifications;

namespace Coach.Application.Features.Gear.GetAllEquipment;

internal sealed class GetAllEquipmentHandler : IRequestHandler<GetAllEquipmentRequest, ErrorOr<GetAllEquipmentResponse>>
{
    private readonly Ardalis.Specification.IReadRepositoryBase<Equipment> _equipmentRepository;

    public GetAllEquipmentHandler(Ardalis.Specification.IReadRepositoryBase<Equipment> equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<ErrorOr<GetAllEquipmentResponse>> Handle(GetAllEquipmentRequest request, CancellationToken cancellationToken)
    {
        var spec = request.Type.HasValue
            ? new ActiveEquipmentByAthleteSpec(request.AthleteId, request.Type.Value)
            : new ActiveEquipmentByAthleteSpec(request.AthleteId);

        var equipment = await _equipmentRepository.ListAsync(spec, cancellationToken);

        var equipmentDtos = equipment.Select(e => new EquipmentDto(
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
