using FloppyShelf.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace FloppyShelf.Mediator.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to register mediator services and their handlers.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers mediator services and automatically discovers and registers request handlers
    /// from the specified assemblies and optional namespaces.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add mediator services to.</param>
    /// <param name="assemblies">
    /// The assemblies to scan for handler implementations. 
    /// If <c>null</c> or empty, the calling assembly will be used.
    /// </param>
    /// <param name="namespaces">
    /// Optional namespaces to filter the discovered handlers. 
    /// If <c>null</c> or empty, all discovered handlers will be registered.
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    /// <remarks>
    /// Each <see cref="IRequestHandler{TRequest,TResponse}"/> implementation will be registered 
    /// as a scoped service against its corresponding interface.
    /// </remarks>

    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly[]? assemblies = null, string[]? namespaces = null)
    {
        // Fallback to the calling assembly if no assemblies are specified
        if (assemblies is null || assemblies.Length == 0)
        {
            assemblies = [Assembly.GetCallingAssembly()];
        }

        // Fallback to an empty array if no namespaces are specified
        namespaces ??= [];

        // Register the Sender service
        services.AddScoped<IMediator, Mediator>();

        var handlerInterfaceType = typeof(IRequestHandler<,>);

        // Scan all types in the given assemblies
        var handlerTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                .Where(i => namespaces.Length == 0 || namespaces.Contains(type.Namespace))
                .Select(i => new { Interface = i, Implementation = type })
            );

        // Register each handler implementation against its interface
        foreach (var handlerType in handlerTypes)
        {
            services.AddScoped(handlerType.Interface, handlerType.Implementation);
        }

        return services;
    }
}
