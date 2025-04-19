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

        /// <summary>
        /// Sends a request to its corresponding handler and returns the result asynchronously.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response expected from the handler.</typeparam>
        /// <param name="request">The request to be handled.</param>
        /// <param name="cancellationToken">The token to cancel the operation if necessary.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response from the handler.</returns>
        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            // Create the expected handler type dynamically based on the request type and response type
            var handleType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            // Resolve the handler instance from the service provider
            var handler = _serviceProvider.GetRequiredService(handleType);
            if(handler is null)
            {
                throw new InvalidOperationException($"Handler of requested type {request.GetType().Name} not found.");
            }

            // Find the HandleAsync method for the handler type
            var handleMethod = handleType.GetMethod("HandleAsync");
            if(handleMethod is null)
            {
                throw new InvalidOperationException($"Method HandleAsync method not found on handler type {handleType.Name}.");
            }

            // Invoke the HandleAsync method using reflection and await the result
            var result = handleMethod.Invoke(handler, new object[] { request, cancellationToken });

            // Ensure that the result is a Task<TResponse>
            if (result is Task<TResponse> responseTask)
            {
                return await responseTask;
            }
            else
            {
                throw new InvalidOperationException("Handler's HandleAsync method did not return a Task<TResponse>.");
            }
        }
    }
}
