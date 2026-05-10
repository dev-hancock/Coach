using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.GetById;

public record GetAthleteByIdRequest(Guid Id) : IRequest<ErrorOr<GetAthleteByIdResponse>>;
