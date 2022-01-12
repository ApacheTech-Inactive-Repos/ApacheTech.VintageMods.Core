using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.Extensions
{
    public static class UnsortedGameExtensions
    {
        public static void Delete(this LoadedTexture texture)
        {
            ApiEx.Client.Gui.DeleteTexture(texture.TextureId);
        }

        public static string FormatLargeNumber(this int number)
        {
            return string.Format(global::System.Globalization.CultureInfo.GetCultureInfo(Lang.CurrentLocale), "{0:#,##0.##}", number);
        }

        public static string FormatLargeNumber(this long number)
        {
            return string.Format(global::System.Globalization.CultureInfo.GetCultureInfo(Lang.CurrentLocale), "{0:#,##0.##}", number);
        }

        public static string FormatLargeNumber(this float number)
        {
            return string.Format(global::System.Globalization.CultureInfo.GetCultureInfo(Lang.CurrentLocale), "{0:#,##0.##}", number);
        }

        public static string FormatLargeNumber(this double number)
        {
            return string.Format(global::System.Globalization.CultureInfo.GetCultureInfo(Lang.CurrentLocale), "{0:#,##0.##}", number);
        }
    }
}