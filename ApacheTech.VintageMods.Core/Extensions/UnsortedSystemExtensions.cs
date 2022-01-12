using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global

namespace ApacheTech.VintageMods.Core.Extensions
{
    public static class UnsortedSystemExtensions
    {
        /// <summary>
        ///     Determines whether a string contains any letters.
        /// </summary>
        /// <param name="input">the input string</param>
        /// <returns><c>true</c> is the string contains letters [Aa..Zz]; otherwise <c>false</c>.</returns>
        public static bool ContainsLetters(this string input)
        {
            return input?.Any(char.IsLetter) ?? false;
        }

        /// <summary>
        ///     Determines whether a string contains any letters.
        /// </summary>
        /// <param name="input">the input string</param>
        /// <returns><c>true</c> is the string contains letters [Aa..Zz]; otherwise <c>false</c>.</returns>
        public static bool OnlyContainsLetters(this string input)
        {
            return input?.All(char.IsLetter) ?? false;
        }

        /// <summary>
        ///     Determines whether a string contains any numbers.
        /// </summary>
        /// <param name="input">the input string</param>
        /// <returns><c>true</c> is the string contains numbers [0..9]; otherwise <c>false</c>.</returns>
        public static bool ContainsNumbers(this string input)
        {
            return input?.Any(char.IsNumber) ?? false;
        }

        /// <summary>
        ///     Determines whether a string contains any numbers.
        /// </summary>
        /// <param name="input">the input string</param>
        /// <returns><c>true</c> is the string contains numbers [0..9]; otherwise <c>false</c>.</returns>
        public static bool OnlyContainsNumbers(this string input)
        {
            return input?.All(char.IsNumber) ?? false;
        }

        /// <summary>
        ///    Strips out non-numeric characters in string, returning only digits
        ///    ref.: https://stackoverflow.com/questions/3977497/stripping-out-non-numeric-characters-in-string
        /// </summary>
        /// <param name="input">the input string</param>
        /// <returns>the input string numeric part: for example, if input is "XYZ1234A5U6" it will return "123456"</returns>
        public static string GetNumbers(this string input)
        {
            return new string(input?.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        ///     Strips out numeric and special characters in string, returning only letters
        /// </summary>
        /// <param name="input">the input string</param>
        /// <returns>the letters contained within the input string: for example, if input is "XYZ1234A5U6~()" it will return "XYZAU"</returns>
        public static string GetLetters(this string input)
        {
            return new string(input?.Where(char.IsLetter).ToArray());
        }

        /// <summary>
        ///     Strips out any non-numeric/non-digit character in string, returning only letters and numbers
        /// </summary>
        /// <param name="input">the input string</param>
        /// <returns>the letters contained within the input string: for example, if input is "XYZ1234A5U6~()" it will return "XYZ1234A5U6"</returns>
        public static string GetLettersAndNumbers(this string input)
        {
            return new string(input?.Where(char.IsLetterOrDigit).ToArray());
        }

        public static string SplitPascalCase(this string input)
        {
            var words = Regex
                .Matches(input, @"[A-Z]+(?=[A-Z][a-z]+)|\d|[A-Z][a-z]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();
            return string.Join(" ", words);
        }

        public static string EnsureEndsWith(this string path, string appendix)
        {
            if (path.EndsWith(appendix))
            {
                path += appendix;
            }
            return path;
        }

        public static void Purge<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TValue : IDisposable
        {
            dictionary.Values.DisposeAll();
            dictionary.Clear();
        }

        /// <summary>
        ///     Purges the specified list, by disposing all elements, then clearing the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public static void Purge<T>(this ICollection<T> list) where T : IDisposable
        {
            list.DisposeAll();
            list.Clear();
        }

        /// <summary>
        ///     Disposes all elements of a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public static void DisposeAll<T>(this ICollection<T> list) where T : IDisposable
        {
            foreach (var item in list)
            {
                item?.Dispose();
            }
        }

        public static string Pluralise(this string singular, int value, string plural)
        {
            return Math.Abs(value) == 1 ? singular : plural;
        }
    }
}