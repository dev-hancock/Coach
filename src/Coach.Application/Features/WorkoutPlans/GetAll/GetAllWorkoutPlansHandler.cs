using Ardalis.Specification;
using Coach.Domain.Entities;
using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.GetAll;

internal sealed class GetAllWorkoutPlansHandler 
    : IRequestHandler<GetAllWorkoutPlansRequest, ErrorOr<GetAllWorkoutPlansResponse>>
{
    private readonly IRepository<WorkoutPlan> _repository;

    public GetAllWorkoutPlansHandler(IRepository<WorkoutPlan> repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<GetAllWorkoutPlansResponse>> Handle(
        GetAllWorkoutPlansRequest request, 
        CancellationToken cancellationToken)
    {
        var workoutPlans = await _repository.ListAsync(cancellationToken);

        var dtos = workoutPlans.Select(wp => new WorkoutPlanDto(
            wp.Id,
            wp.Name,
            wp.Description,
            wp.CreatedAt,
            wp.UpdatedAt
        )).ToList();

        return new GetAllWorkoutPlansResponse(dtos);
    }
}
