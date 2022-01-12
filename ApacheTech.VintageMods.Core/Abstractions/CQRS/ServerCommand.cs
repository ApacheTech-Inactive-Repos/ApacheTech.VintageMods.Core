using Vintagestory.API.Server;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    public abstract class ServerCommand : IServerCommand
    {
        public abstract void Execute(IServerAPI sapi);
    }
}