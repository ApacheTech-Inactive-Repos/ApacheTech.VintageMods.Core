#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Extensions.DotNet
{
    // TODO: ROADMAP - Migrate extensions to `ApacheTech.Common.Extensions` Nuget Package v1.2.0.
    public static class CollectionExtensions
    {
        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>T.</returns>
        public static T Random<T>(this IList<T> collection)
        {
            var index = new Random().Next(0, collection.Count);
            return collection[index];
        }

        /// <summary>
        ///     Gets the closest value to a given number, within a given set of numbers.
        /// </summary>
        /// <param name="list">The set of numbers to clamp to.</param>
        /// <param name="number">The number to find.</param>
        /// <returns>System.Int32.</returns>
        public static int ClosestTo(this IEnumerable<int> list, int number)
        {
            return list.Aggregate((x, y) => Math.Abs(x - number) < Math.Abs(y - number) ? x : y);
        }

        /// <summary>
        ///     Gets the closest value to a given number, within a given set of numbers.
        /// </summary>
        /// <param name="list">The set of numbers to clamp to.</param>
        /// <param name="number">The number to find.</param>
        /// <returns>System.Double.</returns>
        public static double ClosestTo(this IEnumerable<double> list, double number)
        {
            return list.Aggregate((x, y) => Math.Abs(x - number) < Math.Abs(y - number) ? x : y);
        }

        /// <summary>
        ///     Gets the closest value to a given number, within a given set of numbers.
        /// </summary>
        /// <param name="list">The set of numbers to clamp to.</param>
        /// <param name="number">The number to find.</param>
        /// <returns>System.Single.</returns>
        public static float ClosestTo(this IEnumerable<float> list, float number)
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
        ///     Adds a range of entries into a sorted dictionary.
        ///     Includes a work around the fact that keys within Sorted Dictionaries cannot normally be overwritten.
        /// </summary>
        /// <param name="dict">The dictionary to save the syntax list to.</param>
        /// <param name="list">The list of syntax options to add.</param>
        /// <param name="predicate">The data member to search by.</param>
        public static void AddOrUpdateRange<TKey, TValue>(this SortedDictionary<TKey, TValue> dict, IEnumerable<TValue> list, Func<TValue, TKey> predicate)
        {
            foreach (var record in list)
            {
                dict.AddOrUpdate(predicate(record), record);
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
        ///     Adds an item to the <see cref="IDictionary{TKey,TValue}" />, if it not already present in the collection.
        /// </summary>
        /// <typeparam name="TKey">The type of the elements of <paramref name="key" />.</typeparam>
        /// <typeparam name="TValue">The type of the elements of <paramref name="value" />.</typeparam>
        /// <param name="collection">The collection to add the item to.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        /// <returns><c>true</c> if the item was added to collection, <c>false</c> otherwise.</returns>
        public static bool AddIfNotPresent<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            var contains = collection.ContainsKey(key);
            if (!contains) collection.Add(key,value);
            return !contains;
        }

        /// <summary>
        ///     Adds an item to the <see cref="IDictionary{TKey,TValue}" />, if it not already present in the collection.
        /// </summary>
        /// <typeparam name="TKey">The type of the elements of <paramref name="key" />.</typeparam>
        /// <typeparam name="TValue">The type of the elements of <paramref name="value" />.</typeparam>
        /// <param name="collection">The collection to add the item to.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        /// <returns><c>true</c> if the item was added to collection, <c>false</c> otherwise.</returns>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            if (!collection.ContainsKey(key))
            {
                collection.Add(key, value);
                return;
            }
            collection[key] = value;
        }

        /// <summary>
        ///     Adds an item to the <see cref="IEnumerable{TValue}" />, if it should exist within the collection, and doesn't already.
        ///     If the value shouldn't exist, and it does, it will be removed from the collection.
        /// </summary>
        /// <typeparam name="TValue">The type of the elements of <paramref name="value" />.</typeparam>
        /// <param name="collection">The collection to add the item to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="shouldExist">Whether or not the value should exist within the collection.</param>
        /// <returns><c>true</c> if the item was added to collection, <c>false</c> otherwise.</returns>
        public static void EnsureExistence<TValue>(this List<TValue> collection, TValue value, bool shouldExist) where TValue : IEquatable<TValue>
        {
            collection.RemoveAll(p => p.Equals(value));
            if (shouldExist) collection.Add(value);
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
            catch(Exception)
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