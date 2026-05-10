using Coach.Application.Abstractions.Identity;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Logout;

public sealed record LogoutRequest : IRequest<ErrorOr<Success>>;

internal sealed class LogoutHandler(IAuthenticationService auth)
    : IRequestHandler<LogoutRequest, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        LogoutRequest request,
        CancellationToken cancellationToken)
    {
        return await auth.LogoutAsync(cancellationToken);
    }
}
