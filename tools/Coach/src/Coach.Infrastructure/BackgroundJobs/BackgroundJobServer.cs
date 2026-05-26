using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Coach.Infrastructure.BackgroundJobs;

/// <summary>
/// Background job server that processes enqueued jobs.
/// Runs as a hosted service and continuously dequeues and executes work items.
/// </summary>
public sealed class BackgroundJobServer(
    BackgroundJobClient jobClient,
    IServiceProvider serviceProvider,
    ILogger<BackgroundJobServer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Background job server started");

        await ProcessJobsAsync(stoppingToken);
    }

    private async Task ProcessJobsAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var workItem = await jobClient.DequeueAsync(stoppingToken);

                // Execute job in a new scope to support scoped services
                await using var scope = serviceProvider.CreateAsyncScope();

                await workItem(stoppingToken);

                logger.LogDebug("Background job executed successfully");
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Expected during shutdown
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing background job");
            }
        }

        logger.LogInformation("Background job server stopped");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Background job server is stopping");
        await base.StopAsync(cancellationToken);
    }
}
