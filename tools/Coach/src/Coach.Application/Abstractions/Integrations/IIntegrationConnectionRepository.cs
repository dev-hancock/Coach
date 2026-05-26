using Coach.Application.Abstractions.Integrations;
using Coach.Domain.Integrations;
using ErrorOr;

namespace Coach.Application.Abstractions.Integrations;

/// <summary>
/// Repository port for persisting integration connection data.
/// </summary>
public interface IIntegrationConnectionRepository
{
    Task<ErrorOr<Success>> SaveAsync(
        Guid userId,
        IntegrationType type,
        Connection connection,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<Connection>> GetAsync(
        Guid userId,
        IntegrationType type,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<Success>> RemoveAsync(
        Guid userId,
        IntegrationType type,
        CancellationToken cancellationToken = default);
}
