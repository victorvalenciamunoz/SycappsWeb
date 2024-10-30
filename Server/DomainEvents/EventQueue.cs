using System.Threading.Channels;

namespace SycappsWeb.Server;

public class EventQueue : IEventQueue
{
    private readonly Channel<DomainEvent> _queue;

    public EventQueue()
    {
        _queue = Channel.CreateUnbounded<DomainEvent>();
    }

    public async ValueTask QueueEventAsync<T>(T domainEvent) where T : DomainEvent
    {
        await _queue.Writer.WriteAsync(domainEvent);
    }

    public async ValueTask<T> DequeueEventAsync<T>(CancellationToken cancellationToken) where T : DomainEvent
    {
        var domainEvent = await _queue.Reader.ReadAsync(cancellationToken);
        return (T)domainEvent;
    }
}
