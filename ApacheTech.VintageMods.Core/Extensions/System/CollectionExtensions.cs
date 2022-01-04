#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

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

        /// <summary>
        ///     Adds a range of entries into a sorted dictionary.
        ///     Includes a work around the fact that keys within Sorted Dictionaries cannot normally be overwritten.
        /// </summary>
        /// <param name="dict">The dictionary to save the syntax list to.</param>
        /// <param name="list">The list of syntax options to add.</param>
        /// <param name="predicate">The data member to search by.</param>
        public static void AddOrUpdateRange<TKey, TValue>(this SortedDictionary<TKey, TValue> dict, IEnumerable<TValue> list, Func<TValue, IEnumerable<TKey>> predicate)
        {
            var validRecords = list.SelectMany(predicate, (v, k) => new KeyValuePair<TKey, TValue>(k, v));
            foreach (var record in validRecords)
            {
                dict.AddOrUpdate(record);
            }
        }

        /// <summary>
        ///     Adds an entry into a sorted dictionary.
        ///     Includes a work around the fact that keys within Sorted Dictionaries cannot normally be overwritten.
        /// </summary>
        /// <param name="dict">The dictionary to save the syntax list to.</param>
        /// <param name="record">The key-value pair to add.</param>
        private static void AddOrUpdate<TKey, TValue>(this SortedDictionary<TKey, TValue> dict, KeyValuePair<TKey, TValue> record)
        {
            dict.AddOrUpdate(record.Key, record.Value);
        }

        /// <summary>
        ///     Adds an entry into a sorted dictionary.
        ///     Includes a work around the fact that keys within Sorted Dictionaries cannot normally be overwritten.
        /// </summary>
        /// <param name="dict">The dictionary to save the syntax list to.</param>
        /// <param name="key">The key of the value to add.</param>
        /// <param name="value">The value to add.</param>
        private static void AddOrUpdate<TKey, TValue>(this SortedDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            try
            {
                dict.Add(key, value);
            }
            catch (ArgumentException)
            {
                dict.Remove(key);
                dict.Add(key, value);
            }
        }

        /// <summary>
        ///     Returns the first element of the sequence that satisfies a condition or null if no such element is found.
        /// </summary>
        /// <typeparam name="TValue">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IDictionary`2" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><see langword="null" /> if <paramref name="source" /> is empty or if no element passes the test specified by
        /// <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test
        /// specified by <paramref name="predicate" />.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
        public static TValue? FirstOrNull<TValue>(this IDictionary<string, TValue> source, Func<KeyValuePair<string, TValue>, bool> predicate)
            where TValue : class
        {
            try
            {
                return source.First(predicate).Value;
            }
            catch
            {
                return null;
            }
        }
        
        public static bool ContainsAny<TValue>(this IEnumerable<TValue> source, IEnumerable<TValue> keys)
            where TValue : class
        {
            return keys.Any(source.Contains);
        }
        
        public static bool ContainsAnyKey<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<TKey> keys)
            where TValue : class
        {
            return keys.Any(source.Keys.Contains);
        }
    }
}