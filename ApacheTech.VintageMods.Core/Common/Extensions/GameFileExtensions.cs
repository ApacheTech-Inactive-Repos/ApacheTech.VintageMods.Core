using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using JetBrains.Annotations;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace ApacheTech.VintageMods.Core.Common.Extensions
{
    [UsedImplicitly]
    public static class GameFileExtensions
    {
        /// <summary>
        ///     Returns all remaining arguments as single merged string, concatenated with spaces.
        /// </summary>
        /// <param name="args">The CmdArgs instance that called this extension method.</param>
        /// <param name="defaultValue">The default value to use, if nothing remains within the argument buffer.</param>
        public static string PopAll(this CmdArgs args, string defaultValue)
        {
            var retVal = args.PopAll();
            return string.IsNullOrWhiteSpace(retVal) ? defaultValue : retVal;
        }
        
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
