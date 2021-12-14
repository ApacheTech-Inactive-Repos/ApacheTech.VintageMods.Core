using System;
using HarmonyLib;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class HarmonySidedPatchAttribute : HarmonyPatch
    {
        internal EnumAppSide Side { get; }

        public HarmonySidedPatchAttribute(EnumAppSide forSide)
        {
            Side = forSide;
        }
    }
}
