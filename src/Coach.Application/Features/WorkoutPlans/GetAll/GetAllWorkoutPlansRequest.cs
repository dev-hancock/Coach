using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.GetAll;

public record GetAllWorkoutPlansRequest() : IRequest<ErrorOr<GetAllWorkoutPlansResponse>>;
