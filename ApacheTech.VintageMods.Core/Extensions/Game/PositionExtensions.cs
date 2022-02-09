using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class PositionExtensions
    {
        /// <summary>
        ///     Gets the position relative to spawn, given an absolute position within the game world.
        /// </summary>
        /// <param name="pos">The absolute position of the block being queried.</param>
        public static BlockPos RelativeToSpawn(this BlockPos pos)
        {
            var worldSpawn = ApiEx.Current.World.DefaultSpawnPosition.XYZ.AsBlockPos;
            var blockPos = pos.SubCopy(worldSpawn);
            return new BlockPos(blockPos.X, pos.Y, blockPos.Z);
        }

        /// <summary>
        ///     Generates a random position within a specified range of an origin position.
        /// </summary>
        /// <param name="origin">The origin position.</param>
        /// <param name="horizontalRadius">The radius away from the origin to use as the upper and lower bounds for the X and Z coordinates of the returned position.</param>
        /// <param name="verticalRadius">The radius away from the origin to use as the upper and lower bounds for the Y coordinates of the returned position.</param>
        /// <returns>A <see cref="Vec3d"/> representing a position in the game world, a random distance away from the origin position.</returns>
        public static Vec3d GetRandomPositionInRange(this Vec3d origin, int horizontalRadius, int verticalRadius = 0)
        {
            var x = RandomEx.RandomValueBetween(-horizontalRadius, horizontalRadius);
            var y = RandomEx.RandomValueBetween(-verticalRadius, verticalRadius);
            var z = RandomEx.RandomValueBetween(-horizontalRadius, horizontalRadius);
            return origin.AddCopy(x, y, z);
        }


        /// <summary>
        ///     Gets the surface level at the specified block position. This is the highest point on the Y axis, where the block is solid, from above.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns>An <see cref="int"/> value, representing the highest Y value with a solid block underneath it.</returns>
        public static int GetSurfaceLevel(this BlockPos pos)
        {
            var blockAccessor = ApiEx.Current.World.BlockAccessor;
            var y = blockAccessor.GetTerrainMapheightAt(pos);
            var minPos = new BlockPos(pos.X, 1, pos.Z);
            var maxPos = new BlockPos(pos.X, blockAccessor.MapSizeY, pos.Z);
            blockAccessor.WalkBlocks(minPos, maxPos, (block, blockPos) =>
            {
                if (block.SideSolid[BlockFacing.indexUP])
                {
                    y = blockPos.Y;
                }
            });
            return y;
        }

        /// <summary>
        ///     Checks to see whether the entity will collide with anything at a given position in the world.
        /// </summary>
        /// <param name="entity">The entity in question.</param>
        /// <param name="position">The position for which to check for collisions.</param>
        /// <returns>Returns <c>true</c>, if the entity will collide with something at the given position; otherwise, <c>false</c>.</returns>
        public static bool CollisionCheck(this Entity entity, Vec3d position)
        {
            return entity.World
                .CollisionTester
                .GetCollidingCollisionBox(entity.World.BlockAccessor, entity.CollisionBox, position, false) == null;
        }
    }
}
