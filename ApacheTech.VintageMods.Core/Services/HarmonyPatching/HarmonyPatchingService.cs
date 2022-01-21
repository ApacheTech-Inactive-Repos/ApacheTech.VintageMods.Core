using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Annotation;
using ApacheTech.Common.Extensions.Reflection;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using SmartAssembly.Attributes;
using Vintagestory.API.Common;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.HarmonyPatching
{
    /// <summary>
    ///     Provides methods of applying Harmony patches to the game.
    /// </summary>
    /// <remarks>
    ///     By default, all annotated [HarmonyPatch] classes in the executing assembly will
    ///     be processed at launch. Manual patches can be processed later on at runtime.
    /// </remarks>
    [DoNotPruneType]
    [RegisteredService(ServiceLifetime.Singleton, typeof(IHarmonyPatchingService))]
    internal class HarmonyPatchingService : IHarmonyPatchingService, IDisposable
    {
        private readonly Dictionary<string, Harmony> _instances = new();

        /// <summary>
        ///     Creates a new patch host, if one with the specified ID doesn't already exist.
        /// </summary>
        /// <param name="harmonyId">The identifier to use for the patch host.</param>
        /// <returns>A <see cref="Harmony" /> patch host.</returns>
        private Harmony CreateOrUseInstance(string harmonyId)
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
        ///     Runs all patches within classes decorated with the <see cref="HarmonySidedPatchAttribute" /> attribute, for the given side.
        /// </summary>
        /// <param name="instance">The harmony instance for which to run the patches for.</param>
        /// <param name="assembly">The assembly that hold the annotated patch classes to process.</param>
        private static void PatchAll(Harmony instance, Assembly assembly)
        {
            var sidedPatches = assembly.GetTypesWithAttribute<HarmonySidedPatchAttribute>();
            foreach (var (type, attribute) in sidedPatches)
            {
                if (attribute.Side is EnumAppSide.Universal || attribute.Side == ApiEx.Side)
                {
                    instance.CreateClassProcessor(type).Patch();        
                }
            }
        }

        /// <summary>
        ///     Un-patches all methods, within all patch host instances being handled by this service.
        /// </summary>
        private void UnpatchAll()
        {
            foreach (var harmony in _instances)
            {
                harmony.Value.UnpatchAll(harmony.Key);
            }
        }

        /// <summary>
        ///     By default, all annotated [HarmonyPatch] classes in the executing assembly will
        ///     be processed at launch. Manual patches can be processed later on at runtime.
        /// </summary>
        public void UseHarmony()
        {
            ApplyHarmonyPatches(AssemblyEx.GetModAssembly());
        }

        /// <summary>
        ///     By default, all annotated [HarmonyPatch] classes in the executing assembly will
        ///     be processed at launch. Manual patches can be processed later on at runtime.
        /// </summary>
        private void ApplyHarmonyPatches(Assembly assembly)
        {
            try
            {
                var harmony = CreateOrUseInstance(assembly.FullName);
                PatchAll(harmony, assembly);
                var patches = harmony.GetPatchedMethods().ToList();
                if (!patches.Any()) return;
                ApiEx.Current.Logger.Notification($"\t{assembly.GetName()} - Patched Methods:");
                foreach (var method in patches)
                {
                    ApiEx.Current.Logger.Notification($"\t\t{method.FullDescription()}");
                }
            }
            catch (Exception ex)
            {
                ApiEx.Current.Logger.Error($"[VintageMods] {ex}");
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            UnpatchAll();
        }
    }
}