// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    public class ServerResponse<T> : IServerResponse<T>
    {
        public ServerResponse()
        {
            
        }

        public ServerResponse(T response)
        {
            Response = response;
        }
 
        public T Response { get; }
    }
}