using System;
using System.Collections.Generic;
using System.Reflection;
using ApacheTech.VintageMods.Core.Common.Extensions;
using ApacheTech.VintageMods.Core.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.DependencyInjection.Annotation;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Services.HarmonyPatching
{
    /// <summary>
    ///     Provides methods of applying Harmony patches to the game.
    /// </summary>
    /// <remarks>
    ///     By default, all annotated [HarmonyPatch] classes in the executing assembly will
    ///     be processed at launch. Manual patches can be processed later on at runtime.
    /// </remarks>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [RegisteredService(ServiceLifetime.Singleton, typeof(IHarmonyPatchingService))]
    internal class HarmonyPatchingService : IHarmonyPatchingService, IDisposable
    {
        private readonly ICoreAPI _api;
        private readonly Dictionary<string, Harmony> _instances = new();

        public HarmonyPatchingService(ICoreAPI api)
        {
            _api = api;
        }

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
        private void PatchAll(Harmony instance, Assembly assembly)
        {
            // Assume all vanilla patched classes are universal... for backwards compatibility.
            //
            // ROADMAP: This should be obsolete by the time all mods have been migrated over to Core-v2.
            //          It is planned that by convention, only `HarmonySidedPatchAttribute`s will be used
            //          from now on, with this custom patching method being applied wherever it is needed.
            //
            //          This will mean a far more robust Harmony Wrapper will be needed for the VintageMods
            //          Core, that limits access to the underlying Harmony instance. That should be the 
            //          purpose of this service, as we move forwards. This will save any confusion, and also
            //          solve the problem of a lack of documentation from Harmony, as I will add my own XML
            //          Documentation to each accessible method that a mod could use.
            //
            //          It's possible that this could also be shipped separately as a Nuget Package, and
            //          made available to the modding community as a wrapper for their own mods.
            var vanillaPatches = assembly.GetTypesWithAttribute<HarmonyPatch>();
            foreach (var patch in vanillaPatches)
            {
                instance.CreateClassProcessor(patch.Type).Patch();
            }

            var sidedPatches = assembly.GetTypesWithAttribute<HarmonySidedPatchAttribute>();
            foreach (var (type, attribute) in sidedPatches)
            {
                if (attribute.Side is EnumAppSide.Universal || attribute.Side == _api.Side)
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
            ApplyHarmonyPatches(ApiEx.GetModAssembly());
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
                _api.Logger.Notification($"\t{assembly.GetName()} - Patched Methods:");
                foreach (var method in harmony.GetPatchedMethods())
                {
                    _api.Logger.Notification($"\t\t{method.FullDescription()}");
                }
            }
            catch (Exception ex)
            {
                _api.Logger.Error($"[VintageMods] {ex}");
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