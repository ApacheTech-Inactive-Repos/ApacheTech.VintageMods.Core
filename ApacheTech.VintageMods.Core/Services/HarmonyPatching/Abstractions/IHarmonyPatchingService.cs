using HarmonyLib;

namespace ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions
{
    /// <summary>
    ///     Provides methods of applying Harmony patches to the game.
    /// </summary>
    public interface IHarmonyPatchingService
    {
        /// <summary>
        ///     By default, all annotated [HarmonyPatch] classes in the executing assembly will
        ///     be processed at launch. Manual patches can be processed later on at runtime.
        /// </summary>
        public void UseHarmony();


        /// <summary>
        ///     Gets the default harmony instance for the mod.
        /// </summary>
        /// <value>The default Harmony instance for the mod, with the mod assembly's full name as the instance ID.</value>
        public Harmony Default { get; }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public void Dispose();
    }
}