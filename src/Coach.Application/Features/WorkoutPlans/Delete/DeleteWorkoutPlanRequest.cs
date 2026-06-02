using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.Delete;

public record DeleteWorkoutPlanRequest(Guid Id) : IRequest<ErrorOr<DeleteWorkoutPlanResponse>>;
