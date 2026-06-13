using System.Threading.Channels;
using Coach.Application.Abstractions;

namespace Coach.Infrastructure.BackgroundJobs;

/// <summary>
/// In-memory background job queue using System.Threading.Channels.
/// </summary>
public sealed class BackgroundJobClient : IBackgroundJobClient
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    public BackgroundJobClient(int capacity = 100)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    public async ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> workItem, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        await _queue.Writer.WriteAsync(workItem, cancellationToken);
    }

    internal async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
