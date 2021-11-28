using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using JetBrains.Annotations;
using Vintagestory.API.MathTools;

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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
    }
}
