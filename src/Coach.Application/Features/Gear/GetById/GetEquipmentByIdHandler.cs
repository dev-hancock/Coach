using MediatR;
using ErrorOr;
using Coach.Domain.Gear;
using Coach.Domain.Repositories;

namespace Coach.Application.Features.Gear.GetById;

internal sealed class GetEquipmentByIdHandler(IRepository<Equipment> equipment)
    : IRequestHandler<GetEquipmentByIdRequest, ErrorOr<GetEquipmentByIdResponse>>
{
    public async Task<ErrorOr<GetEquipmentByIdResponse>> Handle(GetEquipmentByIdRequest request, CancellationToken cancellationToken)
    {
        var equipment1 = await equipment.GetByIdAsync(request.EquipmentId, cancellationToken);

        if (equipment1 == null)
        {
            return Error.NotFound("Equipment.NotFound", $"Equipment with ID {request.EquipmentId} not found.");
        }

        return new GetEquipmentByIdResponse(
            equipment1.Id,
            equipment1.AthleteId,
            equipment1.Type,
            equipment1.Name,
            equipment1.Brand,
            equipment1.Model,
            equipment1.FirstUsedOn,
            equipment1.RetireAfterKm,
            equipment1.DistanceLoggedKm,
            equipment1.IsRetired);
    }
}
