using System;
using System.Collections.Generic;
using System.Linq;

namespace ApacheTech.VintageMods.Core.Extensions.System
{
    // TODO: ROADMAP - Migrate extensions to `ApacheTech.Common.Extensions` Nuget Package v1.2.0.
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Gets the closest value to a given number, within a given set of numbers.
        /// </summary>
        /// <param name="list">The set of numbers to clamp to.</param>
        /// <param name="number">The number to find.</param>
        /// <returns>System.Int32.</returns>
        public static int GetClosest(this IEnumerable<int> list, int number)
        {
            return list.Aggregate((x, y) => Math.Abs(x - number) < Math.Abs(y - number) ? x : y);
        }

        /// <summary>
        ///     Gets the closest value to a given number, within a given set of numbers.
        /// </summary>
        /// <param name="list">The set of numbers to clamp to.</param>
        /// <param name="number">The number to find.</param>
        /// <returns>System.Double.</returns>
        public static double GetClosest(this IEnumerable<double> list, double number)
        {
            return list.Aggregate((x, y) => Math.Abs(x - number) < Math.Abs(y - number) ? x : y);
        }

        /// <summary>
        ///     Gets the closest value to a given number, within a given set of numbers.
        /// </summary>
        /// <param name="list">The set of numbers to clamp to.</param>
        /// <param name="number">The number to find.</param>
        /// <returns>System.Single.</returns>
        public static float GetClosest(this IEnumerable<float> list, float number)
        {
            return list.Aggregate((x, y) => Math.Abs(x - number) < Math.Abs(y - number) ? x : y);
        }
    }
}