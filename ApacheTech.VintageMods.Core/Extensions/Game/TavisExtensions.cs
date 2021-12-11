using System.Collections.Generic;
using ApacheTech.Common.Extensions.Harmony;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.ServerMods.NoObf;

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    /// <summary>
    ///     Extension methods for Tavis JSON Patching Engine.
    /// </summary>
    public static class TavisExtensions
    {
        /// <summary>
        ///     Applies a single patch to a JSON file.
        /// </summary>
        /// <param name="jsonPatcher">The <see cref="ModJsonPatchLoader"/> ModSystem used to patch JSON files in the game.</param>
        /// <param name="patch">The patch to apply.</param>
        public static void ApplyPatch(this ModJsonPatchLoader jsonPatcher, JsonPatch patch)
        {
            jsonPatcher.ApplyPatches(new List<JsonPatch> { patch });
        }

        /// <summary>
        ///     Applies a number of patches to the JSON assets of the game.
        /// </summary>
        /// <param name="jsonPatcher">The <see cref="ModJsonPatchLoader"/> ModSystem used to patch JSON files in the game.</param>
        /// <param name="patches">The patches to apply.</param>
        public static void ApplyPatches(this ModJsonPatchLoader jsonPatcher, List<JsonPatch> patches)
        {
            for (var i = 0; i < patches.Count; i++)
            {
                var applied = 0;
                var notFound = 0;
                var errorCount = 0;
                var patch = patches[i];
                jsonPatcher.CallMethod("ApplyPatch", i, patch.File, patch, applied, notFound, errorCount);
            }
        }

        /// <summary>
        ///     Registers a BlockBehaviour with the API, and patches the JSON file to add the behaviour to the block.
        /// </summary>
        /// <typeparam name="TBlockBehaviour">The type of <see cref="BlockBehavior"/> to register.</typeparam>
        /// <param name="api">The API to register the <see cref="BlockBehavior"/> with.</param>
        /// <param name="fileAsset">The file to patch.</param>
        public static void PatchBlockBehaviour<TBlockBehaviour>(this ICoreAPI api, AssetLocation fileAsset)
            where TBlockBehaviour : BlockBehavior
        {
            api.RegisterBlockBehaviour<TBlockBehaviour>();
            var value = new KeyValuePair<string, string>("name", nameof(TBlockBehaviour));
            var json = JsonConvert.SerializeObject(value);
            api.ModLoader.GetModSystem<ModJsonPatchLoader>().ApplyPatch(new JsonPatch
            {
                Op = EnumJsonPatchOp.Add,
                File = fileAsset,
                Path = "/behaviors/-",
                Value = JsonObject.FromJson(json)
            });
        }
    }
}