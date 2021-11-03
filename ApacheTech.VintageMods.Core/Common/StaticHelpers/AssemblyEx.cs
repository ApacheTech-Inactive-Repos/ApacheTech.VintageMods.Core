using System.Reflection;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    public static class AssemblyEx
    {
        internal static Assembly ModAssembly { private get; set; }

        /// <summary>
        ///     Ensures that the correct Mod assembly is retrieved, when the core is not merged into mod assembly.
        /// </summary>
        /// <returns>The <see cref="Assembly"/> that contains the mod's entry point.</returns>
        public static Assembly GetModAssembly()
        {
            return ModAssembly;
        }

        /// <summary>
        ///     Ensures that the correct Core assembly is retrieved, when the core is not merged into mod assembly.
        /// </summary>
        /// <returns>The <see cref="Assembly"/> that contains the core's entry point.</returns>
        public static Assembly GetCoreAssembly()
        {
            return typeof(ApiEx).Assembly;
        }
    }
}