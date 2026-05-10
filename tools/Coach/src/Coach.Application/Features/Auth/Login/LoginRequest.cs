using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Login;

public sealed record LoginRequest(
    string Email,
    string Password,
    bool RememberMe = false) : IRequest<ErrorOr<LoginResponse>>;