using Coach.Application.Abstractions.Integrations;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Integrations.OAuth;

/// <summary>
/// Thin handler that delegates to provider-specific IIntegrationService.
/// </summary>
internal sealed class ConnectOAuthHandler(
    IEnumerable<IIntegrationService> integrationServices)
    : IRequestHandler<ConnectOAuthCommand, ErrorOr<ConnectOAuthResponse>>
{
    public async Task<ErrorOr<ConnectOAuthResponse>> Handle(
        ConnectOAuthCommand request,
        CancellationToken cancellationToken)
    {
        var service = integrationServices.FirstOrDefault(s => s.Type == request.Type);

        if (service is null)
        {
            return Error.NotFound(
                "Integration.ProviderNotSupported",
                $"Integration provider '{request.Type}' is not supported");
        }

        var result = await service.ConnectAsync(
            request.UserId,
            request.Code,
            cancellationToken);

        return result.Then(r => new ConnectOAuthResponse(
            Type: service.Type,
            ExternalAthleteId: r.ExternalId,
            AthleteName: "Athlete", // TODO: Get from service if needed
            ConnectedAt: r.ConnectedAt,
            AthleteId: r.AthleteId));
    }
}
