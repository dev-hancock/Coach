using MediatR;
using ErrorOr;
using Coach.Domain.Gear;
using Coach.Domain.Repositories;

namespace Coach.Application.Features.Gear.GetById;

internal sealed class GetEquipmentByIdHandler : IRequestHandler<GetEquipmentByIdRequest, ErrorOr<GetEquipmentByIdResponse>>
{
    private readonly IReadRepository<Equipment> _equipmentRepository;

    public GetEquipmentByIdHandler(IReadRepository<Equipment> equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<ErrorOr<GetEquipmentByIdResponse>> Handle(GetEquipmentByIdRequest request, CancellationToken cancellationToken)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId, cancellationToken);

        if (equipment == null)
        {
            return Error.NotFound("Equipment.NotFound", $"Equipment with ID {request.EquipmentId} not found.");
        }

        return new GetEquipmentByIdResponse(
            equipment.Id,
            equipment.AthleteId,
            equipment.Type,
            equipment.Name,
            equipment.Brand,
            equipment.Model,
            equipment.FirstUsedOn,
            equipment.RetireAfterKm,
            equipment.DistanceLoggedKm,
            equipment.IsRetired);
    }
}
