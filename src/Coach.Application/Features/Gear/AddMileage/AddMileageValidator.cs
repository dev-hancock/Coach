using FluentValidation;

namespace Coach.Application.Features.Gear.AddMileage;

public sealed class AddMileageValidator : AbstractValidator<AddMileageRequest>
{
    public AddMileageValidator()
    {
        RuleFor(x => x.EquipmentId)
            .NotEmpty()
            .WithMessage("Equipment ID is required.");

        RuleFor(x => x.DistanceKm)
            .GreaterThan(0)
            .WithMessage("Distance must be greater than zero.");
    }
}
