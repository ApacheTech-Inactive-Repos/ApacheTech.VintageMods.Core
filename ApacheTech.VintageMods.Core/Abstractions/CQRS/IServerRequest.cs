using Vintagestory.API.Server;

// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    public interface IServerRequest<out T>
    {
        IServerResponse<T> Execute(ICoreServerAPI sapi);
    }
}