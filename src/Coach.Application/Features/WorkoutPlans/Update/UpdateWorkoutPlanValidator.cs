using FluentValidation;

namespace Coach.Application.Features.WorkoutPlans.Update;

public class UpdateWorkoutPlanValidator : AbstractValidator<UpdateWorkoutPlanRequest>
{
    public UpdateWorkoutPlanValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Workout plan ID is required");

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
