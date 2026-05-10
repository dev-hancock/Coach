using FluentValidation;

namespace Coach.Application.Features.WorkoutPlans.Create;

public class CreateWorkoutPlanValidator : AbstractValidator<CreateWorkoutPlanRequest>
{
    public CreateWorkoutPlanValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Workout plan name is required")
            .MaximumLength(200)
            .WithMessage("Workout plan name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");
    }
}
