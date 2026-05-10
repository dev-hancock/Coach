using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Register;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string Name) : IRequest<ErrorOr<RegisterResponse>>;