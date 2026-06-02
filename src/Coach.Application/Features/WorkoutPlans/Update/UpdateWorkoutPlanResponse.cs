namespace Coach.Application.Features.WorkoutPlans.Update;

public record UpdateWorkoutPlanResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTime UpdatedAt
);
