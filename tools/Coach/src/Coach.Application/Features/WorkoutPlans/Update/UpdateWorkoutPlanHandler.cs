using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.Update;

internal sealed class UpdateWorkoutPlanHandler 
    : IRequestHandler<UpdateWorkoutPlanRequest, ErrorOr<UpdateWorkoutPlanResponse>>
{
    private readonly IWorkoutPlanRepository _repository;

    public UpdateWorkoutPlanHandler(IWorkoutPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<UpdateWorkoutPlanResponse>> Handle(
        UpdateWorkoutPlanRequest request, 
        CancellationToken cancellationToken)
    {
        var workoutPlan = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (workoutPlan is null)
        {
            return Error.NotFound(
                "WorkoutPlan.NotFound", 
                $"Workout plan with ID {request.Id} was not found");
        }

        workoutPlan.Name = request.Name;
        workoutPlan.Description = request.Description;
        workoutPlan.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(workoutPlan, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new UpdateWorkoutPlanResponse(
            workoutPlan.Id,
            workoutPlan.Name,
            workoutPlan.Description,
            workoutPlan.UpdatedAt
        );
    }
}
