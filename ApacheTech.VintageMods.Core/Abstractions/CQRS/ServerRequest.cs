using Vintagestory.API.Server;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    /// <summary>
    ///     Acts as a base class for requests that are send to the server, and require a strongly-typed response.
    /// </summary>
    /// <typeparam name="T">The type of response expected back from the request.</typeparam>
    /// <seealso cref="IServerRequest{T}" />
    public abstract class ServerRequest<T> : IServerRequest<T>
    {
        public abstract IServerResponse<T> Execute(IServerAPI sapi);
    }
}
