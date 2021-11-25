using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.Features
{
    /// <summary>
    ///     
    /// </summary>
    public interface IClientFeatureHandler
    {
        /// <summary>
        ///     Starts the theory on the client.
        /// </summary>
        /// <param name="capi">The client-side API.</param>
        void StartClientSide(ICoreClientAPI capi);
    }
}