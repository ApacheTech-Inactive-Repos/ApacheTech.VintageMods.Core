using System.Reflection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    public static class AssemblyEx
    {
        internal static Assembly ModAssembly { private get; set; }

        /// <summary>
        ///     Ensures that the correct assembly is retrieved, when the core is not merged into mod assembly.
        /// </summary>
        /// <returns>The <see cref="Assembly"/> that contains the mod's entry point.</returns>
        public static Assembly GetModAssembly()
        {
            return ModAssembly;
        }
    }
}