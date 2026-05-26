using Coach.Application.Abstractions.Integrations;
using Coach.Domain.Integrations;
using Coach.Infrastructure.Data;
using Coach.Infrastructure.Identity;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Coach.Infrastructure.Services;

/// <summary>
/// Infrastructure implementation of integration connection repository.
/// Provider-agnostic — only handles persistence of connection data.
/// </summary>
public sealed class IntegrationConnectionRepository(AthleteDbContext context) : IIntegrationConnectionRepository
{
    public async Task<ErrorOr<Success>> SaveAsync(
        Guid userId,
        IntegrationType type,
        Connection connection,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.Integrations)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        var existing = user.Integrations.FirstOrDefault(i => i.Type == type);

        if (existing is not null)
        {
            existing.ExternalId = connection.ExternalId;
            existing.AccessToken = connection.AccessToken;
            existing.RefreshToken = connection.RefreshToken;
            existing.TokenExpiresAt = connection.ExpiresAt.DateTime;
            existing.LastSyncedAt = connection.LastSyncedAt?.DateTime;
        }
        else
        {
            user.Integrations.Add(new Integration
            {
                UserId = userId,
                Type = type,
                ExternalId = connection.ExternalId,
                AccessToken = connection.AccessToken,
                RefreshToken = connection.RefreshToken,
                TokenExpiresAt = connection.ExpiresAt.DateTime,
                ConnectedAt = connection.ConnectedAt.DateTime
            });
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    public async Task<ErrorOr<Connection>> GetAsync(
        Guid userId,
        IntegrationType type,
        CancellationToken cancellationToken = default)
    {
        var connection = await context.Integrations
            .FirstOrDefaultAsync(
                ic => ic.UserId == userId && ic.Type == type,
                cancellationToken);

        if (connection is null)
        {
            return Error.NotFound(
                $"{type}.NotConnected",
                $"{type} account not connected.");
        }

        return new Connection(
            connection.ExternalId,
            connection.AccessToken,
            connection.RefreshToken,
            new DateTimeOffset(connection.TokenExpiresAt, TimeSpan.Zero),
            new DateTimeOffset(connection.ConnectedAt, TimeSpan.Zero),
            connection.LastSyncedAt.HasValue ? new DateTimeOffset(connection.LastSyncedAt.Value, TimeSpan.Zero) : null);
    }

    public async Task<ErrorOr<Success>> RemoveAsync(
        Guid userId,
        IntegrationType type,
        CancellationToken cancellationToken = default)
    {
        var connection = await context.Integrations
            .FirstOrDefaultAsync(
                i => i.UserId == userId && i.Type == type,
                cancellationToken);

        if (connection is null)
        {
            return Error.NotFound("Integration.NotConnected", "Account not connected.");
        }

        context.Integrations.Remove(connection);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
