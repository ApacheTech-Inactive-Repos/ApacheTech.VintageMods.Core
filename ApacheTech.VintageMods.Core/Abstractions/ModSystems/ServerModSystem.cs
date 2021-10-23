using ApacheTech.VintageMods.Core.Abstractions.ModSystems.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Abstractions.ModSystems
{
    /// <summary>
    ///     Acts as a base class for Server-Side Only ModSystems. Derived classes will only be loaded on the Server.
    /// </summary>
    /// <seealso cref="ModSystemBase{ICoreAPI}" />
    public abstract class ServerModSystem : ModSystemBase<ICoreServerAPI>
    {
        /// <summary>
        ///     Returns if this mod should be loaded for the given app side.
        /// </summary>
        /// <param name="forSide">For side.</param>
        /// <returns><c>true</c> if the mod should be loaded on the specified side, <c>false</c> otherwise.</returns>
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide.IsServer();
        }
    }
}