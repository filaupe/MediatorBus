using MediatorBus.Interfaces;

namespace MediatorBus;

public class EventDispatcher<TEvent> : IEventDispatcher<TEvent>
{
    public event EventHandler<TEvent>? OnPublished;
    public event AsyncEventHandler<TEvent>? OnPublishedAsync;

    // Invoke sync handlers
    public void Publish(TEvent evt) => OnPublished?.Invoke(this, evt);

    // Invoke async handlers (in parallel) and wait for completion
    public async Task PublishAsync(TEvent evt)
    {
        // Invoke sync first
        Publish(evt);

        if (OnPublishedAsync is not null)
        {
            // Capture invocation list
            var invocationList = OnPublishedAsync.GetInvocationList()
                .Cast<AsyncEventHandler<TEvent>>();

            // Start all handlers
            var tasks = invocationList
                .Select(handler => handler.Invoke(this, evt));

            // Await all
            await Task.WhenAll(tasks);
        }
    }
}
