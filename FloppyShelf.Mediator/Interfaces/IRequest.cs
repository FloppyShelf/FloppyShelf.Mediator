namespace FloppyShelf.Mediator.Interfaces
{
    /// <summary>
    /// Represents a request that expects a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    public interface IRequest<TResponse>
    {
    }
}
