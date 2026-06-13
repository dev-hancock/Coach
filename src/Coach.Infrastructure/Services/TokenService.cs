using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Coach.Application.Abstractions.Identity;
using Coach.Infrastructure.Data;
using Coach.Infrastructure.Identity;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Coach.Infrastructure.Services;

/// <summary>
/// Service for generating and managing JWT tokens.
/// Uses JsonWebTokenHandler (modern handler recommended by Microsoft).
/// </summary>
public sealed class TokenService(IOptions<JwtSettings> settings, AthleteDbContext context) : ITokenService
{
    private readonly JwtSettings _settings = settings.Value;

    public async Task<ErrorOr<TokenResult>> CreateTokenAsync(
        IUser user,
        CancellationToken cancellationToken = default)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

        var accessToken = CreateAccessToken(user, expiresAt);

        var refreshToken = GenerateRefreshToken();

        // Store refresh token in database
        var entity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpiry),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false
        };

        context.RefreshTokens.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return new TokenResult(accessToken, refreshToken, expiresAt);
    }

    public async Task<ErrorOr<TokenResult>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var storedToken = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

        if (storedToken is null)
        {
            return Error.Validation("Auth.InvalidRefreshToken", "Invalid refresh token.");
        }

        if (storedToken.IsRevoked)
        {
            return Error.Validation("Auth.RevokedRefreshToken", "Refresh token has been revoked.");
        }

        if (storedToken.ExpiresAt < DateTime.UtcNow)
        {
            return Error.Validation("Auth.ExpiredRefreshToken", "Refresh token has expired.");
        }

        storedToken.IsRevoked = true;
        storedToken.RevokedAt = DateTime.UtcNow;

        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);
        var newAccessToken = CreateAccessToken(storedToken.User, expiresAt);
        var newRefreshToken = GenerateRefreshToken();

        var entity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = storedToken.UserId,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpiry),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false
        };

        context.RefreshTokens.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return new TokenResult(newAccessToken, newRefreshToken, expiresAt);
    }

    public async Task<ErrorOr<Success>> RevokeTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var storedToken = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

        if (storedToken is null)
        {
            return Error.Validation("Auth.InvalidRefreshToken", "Invalid refresh token.");
        }

        storedToken.IsRevoked = true;
        storedToken.RevokedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    private string CreateAccessToken(IUser user, DateTime expiresAt)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("athleteId", user.AthleteId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(descriptor);

        return token;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}
