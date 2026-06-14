using Coach.Application.Abstractions.Identity;
using Coach.Domain.Integrations;
using Microsoft.AspNetCore.Identity;

namespace Coach.Infrastructure.Identity;

/// <summary>
/// Application user with authentication information.
/// Linked 1:1 to Athlete domain entity.
/// </summary>
public class User : IdentityUser<Guid>, IUser
{
    /// <summary>
    /// Reference to the athlete profile for this user.
    /// </summary>
    public Guid AthleteId { get; set; }

    /// <summary>
    /// Collection of external integration connections
    /// </summary>
    public ICollection<Integration> Integrations { get; set; } = [];

    /// <summary>
    /// Collection of refresh tokens for this user
    /// </summary>
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
