namespace ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions
{
    /// <summary>
    ///     Represents a Harmony patch class that affects, or is affected by specific feature settings.
    /// </summary>
    /// <typeparam name="T">The settings file to use within the patches in this class.</typeparam>
    public abstract class FeaturePatch<T> where T: class, new()
    {
        protected static readonly T Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="FeaturePatch{T}"/> class.
        /// </summary>
        static FeaturePatch()
        {
            Settings = ModServices.IOC.Resolve<T>();
        }
    }
}