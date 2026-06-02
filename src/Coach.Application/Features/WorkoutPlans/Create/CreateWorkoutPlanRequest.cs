using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.Create;

public record CreateWorkoutPlanRequest(
    string Name,
    string? Description
) : IRequest<ErrorOr<CreateWorkoutPlanResponse>>;
