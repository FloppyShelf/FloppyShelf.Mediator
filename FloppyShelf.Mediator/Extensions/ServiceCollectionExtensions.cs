using FloppyShelf.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace FloppyShelf.Mediator.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/> to register mediator services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the mediator services and automatically discovers request handlers in the specified assemblies and namespaces.
        /// </summary>
        /// <param name="services">The service collection to which mediator services will be added.</param>
        /// <param name="assemblies">The assemblies to scan for handler implementations. If null or empty, the calling assembly will be used.</param>
        /// <param name="namespaces">Optional namespaces to filter handlers. If null or empty, all handlers will be registered.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddMediator(this IServiceCollection services, Assembly[] assemblies, string[] namespaces)
        {
            // Fallback to the calling assembly if no assemblies are specified
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            // Fallback to an empty array if no namespaces are specified
            if (namespaces is null)
            {
                namespaces = new string[0];
            }

            // Register the Sender service
            services.AddScoped<ISender, Sender>();

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
}
