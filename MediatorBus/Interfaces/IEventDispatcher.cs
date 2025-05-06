namespace MediatorBus.Interfaces;

public delegate Task AsyncEventHandler<TEvent>(object sender, TEvent evt);

public interface IEventDispatcher<TEvent>
{
    event EventHandler<TEvent>? OnPublished;
    event AsyncEventHandler<TEvent>? OnPublishedAsync;

    void Publish(TEvent evt);
    Task PublishAsync(TEvent evt);
}
