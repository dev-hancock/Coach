using Coach.Application.Abstractions.Identity;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Logout;

internal sealed class LogoutHandler(ITokenService tokens)
    : IRequestHandler<LogoutRequest, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        LogoutRequest request,
        CancellationToken cancellationToken)
    {
        return await tokens.RevokeTokenAsync(
            request.RefreshToken,
            cancellationToken);
    }
}
