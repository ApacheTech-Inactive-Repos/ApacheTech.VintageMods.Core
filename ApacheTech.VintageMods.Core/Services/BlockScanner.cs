using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace ApacheTech.VintageMods.Core.Services
{
    public class BlockScanner
    {
        private readonly ICachingBlockAccessor _walker;

        public BlockScanner(ICoreAPI api)
        {
            _walker = api.World.GetCachingBlockAccessor(false, false);
            _walker.Begin();
        }

        public Dictionary<BlockPos, Block> ScanForBlocks(string code, BlockPos origin, int horizontalRadius, int verticalRadius)
        {
            var worldHeight = ApiEx.ClientMain.GetField<ClientWorldMap>("WorldMap").MapSizeY;
            var minPos = new BlockPos(origin.X - horizontalRadius, Math.Max(origin.Y - verticalRadius, 1), origin.Z - horizontalRadius);
            var maxPos = new BlockPos(origin.X + horizontalRadius, Math.Min(origin.Y + verticalRadius, worldHeight), origin.Z + horizontalRadius);

            var blocks = new Dictionary<BlockPos, Block>();
            _walker.WalkBlocks(minPos, maxPos, (block, blockPos) =>
            {
                if (blocks.ContainsKey(blockPos)) return;
                if (block.BlockMaterial is EnumBlockMaterial.Air or EnumBlockMaterial.Liquid) return;
                if (!block.Code.ToString().ToLowerInvariant().Contains(code.ToLowerInvariant())) return;
                blocks.Add(blockPos, block);
            }, true);
            return blocks;
        }

        public Task<Dictionary<BlockPos, Block>> ScanForBlocksAsync(string code, BlockPos origin, int horizontalRadius,
            int verticalRadius)
        {
            return Task.FromResult(ScanForBlocks(code, origin, horizontalRadius, verticalRadius));
        }
    }
}