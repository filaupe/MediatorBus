using MediatorBus.Interfaces;

namespace MediatorBus.Sample;

public class SamplePublisherService(IMediator mediator)
{
    public async Task PublishMessageAsync(string message)
    {
        var evt = new SampleEvent(message);
        await mediator.Publish(evt);
    }
}