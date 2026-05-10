using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Coach.Infrastructure.Identity;

/// <summary>
/// Custom claims principal factory that adds AthleteId claim.
/// </summary>
public class UserClaimsPrincipalFactory(
    UserManager<User> userManager,
    IOptions<IdentityOptions> options)
    : UserClaimsPrincipalFactory<User>(userManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        if (user.AthleteId != Guid.Empty)
        {
            identity.AddClaim(new Claim("AthleteId", user.AthleteId.ToString()));
        }

        return identity;
    }
}
