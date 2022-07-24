using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class TextureExtensions
    {

        public static void Delete(this LoadedTexture texture, ICoreClientAPI capi)
        {
            ApiEx.Client.Gui.DeleteTexture(texture.TextureId);
        }

        public static void Delete(this LoadedTexture texture)
        {
            texture.Delete(ApiEx.Client);
        }
    }
}