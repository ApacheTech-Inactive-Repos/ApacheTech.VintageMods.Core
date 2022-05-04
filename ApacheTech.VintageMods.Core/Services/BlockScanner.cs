using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Extensions.Game;
using System.Threading.Tasks;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace ApacheTech.VintageMods.Core.Services
{
    public class BlockScanner
    {
        private readonly IBlockAccessor _walker;
        private Dictionary<BlockPos, Block> _blocks;
        private string _code;
        private bool _matchDisplayedName;
        private readonly ICoreClientAPI _capi;

        public BlockScanner(ICoreClientAPI api)
        {
            _walker = (_capi = api).World.GetBlockAccessor(false, false, false);
        }

        public Dictionary<BlockPos, Block> ScanForBlocks(string code, BlockPos origin, int horizontalRadius, int verticalRadius, bool matchDisplayedName)
        {
            _matchDisplayedName = matchDisplayedName;
            _code = code.ToLowerInvariant();
            var worldHeight = ApiEx.ClientMain.GetField<ClientWorldMap>("WorldMap").MapSizeY;
            var minPos = new BlockPos(origin.X - horizontalRadius, Math.Max(origin.Y - verticalRadius, 1), origin.Z - horizontalRadius);
            var maxPos = new BlockPos(origin.X + horizontalRadius, Math.Min(origin.Y + verticalRadius, worldHeight), origin.Z + horizontalRadius);

            _blocks = new Dictionary<BlockPos, Block>();
            try
            {
                _walker.WalkBlocks(minPos, maxPos, OnBlock, true);
            }
            catch(NullReferenceException)
            {
                _capi.EnqueueShowChatMessage("Ran into unloaded chunks. Try a smaller radius, or generate all chunks within range.");
            }
            return _blocks;
        }

        private void OnBlock(Block block, BlockPos blockPos)
        {
            if (_blocks.ContainsKey(blockPos)) return;
            if (block.BlockMaterial is EnumBlockMaterial.Air or EnumBlockMaterial.Liquid) return;
            switch (_matchDisplayedName)
            {
                case false when !block.Code.ToString().ToLowerInvariant().Contains(_code):
                case true when !block.GetPlacedBlockName(ApiEx.ClientMain, blockPos).ToLowerInvariant().Contains(_code):
                    return;
                default:
                    _capi.EnqueueShowChatMessage($"Found At: {blockPos}");
                    _blocks.Add(blockPos.Copy(), block);
                    break;
            }
        }

        public Task<Dictionary<BlockPos, Block>> ScanForBlocksAsync(string code, BlockPos origin, int horizontalRadius, int verticalRadius, bool matchDisplayedName)
        {
            return Task.FromResult(ScanForBlocks(code, origin, horizontalRadius, verticalRadius, matchDisplayedName));
        }

        /// <summary>
        ///     Retrieves a list of all blocks of a certain type, in range of a player.
        /// </summary>
        /// <param name="args">The arguments, passed from a chat command.</param>
        /// <param name="matchDisplayedName"> Whether to match based on code, or display name.</param>
        /// <param name="all">Whether to mark all results, or just the first.</param>
        /// <example>
        ///     .probeAll broken 1000 128 red spiral
        ///     .probe magnetite 1000 128 white pick
        /// </example>
        public async Task BlockProbeAsync(CmdArgs args, bool matchDisplayedName, bool all)
        {
            var code = args.PopWord("broken");
            var origin = _capi.World.Player.Entity.Pos.AsBlockPos;
            var horizontalRadius = args.PopInt().GetValueOrDefault(128);
            var verticalRadius = args.PopInt().GetValueOrDefault(128);
            var colour = args.PopWord("red");
            var icon = args.PopWord("spiral");

            _capi.EnqueueShowChatMessage($"Probe: {code}, {horizontalRadius}x{verticalRadius}, {colour} {icon}");

            var results = (await ScanForBlocksAsync(code, origin, horizontalRadius, verticalRadius, matchDisplayedName)).ToList();

            var found = results.Any();

            _capi.EnqueueShowChatMessage($"Found: {found}");

            if (all)
            {
                const int limit = 10000;
                if (results.Count > limit) return;
                foreach (var r in results)
                {
                    await r.AddWaypointForBlockPosPairAsync(icon, colour);
                }
                return;
            }

            if (!found) return;
            var result = results.FirstOrDefault();
            await result.AddWaypointForBlockPosPairAsync(icon, colour);
        }
    }
}