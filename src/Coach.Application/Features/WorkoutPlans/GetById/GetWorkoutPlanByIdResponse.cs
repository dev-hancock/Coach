namespace Coach.Application.Features.WorkoutPlans.GetById;

public record GetWorkoutPlanByIdResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
