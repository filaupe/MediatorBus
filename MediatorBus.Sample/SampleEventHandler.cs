using MediatorBus.Interfaces;

namespace MediatorBus.Sample;

public class SampleEventHandler(IEventDispatcher<SampleEvent> dispatcher) : INotificationHandler<SampleEvent>
{
    public async Task Handle(SampleEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handler recebeu o evento: {notification.Message}");
        // Repasse para o dispatcher
        await dispatcher.PublishAsync(notification);
    }
}