using Coach.Application.Abstractions.Identity;
using Coach.Infrastructure.Identity;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Coach.Infrastructure.Services;

/// <summary>
/// Infrastructure implementation of user management service using ASP.NET Core Identity.
/// Maps between Infrastructure IdentityUser and Domain User.
/// </summary>
public sealed class UserService(UserManager<User> users) : IUserService
{
    public async Task<ErrorOr<IUser>> CreateUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            UserName = email,
            Email = email
        };

        var result = await users.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => Error.Validation($"Auth.{e.Code}", e.Description))
                .ToList();

            return errors;
        }

        return user;
    }

    public async Task<ErrorOr<IUser>> LinkToAthleteAsync(
        Guid userId,
        Guid athleteId,
        CancellationToken cancellationToken = default)
    {
        var user = await users.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        user.AthleteId = athleteId;

        var result = await users.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(e => Error.Failure($"User.{e.Code}", e.Description))
                .ToList();

            return errors;
        }

        return user;
    }

    public async Task<ErrorOr<IUser>> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await users.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        return user;
    }

    public async Task<ErrorOr<IUser>> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await users.FindByEmailAsync(email);

        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        return user;
    }

    public async Task<ErrorOr<Success>> ValidateAsync(
        string password,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<Error>();

        foreach (var validator in users.PasswordValidators)
        {
            var result = await validator.ValidateAsync(users, null!, password);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors
                    .Select(e => Error.Validation($"Password.{e.Code}", e.Description)));
            }
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return Result.Success;
    }
}
