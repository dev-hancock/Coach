using MediatR;
using ErrorOr;
using Coach.Domain.Repositories;
using EquipmentEntity = Coach.Domain.Gear.Equipment;

namespace Coach.Application.Features.Gear.RegisterEquipment;

internal sealed class RegisterEquipmentHandler : IRequestHandler<RegisterEquipmentRequest, ErrorOr<RegisterEquipmentResponse>>
{
    private readonly IEquipmentRepository _equipmentRepository;

    public RegisterEquipmentHandler(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<ErrorOr<RegisterEquipmentResponse>> Handle(RegisterEquipmentRequest request, CancellationToken cancellationToken)
    {
        var equipment = new EquipmentEntity(
            request.AthleteId,
            request.Type,
            request.Name,
            request.Brand,
            request.Model,
            request.FirstUsedOn,
            request.RetireAfterKm);

        await _equipmentRepository.AddAsync(equipment, cancellationToken);
        await _equipmentRepository.SaveChangesAsync(cancellationToken);

        return new RegisterEquipmentResponse(
            equipment.Id,
            equipment.AthleteId,
            equipment.Type,
            equipment.Name,
            equipment.Brand,
            equipment.Model,
            equipment.FirstUsedOn,
            equipment.RetireAfterKm);
    }
}
