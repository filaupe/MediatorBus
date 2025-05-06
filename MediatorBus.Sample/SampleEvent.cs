using MediatorBus.Interfaces;

namespace MediatorBus.Sample;

public record SampleEvent(string Message) : INotification;