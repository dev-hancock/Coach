using Coach.Application.Abstractions.Identity;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Login;

internal sealed class LoginHandler(
    IAuthService auth,
    ITokenService tokens)
    : IRequestHandler<LoginRequest, ErrorOr<LoginResponse>>
{
    public async Task<ErrorOr<LoginResponse>> Handle(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        return await SignIn(
                request.Email,
                request.Password,
                request.RememberMe,
                cancellationToken)
            .ThenAsync(context =>
                CreateTokens(
                    context, 
                    cancellationToken))
            .Then(context =>
                new LoginResponse(
                    context.User.Id,
                    context.Tokens.AccessToken,
                    context.Tokens.RefreshToken,
                    context.Tokens.ExpiresAt));
    }

    private Task<ErrorOr<LoginContext>> SignIn(
        string email,
        string password,
        bool rememberMe,
        CancellationToken cancellationToken)
    {
        return auth
            .LoginAsync(email, password, rememberMe, cancellationToken)
            .Then(result => new LoginContext
            {
                User = result
            });
    }

    private Task<ErrorOr<LoginContext>> CreateTokens(
        LoginContext context,
        CancellationToken cancellationToken)
    {
        return tokens
            .CreateTokenAsync(context.User, cancellationToken)
            .Then(result => context with
            {
                Tokens = result
            });
    }

    private sealed record LoginContext
    {
        public IUser User { get; set; } = null!;

        public TokenResult Tokens { get; init; } = null!;
    }
}
