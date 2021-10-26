using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ApacheTech.VintageMods.Core.Common.Extensions.System;
using HarmonyLib;
using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Direct access to the game's vanilla assemblies.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class GameAssemblies
    {
        /// <summary>
        ///     VSEssentials.dll
        /// </summary>
        public static Assembly VSEssentials => GetAssembly("VSEssentials.dll");

        /// <summary>
        ///     VSSurvivalMod.dll
        /// </summary>
        public static Assembly VSSurvivalMod => GetAssembly("VSSurvivalMod.dll");

        /// <summary>
        ///     VSCreativeMod.dll
        /// </summary>
        public static Assembly VSCreativeMod => GetAssembly("VSCreativeMod.dll");

        /// <summary>
        ///     VintagestoryAPI.dll
        /// </summary>
        public static Assembly VintagestoryAPI => GetAssembly("VintagestoryAPI.dll");

        /// <summary>
        ///     VintagestoryLib.dll
        /// </summary>
        public static Assembly VintagestoryLib => GetAssembly("VintagestoryLib.dll");

        /// <summary>
        ///     Vintagestory.exe
        /// </summary>
        public static Assembly VintagestoryExe => Assembly.GetEntryAssembly();

        /// <summary>
        ///     Retrieves a list of all the assemblies collated within the <see cref="GameAssemblies"/> class. 
        /// </summary>
        public static IReadOnlyList<Assembly> All
        {
            get
            {
                return typeof(GameAssemblies)
                    .GetProperties(BindingFlags.Public | BindingFlags.Static)
                    .Where(p => p.PropertyType == typeof(Assembly))
                    .Select(prop => (Assembly)prop.GetValue(null))
                    .ToList();
            }
        }

        /// <summary>
        ///     Scans for a specific type within one of the game's vanilla assemblies. Includes internal classes, and nested
        ///     private classes. It can then be instantiated via Harmony.
        /// </summary>
        /// <param name="assembly">The assembly to scan within.</param>
        /// <param name="typeName">The name of the type to scan for.</param>
        /// <returns>The Type definition of the object being scanned for.</returns>
        public static Type FindType(this Assembly assembly, string typeName)
        {
            return AccessTools
                .GetTypesFromAssembly(assembly)
                .FirstOrNull(t => t.Name == typeName);
        }

        /// <summary>
        ///     Scans for a specific type within the game's vanilla assemblies. Includes internal classes, and nested private
        ///     classes. It can then be instantiated via Harmony.
        /// </summary>
        /// <param name="typeName">The name of the type to scan for.</param>
        /// <returns>The Type definition of the object being scanned for.</returns>
        public static Type FindType(string typeName)
        {
            return All
                .Select(assembly => assembly.FindType(typeName))
                .FirstOrNull();
        }

        private static Assembly GetAssembly(string name)
        {
            return Assembly.GetEntryAssembly()?
                .Modules?.FirstOrDefault(p => p.Name == name)?
                .Assembly;
        }
    }
}