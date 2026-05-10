using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.Update;

public record UpdateWorkoutPlanRequest(
    Guid Id,
    string Name,
    string? Description
) : IRequest<ErrorOr<UpdateWorkoutPlanResponse>>;
