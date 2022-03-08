using ApacheTech.VintageMods.Core.Common.StaticHelpers;
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
        internal static IJsonSettingsFile ClientGlobal { get; set; }

        /// <summary>
        ///     The per-world mod settings; these settings can change within each gameworld.
        /// </summary>
        /// <value>The per-world settings.</value>
        internal static IJsonSettingsFile ClientWorld { get; set; }

        /// <summary>
        ///     The local mod settings; these settings are only ever used when the mod is being initialised, and are not intended to be changed by the user.
        /// </summary>
        /// <value>The local settings.</value>
        internal static IJsonSettingsFile ClientLocal { get; set; }

        /// <summary>
        ///     The global mod settings; these settings will persist through each gameworld.
        /// </summary>
        /// <value>The global settings.</value>
        internal static IJsonSettingsFile ServerGlobal { get; set; }

        /// <summary>
        ///     The per-world mod settings; these settings can change within each gameworld.
        /// </summary>
        /// <value>The per-world settings.</value>
        internal static IJsonSettingsFile ServerWorld { get; set; }

        /// <summary>
        ///     The local mod settings; these settings are only ever used when the mod is being initialised, and are not intended to be changed by the user.
        /// </summary>
        /// <value>The local settings.</value>
        internal static IJsonSettingsFile ServerLocal { get; set; }
        
        /// <summary>
        ///     The global mod settings; these settings will persist through each gameworld.
        /// </summary>
        /// <value>The global settings.</value>
        public static IJsonSettingsFile Global => ApiEx.OneOf(ClientGlobal, ServerGlobal);

        /// <summary>
        ///     The per-world mod settings; these settings can change within each gameworld.
        /// </summary>
        /// <value>The per-world settings.</value>
        public static IJsonSettingsFile World => ApiEx.OneOf(ClientWorld, ServerWorld);

        /// <summary>
        ///     The local mod settings; these settings are only ever used when the mod is being initialised, and are not intended to be changed by the user.
        /// </summary>
        /// <value>The local settings.</value>
        public static IJsonSettingsFile Local => ApiEx.OneOf(ClientLocal, ServerLocal);

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public static void Dispose()
        {
            ApiEx.Run(ClientDispose, ServerDispose);
        }

        private static void ClientDispose()
        {
            ClientGlobal?.Dispose();
            ClientGlobal = null;

            ClientWorld?.Dispose();
            ClientWorld = null;

            ClientLocal?.Dispose();
            ClientLocal = null;
        }

        private static void ServerDispose()
        {
            ServerGlobal?.Dispose();
            ServerGlobal = null;

            ServerWorld?.Dispose();
            ServerWorld = null;

            ServerLocal?.Dispose();
            ServerLocal = null;
        }
    }
}