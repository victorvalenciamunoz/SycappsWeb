namespace SycappsWeb.Server;

public interface IEventHandler<T>
{
    Task Handle(T domainEvent);
}
