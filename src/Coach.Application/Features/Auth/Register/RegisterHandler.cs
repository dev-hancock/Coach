using Coach.Application.Abstractions.Identity;
using Coach.Domain.Athletes;
using Coach.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Auth.Register;

internal sealed class RegisterHandler(
    IUserService users,
    IAuthService auth,
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
            .ThenAsync(user => 
                CreateAthlete(
                    user, 
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
                    cancellationToken))
            .Then(context => 
                new RegisterResponse(
                    context.UserId, 
                    context.AthleteId, 
                    request.Email));
    }

    private Task<ErrorOr<IUser>> CreateUser(string email, string password, CancellationToken cancellationToken)
    {
        return users.CreateUserAsync(email, password, cancellationToken);
    }

    private async Task<ErrorOr<RegisterContext>> CreateAthlete(
        IUser user,
        string name,
        CancellationToken cancellationToken)
    {
        var athlete = new Athlete(user.Id, name);

        await athletes.AddAsync(athlete, cancellationToken);
        await athletes.SaveChangesAsync(cancellationToken);

        return new RegisterContext(user.Id, athlete.Id);
    }

    private async Task<ErrorOr<RegisterContext>> LinkToAthlete(
        RegisterContext context,
        CancellationToken cancellationToken)
    {
        var result = await users.LinkToAthleteAsync(context.UserId, context.AthleteId, cancellationToken);

        return result.IsError ? result.Errors : context;
    }

    private async Task<ErrorOr<RegisterContext>> SignIn(
        RegisterContext context,
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        var result = await auth.LoginAsync(email, password, rememberMe: true, cancellationToken);

        return result.IsError ? result.Errors : context;
    }

    private sealed record RegisterContext(Guid UserId, Guid AthleteId);
}
