using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Abstractions.Enum
{
    /// <summary>
    ///     Simple implementation of a string-based enumeration, allowing equatable string constants that can be implicitly
    ///     cast to strings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IEquatable{T}" />
    public abstract class StringEnum<T> : IEquatable<T> where T : StringEnum<T>, new()
    {
        /// <summary>
        ///     A dictionary of values held be this instance.
        /// </summary>
        private static readonly Dictionary<string, T> ValueDict = new();

        /// <summary>
        ///     The value given to this StringEnum member.
        /// </summary>
        protected string Value { get; init; }

        bool IEquatable<T>.Equals(T other)
        {
            return Value.Equals(other?.Value);
        }

        /// <summary>
        ///     Creates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        protected static T Create(string value)
        {
            if (value is null) return default;
            var obj1 = new T { Value = value };
            ValueDict.Add(value, obj1);
            return obj1;
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="StringEnum{T}" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(StringEnum<T> enumValue)
        {
            return enumValue.Value;
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Value;
        }

        public static bool operator !=(StringEnum<T> o1, StringEnum<T> o2)
        {
            return o1?.Value != o2?.Value;
        }

        public static bool operator ==(StringEnum<T> o1, StringEnum<T> o2)
        {
            return o1?.Value == o2?.Value;
        }

        public override bool Equals(object other)
        {
            return Value.Equals((other as T)?.Value ?? other as string);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static T Parse(string value, bool caseSensitive = true)
        {
            if (TryParse(value, caseSensitive, out var obj)) return obj;
            throw new InvalidOperationException(
                $"{(value == null ? "null" : $"'{value}'")} is not a valid {typeof(T).Name}");
        }

        public static bool TryParse(string value, bool caseSensitive, out T item)
        {
            item = null;
            if (value == null)
                return false;
            if (ValueDict.Count == 0)
                RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
            if (caseSensitive) return ValueDict.TryGetValue(value, out item);
            item = ValueDict.FirstOrDefault(f => f.Key.Equals(value, StringComparison.OrdinalIgnoreCase)).Value;
            return item is not null;
        }
    }
}