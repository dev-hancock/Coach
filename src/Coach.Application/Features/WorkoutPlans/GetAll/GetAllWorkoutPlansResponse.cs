namespace Coach.Application.Features.WorkoutPlans.GetAll;

public record GetAllWorkoutPlansResponse(List<WorkoutPlanDto> WorkoutPlans);

public record WorkoutPlanDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
