using System.Collections.Generic;
using HarmonyLib;

namespace ApacheTech.VintageMods.Core.Services.HarmonyPatches
{
    /// <summary>
    ///     A service that provides methods of applying Harmony patches to the game. 
    /// </summary>
    internal class HarmonyPatchingService : IHarmonyPatchingService
    {
        private readonly Dictionary<string, Harmony> _instances = new();

        /// <summary>
        ///     Creates a new patch host, if one with the specified ID doesn't already exist.
        /// </summary>
        /// <param name="harmonyId">The identifier to use for the patch host.</param>
        /// <returns>A <see cref="Harmony" /> patch host.</returns>
        public Harmony CreateOrUseInstance(string harmonyId)
        {
            if (_instances.ContainsKey(harmonyId))
            {
                return _instances[harmonyId];
            }

            var harmony = new Harmony(harmonyId);
            _instances.Add(harmonyId, harmony);
            return harmony;
        }

        /// <summary>
        ///     Un-patches all methods, within all patch host instances being handled by this service.
        /// </summary>
        public void UnpatchAll()
        {
            foreach (var harmony in _instances)
            {
                harmony.Value.UnpatchAll(harmony.Key);
            }
        }
    }
}