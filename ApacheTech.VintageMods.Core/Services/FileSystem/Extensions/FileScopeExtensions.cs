using System;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class FileScopeExtensions
    {
        /// <summary>
        ///     Converts the value of this FileScope enum to its equivalent string representation.
        /// </summary>
        /// <param name="scope">The FileScope enum value to convert.</param>
        [NotNull]
        public static string FastToString(this FileScope scope)
        {
            return scope switch
            {
                FileScope.Global => nameof(FileScope.Global),
                FileScope.World => nameof(FileScope.World),
                _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
            };
        }
    }
}