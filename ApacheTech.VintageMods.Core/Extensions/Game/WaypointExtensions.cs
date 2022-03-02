using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class WaypointExtensions
    {
        /// <summary>
        ///     Determines whether the specified waypoint is within a set horizontal distance of a specific block, or location within the game world.
        /// </summary>
        /// <param name="waypoint">The waypoint.</param>
        /// <param name="targetPosition">The target position.</param>
        /// <param name="squareDistance">The maximum square distance tolerance level.</param>
        /// <returns>System.Boolean.</returns>
        public static bool IsInHorizontalRangeOf(this Waypoint waypoint, BlockPos targetPosition, float squareDistance)
        {
            var waypointPos = waypoint.Position.AsBlockPos;
            var num = waypointPos.X - targetPosition.X;
            var num2 = waypointPos.Z - targetPosition.Z;
            var distance = num * num + num2 * num2;
            return distance <= squareDistance;
        }

        /// <summary>
        ///     Adds a waypoint at the a position within the world, relative to the global spawn point.
        /// </summary>
        /// <param name="pos">The position to add the waypoint at. World Pos - Not Relative to Spawn!</param>
        /// <param name="icon">The icon to use for the waypoint.</param>
        /// <param name="colour">The colour of the waypoint.</param>    
        /// <param name="title">The title to set.</param>
        /// <param name="pinned">if set to <c>true</c>, the waypoint will be pinned to the world map.</param>
        /// <param name="allowDuplicates">if set to <c>true</c>, the waypoint will not be placed, if another similar waypoint already exists at that position.</param>
        public static void AddWaypointAtPos(this BlockPos pos, string icon, string colour, string title, bool pinned, bool allowDuplicates = false)
        {
            if (pos is null) return;
            if (!allowDuplicates)
            {
                if (pos.WaypointExistsAtPos(p => p.Icon == icon)) return;
            }
            pos = pos.RelativeToSpawn();
            ApiEx.Client.SendChatMessage($"/waypoint addati {icon} {pos.X} {pos.Y} {pos.Z} {(pinned ? "true" : "false")} {colour} {title}");
        }

        /// <summary>
        ///     Asynchronously adds a waypoint at the a position within the world, relative to the global spawn point.
        /// </summary>
        /// <param name="pos">The position to add the waypoint at. World Pos - Not Relative to Spawn!</param>
        /// <param name="icon">The icon to use for the waypoint.</param>
        /// <param name="colour">The colour of the waypoint.</param>    
        /// <param name="title">The title to set.</param>
        /// <param name="pinned">if set to <c>true</c>, the waypoint will be pinned to the world map.</param>
        public static Task AddWaypointAtPosAsync(this BlockPos pos, string icon, string colour, string title, bool pinned)
        {
            return Task.Factory.StartNew(() => pos.AddWaypointAtPos(icon, colour, title, pinned));
        }

        /// <summary>
        ///     Determines whether a waypoint already exists within a radius of a specific position on the world map.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <param name="horizontalRadius">The number of blocks away from the origin position to scan, on the X and Z axes.</param>
        /// <param name="verticalRadius">The number of blocks away from the origin position to scan, on the Y axis.</param>
        /// <param name="filter">A custom filter, to narrow the scope of the search.</param>
        /// <returns><c>true</c> if a waypoint already exists within range of the specified position, <c>false</c> otherwise.</returns>
        public static bool WaypointExistsWithinRadius(this BlockPos position, int horizontalRadius, int verticalRadius, System.Func<Waypoint, bool> filter = null)
        {
            try
            {
                var waypointMapLayer = ApiEx.Client.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
                var waypoints =
                    waypointMapLayer.ownWaypoints.Where(wp => wp?.Position.AsBlockPos.InRangeCubic(position, horizontalRadius, verticalRadius) ?? false).ToList();
                if (!waypoints.Any()) return false;
                return filter == null || waypoints.Any(filter);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Asynchronously determines whether a waypoint already exists within a radius of a specific position on the world map.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <param name="horizontalRadius">The number of blocks away from the origin position to scan, on the X and Z axes.</param>
        /// <param name="verticalRadius">The number of blocks away from the origin position to scan, on the Y axis.</param>
        /// <param name="filter">A custom filter, to narrow the scope of the search.</param>
        /// <returns><c>true</c> if a waypoint already exists within range of the specified position, <c>false</c> otherwise.</returns>
        public static Task<bool> WaypointExistsWithinRadiusAsync(this BlockPos position, int horizontalRadius, int verticalRadius, System.Func<Waypoint, bool> filter = null)
        {
            return Task<bool>.Factory.StartNew(() => position.WaypointExistsWithinRadius(horizontalRadius, verticalRadius, filter));
        }

        /// <summary>
        ///     Determines whether a waypoint already exists at a specific position on the world map.
        /// </summary>
        /// <param name="pos">The position to check.</param>
        /// <param name="filter">A custom filter, to narrow the scope of the search.</param>
        /// <returns><c>true</c> if a waypoint already exists at the specified position, <c>false</c> otherwise.</returns>
        public static bool WaypointExistsAtPos(this BlockPos pos, System.Func<Waypoint, bool> filter = null)
        {
            var waypointMapLayer = ApiEx.Client.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
            var waypoints = waypointMapLayer.ownWaypoints.Where(wp => wp?.Position.AsBlockPos.Equals(pos) ?? false).ToList();
            if (!waypoints.Any()) return false;
            return filter == null || waypoints.Any(filter);
        }

        /// <summary>
        ///     Asynchronously determines whether a waypoint already exists at a specific position on the world map.
        /// </summary>
        /// <param name="pos">The position to check.</param>
        /// <param name="filter">A custom filter, to narrow the scope of the search.</param>
        /// <returns><c>true</c> if a waypoint already exists at the specified position, <c>false</c> otherwise.</returns>
        public static Task<bool> WaypointExistsAtPosAsync(this BlockPos pos, System.Func<Waypoint, bool> filter = null)
        {
            return Task<bool>.Factory.StartNew(() => pos.WaypointExistsAtPos(filter));
        }

        /// <summary>
        ///     The game does not implement any way of uniquely identifying waypoints, nor does it set waypoint objects as ValueTypes.
        ///     So this is a memberwise equality checker, to see if one waypoint is the same as another waypoint, when jumping through the numerous hoops required.
        ///     This method should not be needed... but here we are.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>System.Boolean.</returns>
        public static bool IsSameAs(this Waypoint source, Waypoint target)
        {
            if (source.Color != target.Color) return false;
            if (source.OwningPlayerGroupId != target.OwningPlayerGroupId) return false;
            if (source.Icon != target.Icon) return false;
            if (source.Position.X != target.Position.X) return false;
            if (source.Position.Y != target.Position.Y) return false;
            if (source.Position.Z != target.Position.Z) return false;
            if (source.Text != target.Text) return false;
            if (source.OwningPlayerUid != target.OwningPlayerUid) return false;
            if (source.Pinned != target.Pinned) return false;
            if (source.ShowInWorld != target.ShowInWorld) return false;
            if (source.Temporary != target.Temporary) return false;
            if (source.Title != target.Title) return false;
            return true;
        }

        public static async Task AddWaypointForBlockPosPairAsync(this KeyValuePair<BlockPos, Block> result, string icon, string colour)
        {
            var blockPos = result.Key;
            var block = result.Value;
            if (await blockPos.WaypointExistsWithinRadiusAsync(15, 256, p => p.Icon == icon)) return;
            var displayName = block.GetPlacedBlockName(ApiEx.ClientMain, blockPos);
            blockPos.AddWaypointAtPos(icon, colour, $"{displayName}, Y = {blockPos.Y}", true);
            Thread.Sleep(50);
        }

        public static void AddWaypointForBlockPosPair(this KeyValuePair<BlockPos, Block> result, string icon, string colour)
        {
            result.AddWaypointForBlockPosPairAsync(icon, colour).RunSynchronously();
        }
    }
}