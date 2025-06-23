namespace FloppyShelf.Mediator.Interfaces;

/// <summary>
/// Defines a mediator responsible for sending requests to their corresponding handlers and returning responses.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a request to the appropriate handler and asynchronously retrieves the response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected from the handler.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous send operation. The task result contains the response.</returns>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
