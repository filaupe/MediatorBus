using MediatorBus.Interfaces;

namespace MediatorBus.Sample;

public class SampleListenerService
{
    public SampleListenerService(IEventDispatcher<SampleEvent> dispatcher)
    {
        dispatcher.OnPublished += OnEventReceived;
        dispatcher.OnPublishedAsync += OnEventReceivedAsync;
    }

    private static void OnEventReceived(object? sender, SampleEvent evt)
    {
        Console.WriteLine($"[ListenerService] Evento recebido (sync): {evt.Message}");
    }

    private Task OnEventReceivedAsync(object sender, SampleEvent evt)
    {
        Console.WriteLine($"[ListenerService] Evento recebido (async): {evt.Message}");
        return Task.CompletedTask;
    }
}