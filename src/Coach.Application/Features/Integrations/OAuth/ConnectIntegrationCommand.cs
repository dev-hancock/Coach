using Coach.Domain.Integrations;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Integrations.OAuth;

/// <summary>
/// Command to connect an OAuth-based integration provider for a user.
/// </summary>
public sealed record ConnectIntegrationCommand(
    Guid UserId,
    IntegrationType IntegrationType,
    string AuthorizationCode) : IRequest<ErrorOr<ConnectIntegrationResponse>>;

public sealed record ConnectOAuthCommand(
    Guid UserId,
    IntegrationType Type,
    string Code) : IRequest<ErrorOr<ConnectOAuthResponse>>;
