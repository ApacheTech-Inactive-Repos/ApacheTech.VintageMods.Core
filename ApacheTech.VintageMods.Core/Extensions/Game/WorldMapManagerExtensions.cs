using System;
using System.Linq;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
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
        /// <param name="mapManager">The <see cref="WorldMapManager" /> instance that this method was called from.</param>
        public static void ForceSendWaypoints(this WorldMapManager mapManager)
        {
            var capi = mapManager.GetField<ICoreClientAPI>("capi");
            capi.Event.EnqueueMainThreadTask(() =>
                capi.Event.RegisterCallback(_ =>
                    mapManager.GetField<IClientNetworkChannel>("clientChannel")
                        .SendPacket(new OnViewChangedPacket()), 500), "");
        }

        /// <summary>
        ///     Re-centres the map on a specific position.
        /// </summary>
        /// <param name="mapManager">The <see cref="WorldMapManager" /> instance that this method was called from.</param>
        /// <param name="pos">The position to re-centre the map on.</param>
        public static void RecentreMap(this WorldMapManager mapManager, Vec3d pos)
        {
            try
            {
                var map = mapManager.worldMapDlg;
                UpdateMapGui(map.GetField<GuiComposer>("fullDialog"), pos);
                UpdateMapGui(map.GetField<GuiComposer>("hudDialog"), pos);
            }
            catch (Exception ex)
            {
                ApiEx.Current.Logger.Error(ex.Message);
                ApiEx.Current.Logger.Error(ex.StackTrace);
            }
        }

        private static void UpdateMapGui(GuiComposer composer, Vec3d pos)
        {
            var map = (GuiElementMap)composer.GetElement("mapElem");
            map.CurrentBlockViewBounds.X1 = pos.X - map.Bounds.InnerWidth / 2.0 / map.ZoomLevel;
            map.CurrentBlockViewBounds.Z1 = pos.Z - map.Bounds.InnerHeight / 2.0 / map.ZoomLevel;
            map.CurrentBlockViewBounds.X2 = pos.X + map.Bounds.InnerWidth / 2.0 / map.ZoomLevel;
            map.CurrentBlockViewBounds.Z2 = pos.Z + map.Bounds.InnerHeight / 2.0 / map.ZoomLevel;
            map.EnsureMapFullyLoaded();
        }
    }
}