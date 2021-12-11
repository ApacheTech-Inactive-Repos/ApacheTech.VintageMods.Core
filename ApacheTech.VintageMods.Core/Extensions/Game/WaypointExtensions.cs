using System;
using System.Linq;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.GameContent.AssetEnum;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class WaypointExtensions
    {
        /// <summary>
        ///     Adds a waypoint at the player's current position within the world, relative to the global spawn point.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="icon">The icon to use for the waypoint.</param>
        /// <param name="colour">The colour of the waypoint.</param>
        /// <param name="title">The title to set.</param>
        /// <param name="pinned">if set to <c>true</c>, the waypoint will be pinned to the world map.</param>
        public static void AddWaypointAtCurrentPos(
            this ICoreClientAPI api, string icon, string colour, string title, bool pinned)
        {
            api.AddWaypointAtPos(api.World?.Player?.Entity?.Pos.AsBlockPos.RelativeToSpawn(api.World), icon, colour,
                title, pinned);
        }

        /// <summary>
        ///     Adds a waypoint at the a position within the world, relative to the global spawn point.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="pos">The position to add the waypoint at.</param>
        /// <param name="icon">The icon to use for the waypoint.</param>
        /// <param name="colour">The colour of the waypoint.</param>
        /// <param name="title">The title to set.</param>
        /// <param name="pinned">if set to <c>true</c>, the waypoint will be pinned to the world map.</param>
        public static void AddWaypointAtPos(
            this ICoreClientAPI api, BlockPos pos, string icon, string colour, string title, bool pinned)
        {
            if (pos is null) return;
            api.SendChatMessage(
                $"/waypoint addati {icon} {pos.X} {pos.Y} {pos.Z} {(pinned ? "true" : "false")} {colour} {title}");
        }

        /// <summary>
        ///     Determines whether a waypoint already exists at a specific position on the world map.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="pos">The position to check.</param>
        /// <param name="filter">A custom filter, to narrow the scope of the search.</param>
        /// <returns><c>true</c> if a waypoint already exists at the specified position, <c>false</c> otherwise.</returns>
        public static bool WaypointExistsAtPos(this ICoreClientAPI api, BlockPos pos, Func<Waypoint, bool> filter = null)
        {
            var waypointMapLayer = api.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
            var waypoints = waypointMapLayer.ownWaypoints.Where(wp => wp.Position.AsBlockPos.Equals(pos)).ToList();
            if (!waypoints.Any()) return false;
            return filter == null || waypoints.Any(filter);
        }

        /// <summary>
        ///     Determines whether a waypoint already exists within a radius of a specific position on the world map.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="pos">The position to check.</param>
        /// <param name="horizontalRadius">The number of blocks away from the origin position to scan, on the X and Z axes.</param>
        /// <param name="verticalRadius">The number of blocks away from the origin position to scan, on the Y axis.</param>
        /// <param name="filter">A custom filter, to narrow the scope of the search.</param>
        /// <returns><c>true</c> if a waypoint already exists within range of the specified position, <c>false</c> otherwise.</returns>
        public static bool WaypointExistsWithinRadius(this ICoreClientAPI api, BlockPos pos, int horizontalRadius,
            int verticalRadius, Func<Waypoint, bool> filter = null)
        {
            var waypointMapLayer = api.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
            var waypoints =
                waypointMapLayer.ownWaypoints.Where(wp =>
                    wp.Position.AsBlockPos.InRangeCubic(pos, horizontalRadius, verticalRadius)).ToList();
            if (!waypoints.Any()) return false;
            return filter == null || waypoints.Any(filter);
        }

        public static void AddWaypointsForEndPoints(this BlockEntityStaticTranslocator translocator)
        {
            if (!translocator.FullyRepaired || !translocator.Activated) return;

            var toTitle = $"Translocator to ({translocator.TargetLocation.X}, {translocator.TargetLocation.Y}, {translocator.TargetLocation.Z})";
            ApiEx.Client.AddWaypointAtPos(translocator.Pos.RelativeToSpawn(), WaypointIcon.Spiral, NamedColour.MediumPurple, toTitle, false);

            var fromTitle = $"Translocator to ({translocator.Pos.X}, {translocator.Pos.Y}, {translocator.Pos.Z})";
            ApiEx.Client.AddWaypointAtPos(translocator.TargetLocation.RelativeToSpawn(), WaypointIcon.Spiral, NamedColour.MediumPurple, fromTitle, false);
        }
    }
}