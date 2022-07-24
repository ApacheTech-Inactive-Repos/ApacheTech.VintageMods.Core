using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    public interface IClientRequest<out T>
    {
        IServerResponse<T> Execute(ICoreClientAPI capi);
    }
}