namespace SycappsWeb.Server;

public class EventService : IEventService
{
    private readonly IEventQueue _eventQueue;

    public EventService(IEventQueue eventQueue)
    {
        _eventQueue = eventQueue;
    }

    public async Task Publish<T>(T domainEvent) where T : DomainEvent
    {
        await _eventQueue.QueueEventAsync(domainEvent);
    }
}