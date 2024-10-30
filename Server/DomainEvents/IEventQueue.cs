namespace SycappsWeb.Server;

public interface IEventQueue
{
    ValueTask QueueEventAsync<T>(T domainEvent) where T : DomainEvent;
    ValueTask<T> DequeueEventAsync<T>(CancellationToken cancellationToken) where T : DomainEvent;
}
