using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Logout;

public sealed record LogoutRequest(string RefreshToken) : IRequest<ErrorOr<Success>>;