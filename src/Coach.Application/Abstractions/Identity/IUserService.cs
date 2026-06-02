using ErrorOr;

namespace Coach.Application.Abstractions.Identity;

/// <summary>
/// Interface for user account management operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers a new user with the provided credentials.
    /// </summary>
    Task<ErrorOr<IUser>> CreateUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Links an existing user account to an athlete profile.
    /// </summary>
    Task<ErrorOr<Success>> LinkToAthleteAsync(
        Guid userId,
        Guid athleteId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets user by user ID.
    /// </summary>
    Task<ErrorOr<IUser>> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets user by email.
    /// </summary>
    Task<ErrorOr<IUser>> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a password meets the configured requirements.
    /// </summary>
    Task<ErrorOr<Success>> ValidateAsync(
        string password,
        CancellationToken cancellationToken = default);
}
