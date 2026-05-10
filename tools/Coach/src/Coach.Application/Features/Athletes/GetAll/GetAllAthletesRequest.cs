using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.GetAll;

public record GetAllAthletesRequest() : IRequest<ErrorOr<GetAllAthletesResponse>>;
