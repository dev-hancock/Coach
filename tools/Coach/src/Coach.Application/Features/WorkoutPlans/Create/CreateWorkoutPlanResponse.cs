namespace Coach.Application.Features.WorkoutPlans.Create;

public record CreateWorkoutPlanResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
