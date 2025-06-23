namespace FloppyShelf.Mediator.Interfaces;

/// <summary>
/// Defines a handler for a specific request type <typeparamref name="TRequest"/> that produces a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the incoming request and returns a response asynchronously.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">Token to observe cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing the response.</returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
