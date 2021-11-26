using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Services;
using HarmonyLib;
using JetBrains.Annotations;
using Vintagestory.API.Common;
using Vintagestory.Common;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.HarmonyPatches
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ModContainerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ModContainer), "InstantiateModSystems")]
        internal static bool InstantiateModSystems(ModContainer __instance, EnumAppSide side)
        {
            if (__instance.Info?.ModID != ApiEx.ModInfo.ModId)
            {
                return true;
            }
            if (!__instance.Enabled || __instance.Assembly is null || __instance.Systems.Count > 0)
            {
                return false;
            }
            if (__instance.Info is null)
            {
                throw new InvalidOperationException("LoadModInfo was not called before InstantiateModSystems");
            }
            if (!__instance.Info.Side.Is(side))
            {
                __instance.Status = ModStatus.Unused;
                return false;
            }
            var list = new List<ModSystem>();
            var systems = __instance.CallMethod<IEnumerable<Type>>("GetModSystems", __instance.Assembly);
            foreach (var type in systems)
            {
                if (!TryCreateModSystemInstance(__instance, type, out var modSystem)) continue;
                modSystem.SetProperty("Mod", __instance);
                list.Add(modSystem);

            }
            __instance.SetProperty("Systems", list.AsReadOnly());
            if (__instance.Systems.Count == 0 && __instance.FolderPath == null)
            {
                __instance.Logger.Warning("Is a Code mod, but no ModSystems found");
            }
            return false;
        }

        private static bool TryCreateModSystemInstance(ModContainer __instance, Type type, out ModSystem modSystem)
        {
            try
            {
                modSystem = type
                    .GetConstructors()
                    .Any(p => p.IOCEnabled())
                    ? (ModSystem)ModServices.IOC.CreateSidedInstance(type)
                    : (ModSystem)Activator.CreateInstance(type);
                return true;
            }
            catch (Exception ex)
            {
                __instance.Logger.Error($"Exception thrown when trying to create an instance of ModSystem {type}:\n{ex}");
                modSystem = default;
                return false;
            }
        }
    }
}
