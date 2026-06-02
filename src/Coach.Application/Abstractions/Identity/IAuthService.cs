using ErrorOr;

namespace Coach.Application.Abstractions.Identity;

/// <summary>
/// Interface for authentication operations (login, logout).
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with email and password.
    /// </summary>
    Task<ErrorOr<IUser>> LoginAsync(
        string email,
        string password,
        bool rememberMe = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Signs out the current user.
    /// </summary>
    Task<ErrorOr<Success>> LogoutAsync(
        CancellationToken cancellationToken = default);
}
