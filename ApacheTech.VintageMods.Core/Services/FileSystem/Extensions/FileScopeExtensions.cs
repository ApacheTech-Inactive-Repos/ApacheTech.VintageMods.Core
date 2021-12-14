using System;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Extensions
{
    public static class FileScopeExtensions
    {
        /// <summary>
        ///     Converts the value of this FileScope enum to its equivalent string representation.
        /// </summary>
        /// <param name="scope">The FileScope enum value to convert.</param>
        public static string FastToString(this FileScope scope)
        {
            return scope switch
            {
                FileScope.Global => nameof(FileScope.Global),
                FileScope.World => nameof(FileScope.World),
                FileScope.Local => nameof(FileScope.Local),
                _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
            };
        }
    }
}