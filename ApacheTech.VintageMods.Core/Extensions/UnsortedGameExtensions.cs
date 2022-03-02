using System.Globalization;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Extensions
{
    public static class UnsortedGameExtensions
    {

        public static void ReloadShadersThreadSafe(this IShaderAPI api)
        {
            ApiEx.ClientMain.EnqueueMainThreadTask(() => api.ReloadShaders(), "");
        }

        public static void Delete(this LoadedTexture texture)
        {
            ApiEx.Client.Gui.DeleteTexture(texture.TextureId);
        }

        public static string FormatLargeNumber(this int number)
        {
            return string.Format(CultureInfo.GetCultureInfo(Lang.CurrentLocale), "{0:#,##0.##}", number);
        }

        public static string FormatLargeNumber(this long number)
        {
            return string.Format(CultureInfo.GetCultureInfo(Lang.CurrentLocale), "{0:#,##0.##}", number);
        }

        public static string FormatLargeNumber(this float number)
        {
            return string.Format(CultureInfo.GetCultureInfo(Lang.CurrentLocale), "{0:#,##0.##}", number);
        }

        public static string FormatLargeNumber(this double number)
        {
            return string.Format(CultureInfo.GetCultureInfo(Lang.CurrentLocale), "{0:#,##0.##}", number);
        }
    }
}