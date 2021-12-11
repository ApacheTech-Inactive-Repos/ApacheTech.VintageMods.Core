using System;
using System.Collections.Generic;
using System.Linq;

namespace ApacheTech.VintageMods.Core.Extensions.System
{
    // TODO: Migrate to ApacheTech.Common.Extensions Nuget Package
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
    }
}