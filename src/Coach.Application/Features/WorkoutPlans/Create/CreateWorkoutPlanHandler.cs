using Coach.Domain.Entities;
using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.Create;

internal sealed class CreateWorkoutPlanHandler 
    : IRequestHandler<CreateWorkoutPlanRequest, ErrorOr<CreateWorkoutPlanResponse>>
{
    private readonly IRepository<WorkoutPlan> _repository;

    public CreateWorkoutPlanHandler(IRepository<WorkoutPlan> repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<CreateWorkoutPlanResponse>> Handle(
        CreateWorkoutPlanRequest request, 
        CancellationToken cancellationToken)
    {
        var workoutPlan = new WorkoutPlan
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(workoutPlan, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new CreateWorkoutPlanResponse(
            workoutPlan.Id,
            workoutPlan.Name,
            workoutPlan.Description,
            workoutPlan.CreatedAt,
            workoutPlan.UpdatedAt
        );
    }
}
