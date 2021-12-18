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
        ///     Disposes this instance.
        /// </summary>
        public void Dispose();
    }
}