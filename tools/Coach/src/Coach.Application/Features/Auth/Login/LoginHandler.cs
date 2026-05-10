using Coach.Application.Abstractions.Identity;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Login;

internal sealed class LoginHandler(IAuthenticationService auth)
    : IRequestHandler<LoginRequest, ErrorOr<LoginResponse>>
{
    public async Task<ErrorOr<LoginResponse>> Handle(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        return await SignIn(
                request.Email,
                request.Password,
                cancellationToken)
            .Then(user =>
                new LoginResponse(
                    user.Id,
                    user.AthleteId,
                    request.Email));
    }

    private Task<ErrorOr<IUser>> SignIn(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        return auth.LoginAsync(email, password, rememberMe: true, cancellationToken);
    }
}
