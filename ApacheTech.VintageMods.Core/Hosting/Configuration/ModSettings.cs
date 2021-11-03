using ApacheTech.VintageMods.Core.Hosting.Configuration.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;

namespace ApacheTech.VintageMods.Core.Hosting.Configuration
{
    /// <summary>
    ///     Globally accessible settings files for the mod. Populated via the <see cref="IFileSystemService"/>.
    /// </summary>
    public static class ModSettings
    {
        /// <summary>
        ///     The global mod settings; these settings will persist through each gameworld.
        /// </summary>
        /// <value>The global settings.</value>
        public static IJsonSettingsFile Global { get; internal set; }

        /// <summary>
        ///     The per-world mod settings; these settings can change within each gameworld.
        /// </summary>
        /// <value>The per-world settings.</value>
        public static IJsonSettingsFile World { get; internal set; }

        /// <summary>
        ///     The local mod settings; these settings are only ever used when the mod is being initialised, and are not intended to be changed by the user.
        /// </summary>
        /// <value>The local settings.</value>
        internal static IJsonSettingsFile Local { get; set; }
    }
}