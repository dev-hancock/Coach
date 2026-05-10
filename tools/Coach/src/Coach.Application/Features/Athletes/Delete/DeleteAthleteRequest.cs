using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.Delete;

public record DeleteAthleteRequest(Guid Id) : IRequest<ErrorOr<DeleteAthleteResponse>>;
