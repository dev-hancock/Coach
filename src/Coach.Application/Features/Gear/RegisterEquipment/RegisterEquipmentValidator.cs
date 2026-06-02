using FluentValidation;

namespace Coach.Application.Features.Gear.RegisterEquipment;

public sealed class RegisterEquipmentValidator : AbstractValidator<RegisterEquipmentRequest>
{
    public RegisterEquipmentValidator()
    {
        RuleFor(x => x.AthleteId)
            .NotEmpty()
            .WithMessage("Athlete ID is required.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Equipment type must be a valid value.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Equipment name is required.")
            .MaximumLength(200)
            .WithMessage("Equipment name must not exceed 200 characters.");

        RuleFor(x => x.Brand)
            .MaximumLength(100)
            .WithMessage("Brand must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Brand));

        RuleFor(x => x.Model)
            .MaximumLength(100)
            .WithMessage("Model must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Model));

        RuleFor(x => x.RetireAfterKm)
            .GreaterThan(0)
            .WithMessage("Retirement distance must be greater than zero.")
            .When(x => x.RetireAfterKm.HasValue);

        RuleFor(x => x.FirstUsedOn)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("First used date cannot be in the future.")
            .When(x => x.FirstUsedOn.HasValue);
    }
}
