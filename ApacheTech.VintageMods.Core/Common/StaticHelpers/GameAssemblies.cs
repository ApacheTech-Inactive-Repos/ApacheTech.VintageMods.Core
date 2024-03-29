﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Extensions.DotNet;
using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Direct access to the game's vanilla assemblies.
    /// </summary>
    public static class GameAssemblies
    {
        /// <summary>
        ///     VSEssentials.dll
        /// </summary>
        public static Assembly VSEssentials => GetAssembly("VSEssentials");

        /// <summary>
        ///     VSSurvivalMod.dll
        /// </summary>
        public static Assembly VSSurvivalMod => GetAssembly("VSSurvivalMod");

        /// <summary>
        ///     VSCreativeMod.dll
        /// </summary>
        public static Assembly VSCreativeMod => GetAssembly("VSCreativeMod");

        /// <summary>
        ///     VintagestoryAPI.dll
        /// </summary>
        public static Assembly VintagestoryAPI => GetAssembly("VintagestoryAPI");

        /// <summary>
        ///     VintagestoryLib.dll
        /// </summary>
        public static Assembly VintagestoryLib => GetAssembly("VintagestoryLib");

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
            return AppDomain.CurrentDomain.GetAssemblyByName(name);
        }
    }
}