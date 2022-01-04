using System;
using System.Drawing;
using System.Globalization;
using ApacheTech.VintageMods.Core.Common.Enum;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Config;
// ReSharper disable StringLiteralTypo
// ReSharper disable MemberCanBePrivate.Global

namespace ApacheTech.VintageMods.Core.Extensions.System
{
    /// <summary>
    ///     Extension Methods for when working with Colours.
    /// </summary>
    public static class ColourExtensions
    {
        /// <summary>
        ///     Updates a single colour channel within a <see cref="Color"/>
        /// </summary>
        /// <param name="colour">The colour to change the channel of.</param>
        /// <param name="channel">The channel to change the value of.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>A new instance of <see cref="Color"/>, with the updated values.</returns>
        public static Color UpdateColourChannel(this Color colour, ColourChannel channel, int value)
        {
            return channel switch
            {
                ColourChannel.A => Color.FromArgb(value, colour.R, colour.G, colour.B),
                ColourChannel.R => Color.FromArgb(colour.A, value, colour.G, colour.B),
                ColourChannel.G => Color.FromArgb(colour.A, colour.R, value, colour.B),
                ColourChannel.B => Color.FromArgb(colour.A, colour.R, colour.G, value),
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }

        /// <summary>
        ///     Normalises the specified colour. That being, it converts each channel value into a <see cref="double"/>, between 0.0 and 1.0, by dividing each value by 255.0.
        /// </summary>
        /// <param name="colour">The colour to normalise.</param>
        /// <returns>A double array, with normalised values set in RGBA order.</returns>
        public static double[] Normalise(this Color colour)
        {
            return new[]
            {
                colour.R / 255d,
                colour.G / 255d,
                colour.B / 255d,
                colour.A / 255d
            };
        }

        public static Color ToColour(this string colour)
        {
            try
            {
                return colour.StartsWith("#")
                    ? Color.FromArgb(int.Parse(colour.Replace("#", ""), NumberStyles.HexNumber))
                    : Color.FromName(colour);
            }
            catch (Exception)
            {
                ApiEx.Current.Logger.Error(Lang.Get("command-waypoint-invalidcolor"));
                return Color.Black;
            }
        }

        public static int ColourValue(this string colour)
        {
            return colour.ToColour().ToArgb() | -16777216;
        }


        public static string ToRgbHexString(this Color c) => $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";

        public static string ToArgbHexString(this Color c) => $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";

        public static string ToRgbString(this Color c) => $"RGB({c.R}, {c.G}, {c.B})";
    }
}