using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;

// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class RenderApiExtensions
    {
        public static void ReloadShadersThreadSafe(this IShaderAPI api, ICoreClientAPI capi)
        {
            capi.AsClientMain().EnqueueMainThreadTask(() => api.ReloadShaders(), "");
        }

        public static void ReloadShadersThreadSafe(this IShaderAPI api)
        {
            api.ReloadShadersThreadSafe(ApiEx.Client);
        }
    }
}