namespace SycappsWeb.Server;

public interface IEventService
{
    Task Publish<T>(T domainEvent) where T : DomainEvent;
}
