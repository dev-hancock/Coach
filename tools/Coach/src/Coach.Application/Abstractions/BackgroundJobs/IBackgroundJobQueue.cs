namespace Coach.Application.Abstractions.BackgroundJobs;

/// <summary>
/// Port for queueing background work items.
/// Infrastructure layer provides channel-based or message queue implementation.
/// </summary>
public interface IBackgroundJobQueue
{
    /// <summary>
    /// Queue a background work item for execution.
    /// </summary>
    void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
}
