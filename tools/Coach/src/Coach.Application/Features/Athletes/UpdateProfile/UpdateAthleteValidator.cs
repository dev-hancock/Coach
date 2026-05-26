using FluentValidation;

namespace Coach.Application.Features.Athletes.UpdateProfile;

public sealed class UpdateAthleteValidator : AbstractValidator<UpdateAthleteRequest>
{
    public UpdateAthleteValidator()
    {
        RuleFor(x => x.TrainingDaysPerWeek)
            .InclusiveBetween(1, 7)
            .WithMessage("Training days per week must be between 1 and 7.");

        RuleFor(x => x.CurrentWeeklyDistanceKm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Current weekly distance must be non-negative.");

        RuleFor(x => x.TypicalLongRunDistanceKm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Typical long run distance must be non-negative.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 1000 characters.");
    }
}
