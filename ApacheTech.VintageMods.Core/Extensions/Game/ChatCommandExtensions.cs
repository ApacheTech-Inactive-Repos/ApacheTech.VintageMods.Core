using Vintagestory.API.Common;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class ChatCommandExtensions
    {
        /// <summary>
        ///     Returns all remaining arguments as single merged string, concatenated with spaces.
        /// </summary>
        /// <param name="args">The CmdArgs instance that called this extension method.</param>
        /// <param name="defaultValue">The default value to use, if nothing remains within the argument buffer.</param>
        public static string PopAll(this CmdArgs args, string defaultValue)
        {
            var retVal = args.PopAll();
            return string.IsNullOrWhiteSpace(retVal) ? defaultValue : retVal;
        }
    }
}