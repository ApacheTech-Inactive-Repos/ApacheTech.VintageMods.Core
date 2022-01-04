using System;
using Vintagestory.API.Config;

// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Extended functionality for the <see cref="Lang"/> class.
    /// </summary>
    public static class LangEx
    {
        /// <summary>
        ///     Returns a localised string based on the state of a boolean variable.
        /// </summary>
        /// <param name="state">The boolean value to localise.</param>
        /// <returns>A localised string representation of the boolean value.</returns>
        public static string BooleanString(bool state)
        {
            return Lang.Get($"vmods:boolean-value-{state}");
        }

        /// <summary>
        ///     Returns a localised string based on the month value within a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object to localise.</param>
        /// <returns>A localised string representation of the full name of the month of the year.</returns>
        public static string FullMonthString(DateTime dateTime)
        {
            return Lang.Get($"vmods:datetime-months-full-{dateTime.Month}");
        }

        /// <summary>
        ///     Returns a localised string based on the month value within a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object to localise.</param>
        /// <returns>A localised string representation of the abbreviated name of the month of the year.</returns>
        public static string AbbreviatedMonthString(DateTime dateTime)
        {
            return Lang.Get($"vmods:datetime-months-abbr-{dateTime.Month}");
        }

        /// <summary>
        ///     Returns a localised string based on the day value within a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object to localise.</param>
        /// <returns>A localised string representation of the full name of the day of the week.</returns>
        public static string FullDayString(DateTime dateTime)
        {
            return Lang.Get($"vmods:datetime-days-full-{dateTime.Month}");
        }

        /// <summary>
        ///     Returns a localised string based on the day value within a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object to localise.</param>
        /// <returns>A localised string representation of the abbreviated name of the day of the week.</returns>
        public static string AbbreviatedDayString(DateTime dateTime)
        {
            return Lang.Get($"vmods:datetime-days-abbr-{dateTime.Month}");
        }

        /// <summary>
        ///     Returns a localised string.
        /// </summary>
        /// <param name="featureName">The name of the feature.</param>
        /// <param name="path">The path to the feature based string to translate.</param>
        /// <param name="args">The arguments to pass to the lang file.</param>
        /// <returns>A localised string representation of the abbreviated name of the day of the week.</returns>
        public static string FeatureString(string featureName, string path, params object[] args)
        {
            return Lang.Get(FeatureCode(featureName, path), args);
        }

        /// <summary>
        ///     Returns a localised string.
        /// </summary>
        /// <param name="featureName">The name of the feature.</param>
        /// <param name="path">The path to the feature based string to translate.</param>
        /// <returns>A localised string representation of the abbreviated name of the day of the week.</returns>
        public static string FeatureCode(string featureName, string path)
        {
            return $"{ApiEx.ModInfo.ModId}:Features.{featureName}.{path}";
        }

    }
}
