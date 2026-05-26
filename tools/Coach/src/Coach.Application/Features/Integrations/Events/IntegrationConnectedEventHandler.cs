using Coach.Application.Abstractions.Integrations;
using Coach.Application.Common;
using Coach.Domain.Integrations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Coach.Application.Features.Integrations.Events;

/// <summary>
/// Handles the IntegrationConnectedDomainEvent by queuing a background job
/// to sync the athlete's activities from the integration provider.
/// </summary>
internal sealed class IntegrationConnectedEventHandler(
    IBackgroundJobClient jobClient,
    ILogger<IntegrationConnectedEventHandler> logger)
    : INotificationHandler<DomainEventNotification<IntegrationConnectedDomainEvent>>
{
    public async Task Handle(
        DomainEventNotification<IntegrationConnectedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        logger.LogInformation(
            "Integration connected for athlete {AthleteId}, queueing activity sync job",
            domainEvent.AthleteId);

        // Queue a background job to sync activities
        await jobClient.EnqueueAsync(async ct =>
        {
            // The background job server will resolve IIntegrationService from a new scope
            // and call SyncActivitiesAsync for this user/integration

            // TODO: Implement activity sync orchestration
            await Task.CompletedTask;
        }, cancellationToken);
    }
}
