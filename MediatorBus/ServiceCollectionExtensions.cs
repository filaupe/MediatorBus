using System.Reflection;
using MediatorBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSingleton<IMediator, Mediator>();
        services.AddSingleton(typeof(IEventDispatcher<>), typeof(EventDispatcher<>));

        RegisterHandlers(services, assemblies);
        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract);

        foreach (var impl in types)
        {
            foreach (var iface in impl.GetInterfaces())
            {
                if (iface.IsGenericType &&
                    (iface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                     iface.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                {
                    services.AddTransient(iface, impl);
                }
            }
        }
    }
}
