using Ardalis.Specification;
using Coach.Domain.Gear;
using MediatR;
using ErrorOr;
using Coach.Domain.Repositories;

namespace Coach.Application.Features.Gear.RetireEquipment;

internal sealed class RetireEquipmentHandler : IRequestHandler<RetireEquipmentRequest, ErrorOr<Success>>
{
    private readonly IRepository<Equipment> _equipmentRepository;

    public RetireEquipmentHandler(IRepository<Equipment> equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<ErrorOr<Success>> Handle(RetireEquipmentRequest request, CancellationToken cancellationToken)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId, cancellationToken);

        if (equipment == null)
        {
            return Error.NotFound("Equipment.NotFound", $"Equipment with ID {request.EquipmentId} not found.");
        }

        if (equipment.IsRetired)
        {
            return Error.Validation("Equipment.AlreadyRetired", "Equipment is already retired.");
        }

        equipment.Retire();

        await _equipmentRepository.UpdateAsync(equipment, cancellationToken);
        await _equipmentRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
