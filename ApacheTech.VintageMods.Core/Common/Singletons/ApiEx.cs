using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

namespace ApacheTech.VintageMods.Core.Common.Singletons
{
    /// <summary>
    ///     Global access to game's sided APIs. 
    /// </summary>
    public static class ApiEx
    {
        /// <summary>
        ///     The core API implemented by the client. The main interface for accessing the client. Contains all sub-components, and some miscellaneous methods.
        /// </summary>
        /// <value>The client-side API.</value>
        public static ICoreClientAPI Client { get; internal set; }

        /// <summary>
        ///     The core API implemented by the server. The main interface for accessing the server. Contains all sub-components, and some miscellaneous methods.
        /// </summary>
        /// <value>The server-side API.</value>
        public static ICoreServerAPI Server { get; internal set; }

        /// <summary>
        ///     Common API Components that are available on the server and the client. Cast to ICoreServerAPI, or ICoreClientAPI, to access side specific features.
        /// </summary>
        /// <value>The universal API.</value>
        public static ICoreAPI Universal { get; internal set; }

        /// <summary>
        ///     The concrete implementation of the <see cref="IClientWorldAccessor"/> interface. Contains access to lots of world manipulation and management features.
        /// </summary>
        /// <value>The <see cref="Vintagestory.Client.NoObf.ClientMain"/> instance that controls access to features within  the gameworld.</value>
        public static ClientMain ClientMain { get; internal set; }

        /// <summary>
        ///     The concrete implementation of the <see cref="IServerWorldAccessor"/> interface. Contains access to lots of world manipulation and management features.
        /// </summary>
        /// <value>The <see cref="Vintagestory.Server.ServerMain"/> instance that controls access to features within  the gameworld.</value>
        public static ServerMain ServerMain { get; internal set; }
    }
}
