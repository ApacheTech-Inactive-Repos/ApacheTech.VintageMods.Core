using System.Linq;
using ApacheTech.Common.Extensions.Harmony;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    /// <summary>
    ///     Extension Methods for the World Map Manager.
    /// </summary>
    public static class WorldMapManagerExtensions
    {
        /// <summary>
        ///     Returns the map layer used for rendering waypoints.
        /// </summary>
        /// <param name="mapManager">The <see cref="WorldMapManager" /> instance that this method was called from.</param>
        public static WaypointMapLayer WaypointMapLayer(this WorldMapManager mapManager)
        {
            return mapManager.MapLayers.OfType<WaypointMapLayer>().First();
        }

        /// <summary>
        ///     Trick the server into sending waypoints to the client even if they don't have their map opened. <br/>
        ///     Credit to Novocain.
        /// </summary>
        /// <param name="mapManager">The map manager.</param>
        public static void ForceSendWaypoints(this WorldMapManager mapManager)
        {
            var capi = mapManager.GetField<ICoreClientAPI>("capi");
            capi.Event.EnqueueMainThreadTask(() =>
                capi.Event.RegisterCallback(_ =>
                    mapManager.GetField<IClientNetworkChannel>("clientChannel")
                        .SendPacket(new OnViewChangedPacket()), 500), "");
        }
    }
}