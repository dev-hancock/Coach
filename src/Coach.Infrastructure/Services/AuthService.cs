using Coach.Application.Abstractions.Identity;
using Coach.Infrastructure.Identity;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Coach.Infrastructure.Services;

/// <summary>
/// Infrastructure implementation of authentication service using ASP.NET Core Identity.
/// </summary>
public sealed class AuthService(UserManager<User> users) : IAuthService
{
    public async Task<ErrorOr<IUser>> LoginAsync(
        string email,
        string password,
        bool rememberMe = false,
        CancellationToken cancellationToken = default)
    {
        var user = await users.FindByEmailAsync(email);
        if (user is null)
        {
            return Error.Validation("Auth.InvalidCredentials", "Invalid email or password.");
        }

        if (await users.IsLockedOutAsync(user))
        {
            return Error.Validation("Auth.LockedOut", "Account is locked out.");
        }

        var valid = await users.CheckPasswordAsync(user, password);
        if (!valid)
        {
            await users.AccessFailedAsync(user);

            return Error.Validation("Auth.InvalidCredentials", "Invalid email or password.");
        }

        await users.ResetAccessFailedCountAsync(user);

        return user;
    }
}
