using System.Security.Claims;
using Coach.Application.Abstractions.Identity;

namespace Coach.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (id is null)
        {
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }

        return Guid.Parse(id);
    }

    public static Guid? GetAthleteId(this ClaimsPrincipal principal)
    {
        var id = principal.FindFirstValue("AthleteId");

        return id != null ? Guid.Parse(id) : null;
    }

    public static bool IsAuthenticated(this ClaimsPrincipal principal)
    {
        return principal.Identity?.IsAuthenticated ?? false;
    }

    public static UserContext GetUserContext(this ClaimsPrincipal principal)
    {
        return new UserContext(GetUserId(principal), GetAthleteId(principal));
    }
}
