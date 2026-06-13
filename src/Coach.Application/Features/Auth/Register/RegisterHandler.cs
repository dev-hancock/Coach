using Coach.Application.Abstractions.Identity;
using Coach.Domain.Athletes;
using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Register;

internal sealed class RegisterHandler(
        IUserService users,
        IAuthService auth,
        ITokenService tokens,
        IRepository<Athlete> athletes)
    : IRequestHandler<RegisterRequest, ErrorOr<RegisterResponse>>
{
    public async Task<ErrorOr<RegisterResponse>> Handle(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var exists = await users.GetUserByEmailAsync(request.Email, cancellationToken);

        if (!exists.IsError)
        {
            return Error.Conflict("Auth.DuplicateEmail", "Email is already registered.");
        }

        return await CreateUser(
                request.Email,
                request.Password,
                cancellationToken)
            .ThenAsync(context =>
                CreateAthlete(
                    context,
                    request.Name,
                    cancellationToken))
            .ThenAsync(context =>
                LinkToAthlete(
                    context,
                    cancellationToken))
            .ThenAsync(context =>
                SignIn(
                    context,
                    request.Email,
                    request.Password,
                    false,
                    cancellationToken))
            .ThenAsync(context =>
                CreateTokens(
                    context,
                    cancellationToken))
            .Then(context =>
                new RegisterResponse(
                    context.User.Id,
                    context.Tokens.AccessToken,
                    context.Tokens.RefreshToken,
                    context.Tokens.ExpiresAt));
    }

    private Task<ErrorOr<RegisterContext>> CreateUser(string email, string password, CancellationToken cancellationToken)
    {
        return users
            .CreateUserAsync(
                email,
                password,
                cancellationToken)
            .Then(result => new RegisterContext
            {
                User = result
            });
    }

    private async Task<ErrorOr<RegisterContext>> CreateAthlete(
        RegisterContext context,
        string name,
        CancellationToken cancellationToken)
    {
        var athlete = new Athlete(context.User.Id, name);

        await athletes.AddAsync(athlete, cancellationToken);
        await athletes.SaveChangesAsync(cancellationToken);

        return context;
    }

    private Task<ErrorOr<RegisterContext>> LinkToAthlete(
        RegisterContext context,
        CancellationToken cancellationToken)
    {
        return users.LinkToAthleteAsync(
                context.User.Id,
                context.User.AthleteId,
                cancellationToken)
            .Then(result => context with
            {
                User = result
            });
    }

    private Task<ErrorOr<RegisterContext>> SignIn(
        RegisterContext context,
        string email,
        string password,
        bool rememberMe,
        CancellationToken cancellationToken)
    {
        return auth.LoginAsync(
                email,
                password,
                rememberMe,
                cancellationToken)
            .Then(result => context with
            {
                User = result
            });
    }

    private Task<ErrorOr<RegisterContext>> CreateTokens(
        RegisterContext context,
        CancellationToken cancellationToken)
    {
        return tokens.CreateTokenAsync(
                context.User,
                cancellationToken)
            .Then(result => context with
            {
                Tokens = result
            });
    }

    private sealed record RegisterContext
    {
        public IUser User { get; init; } = null!;

        public TokenResult Tokens { get; init; } = null!;
    }
}
