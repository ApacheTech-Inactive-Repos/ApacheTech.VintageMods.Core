using ApacheTech.VintageMods.Core.Abstractions.ModSystems.Generic;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.ModSystems
{
    /// <summary>
    ///     Acts as a base class for Universal Mod Systems, that work on both the Client, and Server.
    /// </summary>
    /// <seealso cref="ModSystemBase{ICoreAPI}" />
    public abstract class UniversalModSystem : ModSystemBase<ICoreAPI>
    {
        /// <summary>
        ///     Returns if this mod should be loaded for the given app side.
        /// </summary>
        /// <param name="forSide">For side.</param>
        /// <returns><c>true</c> if the mod should be loaded on the specified side, <c>false</c> otherwise.</returns>
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return true;
        }
    }
}