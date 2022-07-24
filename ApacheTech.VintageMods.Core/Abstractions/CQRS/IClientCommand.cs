using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    /// <summary>
    ///     Represents an action, to be performed on the server.
    /// </summary>
    public interface IClientCommand
    {
        /// <summary>
        ///     Executes the specified command. Run on the client.
        /// </summary>
        /// <param name="capi">The client-side API.</param>
        void Execute(ICoreClientAPI capi);
    }
}