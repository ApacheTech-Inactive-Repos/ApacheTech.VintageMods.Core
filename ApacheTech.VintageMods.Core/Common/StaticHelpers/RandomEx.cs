using System;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Static Helper class for Random Number Generation.
    /// </summary>
    public static class RandomEx
    {
        private static readonly Random Random;

        static RandomEx()
        {
            Random = new Random();
        }

        /// <summary>
        ///     Returns a random <see langword="double"/> that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A double-precision floating-point number greater than or equal to <paramref name="minValue">minValue</paramref> and less than <paramref name="maxValue">maxValue</paramref>; that is, the range of return values includes <paramref name="minValue">minValue</paramref> but not <paramref name="maxValue">maxValue</paramref>. If <paramref name="minValue">minValue</paramref> equals <paramref name="maxValue">maxValue</paramref>, <paramref name="minValue">minValue</paramref> is returned.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="minValue">minValue</paramref> is greater than <paramref name="maxValue">maxValue</paramref>.</exception>
        public static double RandomValueBetween(double minValue, double maxValue)
        {
            if (minValue.Equals(maxValue)) return minValue;
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), minValue,
                    "The minimum value is greater than the maximum value.");
            }
            return Random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static bool RandomBool()
        {
            return Random.Next(2) == 0;
        }

        public static double RandomSignedValueBetween(double minValue, double maxValue)
        {
            var value = RandomValueBetween(minValue, maxValue);
            return RandomBool() ? value : -value;
        }
    }
}