
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using ApacheTech.VintageMods.Core.Abstractions.Enum;
using ApacheTech.VintageMods.Core.Extensions;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.GameContent.AssetEnum
{
    public class NamedColour : StringEnum<NamedColour>
    {
        //
        // A
        //
        public static readonly string AliceBlue = Create(Color.AliceBlue.Name.ToLowerInvariant());
        public static readonly string AntiqueWhite = Create(Color.AntiqueWhite.Name.ToLowerInvariant());
        public static readonly string Aqua = Create(Color.Aqua.Name.ToLowerInvariant());
        public static readonly string Aquamarine = Create(Color.Aquamarine.Name.ToLowerInvariant());

        public static readonly string Azure = Create(Color.Azure.Name.ToLowerInvariant());

        //
        // B
        //
        public static readonly string Beige = Create(Color.Beige.Name.ToLowerInvariant());
        public static readonly string Bisque = Create(Color.Bisque.Name.ToLowerInvariant());
        public static readonly string Black = Create(Color.Black.Name.ToLowerInvariant());
        public static readonly string BlanchedAlmond = Create(Color.BlanchedAlmond.Name.ToLowerInvariant());
        public static readonly string Blue = Create(Color.Blue.Name.ToLowerInvariant());
        public static readonly string BlueViolet = Create(Color.BlueViolet.Name.ToLowerInvariant());
        public static readonly string Brown = Create(Color.Brown.Name.ToLowerInvariant());

        public static readonly string BurlyWood = Create(Color.BurlyWood.Name.ToLowerInvariant());

        //
        // C
        //
        public static readonly string CadetBlue = Create(Color.CadetBlue.Name.ToLowerInvariant());
        public static readonly string Chartreuse = Create(Color.Chartreuse.Name.ToLowerInvariant());
        public static readonly string Chocolate = Create(Color.Chocolate.Name.ToLowerInvariant());
        public static readonly string Coral = Create(Color.Coral.Name.ToLowerInvariant());
        public static readonly string CornflowerBlue = Create(Color.CornflowerBlue.Name.ToLowerInvariant());
        public static readonly string CornSilk = Create(Color.Cornsilk.Name.ToLowerInvariant());
        public static readonly string Crimson = Create(Color.Crimson.Name.ToLowerInvariant());

        public static readonly string Cyan = Create(Color.Cyan.Name.ToLowerInvariant());

        //
        // D
        //
        public static readonly string DarkBlue = Create(Color.DarkBlue.Name.ToLowerInvariant());
        public static readonly string DarkCyan = Create(Color.DarkCyan.Name.ToLowerInvariant());
        public static readonly string DarkGoldenRod = Create(Color.DarkGoldenrod.Name.ToLowerInvariant());
        public static readonly string DarkGrey = Create(Color.DarkGray.Name.ToLowerInvariant());
        public static readonly string DarkGreen = Create(Color.DarkGreen.Name.ToLowerInvariant());
        public static readonly string DarkKhaki = Create(Color.DarkKhaki.Name.ToLowerInvariant());
        public static readonly string DarkMagenta = Create(Color.DarkMagenta.Name.ToLowerInvariant());
        public static readonly string DarkOliveGreen = Create(Color.DarkOliveGreen.Name.ToLowerInvariant());
        public static readonly string DarkOrange = Create(Color.DarkOrange.Name.ToLowerInvariant());
        public static readonly string DarkOrchid = Create(Color.DarkOrchid.Name.ToLowerInvariant());
        public static readonly string DarkRed = Create(Color.DarkRed.Name.ToLowerInvariant());
        public static readonly string DarkSalmon = Create(Color.DarkSalmon.Name.ToLowerInvariant());
        public static readonly string DarkSeaGreen = Create(Color.DarkSeaGreen.Name.ToLowerInvariant());
        public static readonly string DarkSlateBlue = Create(Color.DarkSlateBlue.Name.ToLowerInvariant());
        public static readonly string DarkSlateGrey = Create(Color.DarkSlateGray.Name.ToLowerInvariant());
        public static readonly string DarkTurquoise = Create(Color.DarkTurquoise.Name.ToLowerInvariant());
        public static readonly string DarkViolet = Create(Color.DarkViolet.Name.ToLowerInvariant());
        public static readonly string DeepPink = Create(Color.DeepPink.Name.ToLowerInvariant());
        public static readonly string DeepSkyBlue = Create(Color.DeepSkyBlue.Name.ToLowerInvariant());
        public static readonly string DimGrey = Create(Color.DimGray.Name.ToLowerInvariant());

        public static readonly string DodgerBlue = Create(Color.DodgerBlue.Name.ToLowerInvariant());

        //
        // F
        //
        public static readonly string Firebrick = Create(Color.Firebrick.Name.ToLowerInvariant());
        public static readonly string FloralWhite = Create(Color.FloralWhite.Name.ToLowerInvariant());
        public static readonly string ForestGreen = Create(Color.ForestGreen.Name.ToLowerInvariant());

        public static readonly string Fuchsia = Create(Color.Fuchsia.Name.ToLowerInvariant());

        //
        // G
        //
        public static readonly string Gainsborough = Create(Color.Gainsboro.Name.ToLowerInvariant());
        public static readonly string GhostWhite = Create(Color.GhostWhite.Name.ToLowerInvariant());
        public static readonly string Gold = Create(Color.Gold.Name.ToLowerInvariant());
        public static readonly string GoldenRod = Create(Color.Goldenrod.Name.ToLowerInvariant());
        public static readonly string Grey = Create(Color.Gray.Name.ToLowerInvariant());
        public static readonly string Green = Create(Color.Green.Name.ToLowerInvariant());

        public static readonly string GreenYellow = Create(Color.GreenYellow.Name.ToLowerInvariant());

        //
        // H
        //
        public static readonly string Honeydew = Create(Color.Honeydew.Name.ToLowerInvariant());

        public static readonly string HotPink = Create(Color.HotPink.Name.ToLowerInvariant());

        //
        // I
        //
        public static readonly string IndianRed = Create(Color.IndianRed.Name.ToLowerInvariant());
        public static readonly string Indigo = Create(Color.Indigo.Name.ToLowerInvariant());

        public static readonly string Ivory = Create(Color.Ivory.Name.ToLowerInvariant());

        //
        // K
        //
        public static readonly string Khaki = Create(Color.Khaki.Name.ToLowerInvariant());

        //
        // L
        //
        public static readonly string Lavender = Create(Color.Lavender.Name.ToLowerInvariant());
        public static readonly string LavenderBlush = Create(Color.LavenderBlush.Name.ToLowerInvariant());
        public static readonly string LawnGreen = Create(Color.LawnGreen.Name.ToLowerInvariant());
        public static readonly string LemonChiffon = Create(Color.LemonChiffon.Name.ToLowerInvariant());
        public static readonly string LightBlue = Create(Color.LightBlue.Name.ToLowerInvariant());
        public static readonly string LightCoral = Create(Color.LightCoral.Name.ToLowerInvariant());
        public static readonly string LightCyan = Create(Color.LightCyan.Name.ToLowerInvariant());
        public static readonly string LightGoldenRodYellow = Create(Color.LightGoldenrodYellow.Name.ToLowerInvariant());
        public static readonly string LightGrey = Create(Color.LightGray.Name.ToLowerInvariant());
        public static readonly string LightGreen = Create(Color.LightGreen.Name.ToLowerInvariant());
        public static readonly string LightPink = Create(Color.LightPink.Name.ToLowerInvariant());
        public static readonly string LightSalmon = Create(Color.LightSalmon.Name.ToLowerInvariant());
        public static readonly string LightSeaGreen = Create(Color.LightSeaGreen.Name.ToLowerInvariant());
        public static readonly string LightSkyBlue = Create(Color.LightSkyBlue.Name.ToLowerInvariant());
        public static readonly string LightSlateGray = Create(Color.LightSlateGray.Name.ToLowerInvariant());
        public static readonly string LightSteelBlue = Create(Color.LightSteelBlue.Name.ToLowerInvariant());
        public static readonly string LightYellow = Create(Color.LightYellow.Name.ToLowerInvariant());
        public static readonly string Lime = Create(Color.Lime.Name.ToLowerInvariant());
        public static readonly string LimeGreen = Create(Color.LimeGreen.Name.ToLowerInvariant());

        public static readonly string Linen = Create(Color.Linen.Name.ToLowerInvariant());

        //
        // M
        //
        public static readonly string Magenta = Create(Color.Magenta.Name.ToLowerInvariant());
        public static readonly string Maroon = Create(Color.Maroon.Name.ToLowerInvariant());
        public static readonly string MediumAquamarine = Create(Color.MediumAquamarine.Name.ToLowerInvariant());
        public static readonly string MediumBlue = Create(Color.MediumBlue.Name.ToLowerInvariant());
        public static readonly string MediumOrchid = Create(Color.MediumOrchid.Name.ToLowerInvariant());
        public static readonly string MediumPurple = Create(Color.MediumPurple.Name.ToLowerInvariant());
        public static readonly string MediumSeaGreen = Create(Color.MediumSeaGreen.Name.ToLowerInvariant());
        public static readonly string MediumSlateBlue = Create(Color.MediumSlateBlue.Name.ToLowerInvariant());
        public static readonly string MediumSpringGreen = Create(Color.MediumSpringGreen.Name.ToLowerInvariant());
        public static readonly string MediumTurquoise = Create(Color.MediumTurquoise.Name.ToLowerInvariant());
        public static readonly string MediumVioletRed = Create(Color.MediumVioletRed.Name.ToLowerInvariant());
        public static readonly string MidnightBlue = Create(Color.MidnightBlue.Name.ToLowerInvariant());
        public static readonly string MintCream = Create(Color.MintCream.Name.ToLowerInvariant());
        public static readonly string MistyRose = Create(Color.MistyRose.Name.ToLowerInvariant());

        public static readonly string Moccasin = Create(Color.Moccasin.Name.ToLowerInvariant());

        //
        // N
        //
        public static readonly string NavajoWhite = Create(Color.NavajoWhite.Name.ToLowerInvariant());

        public static readonly string Navy = Create(Color.Navy.Name.ToLowerInvariant());

        //
        // O
        //
        public static readonly string OldLace = Create(Color.OldLace.Name.ToLowerInvariant());
        public static readonly string Olive = Create(Color.Olive.Name.ToLowerInvariant());
        public static readonly string OliveDrab = Create(Color.OliveDrab.Name.ToLowerInvariant());
        public static readonly string Orange = Create(Color.Orange.Name.ToLowerInvariant());
        public static readonly string OrangeRed = Create(Color.OrangeRed.Name.ToLowerInvariant());

        public static readonly string Orchid = Create(Color.Orchid.Name.ToLowerInvariant());

        //
        // P
        //
        public static readonly string PaleGoldenRod = Create(Color.PaleGoldenrod.Name.ToLowerInvariant());
        public static readonly string PaleGreen = Create(Color.PaleGreen.Name.ToLowerInvariant());
        public static readonly string PaleVioletRed = Create(Color.PaleVioletRed.Name.ToLowerInvariant());
        public static readonly string PapayaWhip = Create(Color.PapayaWhip.Name.ToLowerInvariant());
        public static readonly string PeachPuff = Create(Color.PeachPuff.Name.ToLowerInvariant());
        public static readonly string Peru = Create(Color.Peru.Name.ToLowerInvariant());
        public static readonly string Pink = Create(Color.Pink.Name.ToLowerInvariant());
        public static readonly string Plum = Create(Color.Plum.Name.ToLowerInvariant());
        public static readonly string PowderBlue = Create(Color.PowderBlue.Name.ToLowerInvariant());

        public static readonly string Purple = Create(Color.Purple.Name.ToLowerInvariant());

        //
        // R
        //
        public static readonly string Red = Create(Color.Red.Name.ToLowerInvariant());
        public static readonly string RosyBrown = Create(Color.RosyBrown.Name.ToLowerInvariant());

        public static readonly string RoyalBlue = Create(Color.RoyalBlue.Name.ToLowerInvariant());

        //
        // S
        //
        public static readonly string SaddleBrown = Create(Color.SaddleBrown.Name.ToLowerInvariant());
        public static readonly string Salmon = Create(Color.Salmon.Name.ToLowerInvariant());
        public static readonly string SandyBrown = Create(Color.SandyBrown.Name.ToLowerInvariant());
        public static readonly string SeaGreen = Create(Color.SeaGreen.Name.ToLowerInvariant());
        public static readonly string SeaShell = Create(Color.SeaShell.Name.ToLowerInvariant());
        public static readonly string Silver = Create(Color.Silver.Name.ToLowerInvariant());
        public static readonly string Sienna = Create(Color.Sienna.Name.ToLowerInvariant());
        public static readonly string SkyBlue = Create(Color.SkyBlue.Name.ToLowerInvariant());
        public static readonly string SlateBlue = Create(Color.SlateBlue.Name.ToLowerInvariant());
        public static readonly string SlateGrey = Create(Color.SlateGray.Name.ToLowerInvariant());
        public static readonly string Snow = Create(Color.Snow.Name.ToLowerInvariant());
        public static readonly string SpringGreen = Create(Color.SpringGreen.Name.ToLowerInvariant());

        public static readonly string SteelBlue = Create(Color.SteelBlue.Name.ToLowerInvariant());

        //
        // T
        //
        public static readonly string Tan = Create(Color.Tan.Name.ToLowerInvariant());
        public static readonly string Teal = Create(Color.Teal.Name.ToLowerInvariant());
        public static readonly string Thistle = Create(Color.Thistle.Name.ToLowerInvariant());
        public static readonly string Tomato = Create(Color.Tomato.Name.ToLowerInvariant());
        public static readonly string Transparent = Create(Color.Transparent.Name.ToLowerInvariant());

        public static readonly string Turquoise = Create(Color.Turquoise.Name.ToLowerInvariant());

        //
        // V
        //
        public static readonly string Violet = Create(Color.Violet.Name.ToLowerInvariant());

        //
        // W
        //
        public static readonly string Wheat = Create(Color.Wheat.Name.ToLowerInvariant());
        public static readonly string White = Create(Color.White.Name.ToLowerInvariant());

        public static readonly string WhiteSmoke = Create(Color.WhiteSmoke.Name.ToLowerInvariant());

        //
        // Y
        //
        public static readonly string Yellow = Create(Color.Yellow.Name.ToLowerInvariant());
        public static readonly string YellowGreen = Create(Color.YellowGreen.Name.ToLowerInvariant());






        public static string[] ValuesList()
        {
            var list = AllFields.Select(p => p.GetValue(null).ToString()).ToArray();
            return list;
        }

        public static string[] NamesList()
        {
            var list = AllFields.Select(p => p.Name.SplitPascalCase()).ToArray();
            return list;
        }

        public static Dictionary<string, string> ColoursByName()
        {
            var dict = new Dictionary<string, string>();
            var keys = ValuesList();
            var values = NamesList();
            for (var i = 0; i < ValuesList().Length; i++)
            {
                dict.Add(keys[i], values[i]);
            }
            return dict;
        }

        public static bool Validate(string colour)
        {
            return NamesList().Contains(colour.ToLowerInvariant());
        }

        public static string GetFullName(string colour)
        {
            return !Validate(colour) 
                ? string.Empty 
                : ColoursByName()[colour];
        }

        static NamedColour()
        {
            var type = typeof(NamedColour);
            AllFields = type.GetFields(BindingFlags.Static | BindingFlags.Public).ToList();
        }

        private static List<FieldInfo> AllFields { get; }
    }
}
