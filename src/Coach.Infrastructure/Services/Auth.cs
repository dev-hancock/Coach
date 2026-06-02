using System.Security.Claims;
using Coach.Application.Abstractions.Identity;
using Coach.Infrastructure.Identity;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Coach.Infrastructure.Services;

/// <summary>
/// Infrastructure implementation of authentication service using ASP.NET Core Identity.
/// </summary>
public sealed class Auth : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;

    public Auth(
        UserManager<User> userManager,
        IUserClaimsPrincipalFactory<User> claimsPrincipalFactory)
    {
        _userManager = userManager;
        _claimsPrincipalFactory = claimsPrincipalFactory;
    }

    public async Task<ErrorOr<IUser>> LoginAsync(
        string email,
        string password,
        bool rememberMe = false,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Error.Validation("Auth.InvalidCredentials", "Invalid email or password.");
        }

        // Verify password
        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
        {
            // Record failed attempt for lockout
            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
                return Error.Validation("Auth.LockedOut", "Account is locked out.");
            }

            return Error.Validation("Auth.InvalidCredentials", "Invalid email or password.");
        }

        // Reset access failed count on successful login
        await _userManager.ResetAccessFailedCountAsync(user);

        // Note: Actual sign-in will be handled by cookie middleware via the claims principal
        // The API layer will need to call SignInAsync if using cookie authentication

        return user;
    }

    public async Task<ErrorOr<Success>> LogoutAsync(CancellationToken cancellationToken = default)
    {
        // Note: Actual sign-out should be handled by the API layer calling HttpContext.SignOutAsync
        // This is kept minimal to avoid Infrastructure depending on HttpContext

        return await Task.FromResult(Result.Success);
    }
}
