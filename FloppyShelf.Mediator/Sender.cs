using FloppyShelf.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FloppyShelf.Mediator
{
    /// <summary>
    /// Default implementation of <see cref="ISender"/> that dispatches requests to their corresponding handlers.
    /// </summary>
    public class Sender : ISender
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sender"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve handlers.</param>
        public Sender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            // Create the expected handler type dynamically based on the request type and response type
            var handleType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            // Resolve the handler instance from the service provider
            dynamic handler = _serviceProvider.GetRequiredService(handleType);

            // Forward the request to the handler asynchronously
            return await handler.HandleAsync(request, cancellationToken);
        }
    }
}
