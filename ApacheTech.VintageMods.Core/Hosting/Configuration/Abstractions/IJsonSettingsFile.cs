using System;

namespace ApacheTech.VintageMods.Core.Hosting.Configuration.Abstractions
{
    /// <summary>
    ///     Represents a settings file for the mod, in JSON format.
    /// </summary>
    public interface IJsonSettingsFile
    {
        /// <summary>
        ///     Retrieves the settings for a specific feature, parsed as a strongly-typed POCO class instance.
        ///     Changes made to the settings will automatically be written to the file, as they are set.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object to parse the settings for the feature into.</typeparam>
        /// <param name="featureName">The name of the feature.</param>
        /// <returns>An object, that represents the settings for a given mod feature.</returns>
        public T Feature<T>(string featureName) where T : class, new();
    }
}