namespace Coach.Application.Abstractions;

/// <summary>
/// Background job client for enqueueing async work items.
/// Implemented in Infrastructure layer using in-memory channels.
/// </summary>
public interface IBackgroundJobClient
{
    /// <summary>
    /// Enqueues a background job for execution.
    /// </summary>
    ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> workItem, CancellationToken cancellationToken = default);
}
