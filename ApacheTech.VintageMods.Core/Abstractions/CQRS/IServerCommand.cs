using Vintagestory.API.Server;

// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    /// <summary>
    ///     Represents an action, to be performed on the server.
    /// </summary>
    public interface IServerCommand
    {
        /// <summary>
        ///     Executes the specified command. Run on the server.
        /// </summary>
        /// <param name="sapi">The server-side API.</param>
        void Execute(ICoreServerAPI sapi);
    }
}