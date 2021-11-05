using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.VintageMods.Core.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
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
                try
                {
                    ModSystem modSystem;
                    if (type.GetConstructors().Any(p => p.IOCEnabled()))
                    {
                        __instance.Logger.Audit($"ModSystem {type} initialised through IOC.");
                        modSystem = (ModSystem)ModServices.IOC.CreateInstance(type);
                    }
                    else
                    {
                        __instance.Logger.Audit($"ModSystem {type} initialised through Activator.");
                        modSystem = (ModSystem)Activator.CreateInstance(type);
                    }
                    modSystem.SetProperty("Mod", __instance);
                    list.Add(modSystem);
                }
                catch (Exception ex)
                {
                    __instance.Logger.Error($"Exception thrown when trying to create an instance of ModSystem {type}:\n{ex}");
                }
            }
            __instance.SetProperty("Systems", list.AsReadOnly());
            if (__instance.Systems.Count == 0 && __instance.FolderPath == null)
            {
                __instance.Logger.Warning("Is a Code mod, but no ModSystems found");
            }
            return false;
        }
    }
}
