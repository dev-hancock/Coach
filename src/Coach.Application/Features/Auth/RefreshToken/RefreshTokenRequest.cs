using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.RefreshToken;

public sealed record RefreshTokenRequest(string RefreshToken)
    : IRequest<ErrorOr<RefreshTokenResponse>>;
