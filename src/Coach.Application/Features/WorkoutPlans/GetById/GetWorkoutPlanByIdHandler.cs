using Ardalis.Specification;
using Coach.Domain.Entities;
using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.GetById;

internal sealed class GetWorkoutPlanByIdHandler 
    : IRequestHandler<GetWorkoutPlanByIdRequest, ErrorOr<GetWorkoutPlanByIdResponse>>
{
    private readonly IRepository<WorkoutPlan> _repository;

    public GetWorkoutPlanByIdHandler(IRepository<WorkoutPlan> repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<GetWorkoutPlanByIdResponse>> Handle(
        GetWorkoutPlanByIdRequest request, 
        CancellationToken cancellationToken)
    {
        var workoutPlan = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (workoutPlan is null)
        {
            return Error.NotFound(
                "WorkoutPlan.NotFound", 
                $"Workout plan with ID {request.Id} was not found");
        }

        return new GetWorkoutPlanByIdResponse(
            workoutPlan.Id,
            workoutPlan.Name,
            workoutPlan.Description,
            workoutPlan.CreatedAt,
            workoutPlan.UpdatedAt
        );
    }
}
