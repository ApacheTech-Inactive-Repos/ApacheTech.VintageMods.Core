using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Abstractions.Features
{
    /// <summary>
    ///     
    /// </summary>
    public interface IServerFeatureHandler
    {
        /// <summary>
        ///     Starts the theory on the server.
        /// </summary>
        /// <param name="sapi">The server-side API.</param>
        void StartServerSide(ICoreServerAPI sapi);
    }
}