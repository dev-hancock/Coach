using MediatR;
using ErrorOr;
using Coach.Domain.Repositories;

namespace Coach.Application.Features.Gear.AddMileage;

internal sealed class AddMileageHandler : IRequestHandler<AddMileageRequest, ErrorOr<Success>>
{
    private readonly IEquipmentRepository _equipmentRepository;

    public AddMileageHandler(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<ErrorOr<Success>> Handle(AddMileageRequest request, CancellationToken cancellationToken)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId, cancellationToken);

        if (equipment == null)
        {
            return Error.NotFound("Equipment.NotFound", $"Equipment with ID {request.EquipmentId} not found.");
        }

        if (equipment.IsRetired)
        {
            return Error.Validation("Equipment.Retired", "Cannot add mileage to retired equipment.");
        }

        equipment.AddMileage(request.DistanceKm);

        await _equipmentRepository.UpdateAsync(equipment, cancellationToken);
        await _equipmentRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
