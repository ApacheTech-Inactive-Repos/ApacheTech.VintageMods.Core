using JetBrains.Annotations;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Utility class for enabling i18n. Loads language entries from assets/[category]/[locale].json
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class LangEx
    {
        private static string Get(string code, string section, object[] args)
        {
            return Lang.Get(code.StartsWith(ServiceEx.ModInfo.ModId) 
                ? code 
                : $"{ServiceEx.ModInfo.ModId}:{section}.{code}", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string Error(string code, params object[] args)
        {
            return Get(code, "Errors", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string UI(string code, params object[] args)
        {
            return Get(code, "UI", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string Message(string code, params object[] args)
        {
            return Get(code, "Messages", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string Meta(string code, params object[] args)
        {
            return Get(code, "Meta", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string Phrases(string code, params object[] args)
        {
            return Get(code, "Phrases", args);
        }
    }
}