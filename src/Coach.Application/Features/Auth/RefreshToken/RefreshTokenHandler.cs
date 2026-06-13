using Coach.Application.Abstractions.Identity;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.RefreshToken;

internal sealed class RefreshTokenHandler(ITokenService tokens)
    : IRequestHandler<RefreshTokenRequest, ErrorOr<RefreshTokenResponse>>
{
    public async Task<ErrorOr<RefreshTokenResponse>> Handle(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        return await tokens.RefreshTokenAsync(
                request.RefreshToken,
                cancellationToken)
            .Then(context =>
                new RefreshTokenResponse(
                    context.AccessToken,
                    context.RefreshToken,
                    context.ExpiresAt));
    }
}
