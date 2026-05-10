using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.WorkoutPlans.GetAll;

internal sealed class GetAllWorkoutPlansHandler 
    : IRequestHandler<GetAllWorkoutPlansRequest, ErrorOr<GetAllWorkoutPlansResponse>>
{
    private readonly IWorkoutPlanRepository _repository;

    public GetAllWorkoutPlansHandler(IWorkoutPlanRepository repository)
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
