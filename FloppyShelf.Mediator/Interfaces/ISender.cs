using System.Threading;
using System.Threading.Tasks;

namespace FloppyShelf.Mediator.Interfaces
{
    /// <summary>
    /// Sends requests to their respective handlers and returns responses.
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Sends a request to the appropriate handler and asynchronously returns the response.
        /// </summary>
        /// <typeparam name="TResponse">The type of response expected from the request.</typeparam>
        /// <param name="request">The request instance.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, containing the response.</returns>
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
