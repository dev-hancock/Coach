using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.GetById;

public record GetWorkoutPlanByIdRequest(Guid Id) : IRequest<ErrorOr<GetWorkoutPlanByIdResponse>>;
