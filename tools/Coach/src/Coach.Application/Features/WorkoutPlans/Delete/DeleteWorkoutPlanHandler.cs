using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.Delete;

internal sealed class DeleteWorkoutPlanHandler 
    : IRequestHandler<DeleteWorkoutPlanRequest, ErrorOr<DeleteWorkoutPlanResponse>>
{
    private readonly IWorkoutPlanRepository _repository;

    public DeleteWorkoutPlanHandler(IWorkoutPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<DeleteWorkoutPlanResponse>> Handle(
        DeleteWorkoutPlanRequest request, 
        CancellationToken cancellationToken)
    {
        var workoutPlan = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (workoutPlan is null)
        {
            return Error.NotFound(
                "WorkoutPlan.NotFound", 
                $"Workout plan with ID {request.Id} was not found");
        }

        await _repository.DeleteAsync(workoutPlan, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new DeleteWorkoutPlanResponse(true);
    }
}
