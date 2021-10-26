using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems.Composite;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.DependencyInjection.Abstractions;
using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

// AppSide Anywhere - Code by: Novocain1

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Global access to game's sided APIs. 
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ApiEx
    {
        /// <summary>
        ///     Gets the mod information.
        /// </summary>
        /// <value>The mod information.</value>
        public static IVintageModInfo ModInfo { get; internal set; }

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
        public static ICoreAPI Current => Side.IsClient() ? Client : Server;

        /// <summary>
        ///     Gets the IOC Resolver for the Server.
        /// </summary>
        /// <value>The IOC Resolver for the Server.</value>
        public static IDependencyResolver ServerIOC { get; internal set; }

        /// <summary>
        ///     Gets the IOC Resolver for the Client.
        /// </summary>
        /// <value>The IOC Resolver for the Client.</value>
        public static IDependencyResolver ClientIOC { get; internal set; }

        /// <summary>
        ///     Gets the IOC Resolver for the current app-side.
        /// </summary>
        /// <value>The IOC Resolver for the current app-side.</value>
        public static IDependencyResolver IOC => Side.IsClient() ? ClientIOC : ServerIOC;

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

        /// <summary>
        ///     Gets the current app-side.
        /// </summary>
        /// <value>The current app-side.</value>
        public static EnumAppSide Side => GetAppSide();

        /// <summary>
        ///     Ensures that the correct Mod assembly is retrieved, when the core is not merged into mod assembly.
        /// </summary>
        /// <returns>The <see cref="Assembly"/> that contains the mod's entry point.</returns>
        public static Assembly GetModAssembly()
        {
            return ModAssembly;
        }

        /// <summary>
        ///     Ensures that the correct Core assembly is retrieved, when the core is not merged into mod assembly.
        /// </summary>
        /// <returns>The <see cref="Assembly"/> that contains the core's entry point.</returns>
        public static Assembly GetCoreAssembly()
        {
            return typeof(VintageModLoader).Assembly;
        }
        
        internal static Assembly ModAssembly { private get; set; }

        private static readonly Dictionary<int, EnumAppSide> FastSideLookup = new();

        private static void CacheCurrentThread()
        {
            FastSideLookup[Thread.CurrentThread.ManagedThreadId] =
                Thread.CurrentThread.Name == "SingleplayerServer"
                    ? EnumAppSide.Server
                    : EnumAppSide.Client;
        }

        private static EnumAppSide GetAppSide()
        {
            if (FastSideLookup.TryGetValue(Thread.CurrentThread.ManagedThreadId, out EnumAppSide side)) return side;
            CacheCurrentThread();
            side = FastSideLookup[Thread.CurrentThread.ManagedThreadId];
            return side;
        }

        public static void Dispose()
        {
            FastSideLookup.Clear();
        }
    }
}
