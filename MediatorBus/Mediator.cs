namespace MediatorBus;

using MediatorBus;
using MediatorBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => InvokeHandlerAsync<TResponse>(request, cancellationToken);

    public Task Send(IRequest request, CancellationToken cancellationToken = default)
        => InvokeHandlerAsync<object>(request, cancellationToken);

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var handlers = serviceProvider.GetServices<INotificationHandler<TNotification>>() ?? Enumerable.Empty<INotificationHandler<TNotification>>();
        foreach (var handler in handlers)
            await handler.Handle(notification, cancellationToken);
    }

    private Task<TResponse> InvokeHandlerAsync<TResponse>(object request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerInterface = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        dynamic handler = serviceProvider.GetService(handlerInterface)
            ?? throw new InvalidOperationException($"Handler não encontrado para {requestType.Name}");

        return handler.Handle((dynamic)request, cancellationToken);
    }
}
