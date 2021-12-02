using System.Collections.Generic;
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
            jsonPatcher.ApplyPatches(new List<JsonPatch>{patch});
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
                jsonPatcher.ApplyPatch(i, patch.File, patch, ref applied, ref notFound, ref errorCount);
            }
        }
    }
}