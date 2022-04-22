using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.InternalSystems;
using ApacheTech.VintageMods.Core.Extensions.DotNet;
using ApacheTech.VintageMods.Core.Extensions.Game;
using ApacheTech.VintageMods.Core.Extensions.Game.Threading;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

// AppSide Anywhere - Code inspired by: Novocain1 (https://github.com/Novocain1/MiscMods/blob/1.15/VSHUD/Utility/CheckAppSideAnywhere.cs)

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Global access to game's sided APIs. 
    /// </summary>
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
        public static ICoreAPI Current => OneOf<ICoreAPI>(Client, Server);

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
        ///     Chooses between one of two objects, based on whether it's being called by the client, or the server.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientObject">The client object.</param>
        /// <param name="serverObject">The server object.</param>
        /// <returns>Returns <paramref name="clientObject"/> if called from the client, or <paramref name="serverObject"/> if called from the server.</returns>
        public static T OneOf<T>(T clientObject, T serverObject)
        {
            return ModInfo.Side switch
            {
                EnumAppSide.Client => clientObject,
                EnumAppSide.Server => serverObject,
                EnumAppSide.Universal => Side.IsClient() ? clientObject : serverObject,
                _ => throw new ArgumentOutOfRangeException(nameof(ModInfo.Side), ModInfo.Side, "Corrupted ModInfo data.")
            };
        }

        /// <summary>
        ///     Invokes an action, based on whether it's being called by the client, or the server.
        /// </summary>
        /// <param name="clientAction">The client action.</param>
        /// <param name="serverAction">The server action.</param>
        public static void Run(Action clientAction, Action serverAction)
        {
            OneOf(clientAction, serverAction).Invoke();
        }

        /// <summary>
        ///     Invokes an action, based on whether it's being called by the client, or the server.
        /// </summary>
        /// <remarks>
        ///     This generic method works best with the Options Pattern, rather than anonymous tuples, when passing in multiple values as a single parameter.
        /// </remarks>
        /// <param name="clientAction">The client action.</param>
        /// <param name="serverAction">The server action.</param>
        /// <param name="parameter">The parameter to pass to the invoked action.</param>
        public static void Run<T>(Action<T> clientAction, Action<T> serverAction, T parameter)
        {
            OneOf(clientAction, serverAction).Invoke(parameter);
        }

        /// <summary>
        ///     Determines whether a given mod is installed, and enabled, on the current app-side.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns><c>true</c> if the mod is enabled; otherwise, <c>false</c>.</returns>
        public static bool IsModEnabled(string modId)
        {
            return Current.ModLoader.IsModEnabled(modId);
        }

        /// <summary>
        ///     Gets the current app-side.
        /// </summary>
        /// <value>An <see cref="EnumAppSide"/> value, representing current app-side; Client, or Server.</value>
        public static EnumAppSide Side
        {
            get
            {
                var thread = Thread.CurrentThread;
                if (thread.IsThreadPoolThread) return DetermineAppSide(thread);
                try
                {
                    return ThreadSideCache[thread.ManagedThreadId];
                }
                catch (KeyNotFoundException)
                {
                    var side = DetermineAppSide(thread);
                    return CacheAppSide(side, thread);
                }
            }
        }

        private static EnumAppSide DetermineAppSide(Thread thread)
        {                    
            // Obtaining the app-side, without having direct access to a specific CoreAPI.
            // NB: This is not a fool-proof. This is a drawback of using a Threaded Server, over Dedicated Server for Single-Player games.

            // TODO: This causes performance hits. Slowed CC down to a crawl, when used in the OnEntityCollide patch.

            // 1. If modinfo.json states the mod is only for a single side, return that side.
            if (ModInfo.Side is not EnumAppSide.Universal) return ModInfo.Side;

            // 2. If the process name is "VintagestoryServer", we are on the server.
            if (Process.GetCurrentProcess().ProcessName == "VintagestoryServer") return EnumAppSide.Server;

            // 3. If the current thread name is "SingleplayerServer", we are on the server. 
            // NB: A thread's name filters down through child threads, and thread-pool threads, unless manually changed.
            if (string.Equals(thread.Name, "SingleplayerServer", StringComparison.InvariantCultureIgnoreCase)) return EnumAppSide.Server;

            // By this stage, we know that we're in a single player game, or at least on a Threaded Server; and the ServerMain member should be populated.
            // 4. If ServerMain is populated, and the thread's name matches one within the server's thread list, we are on the server.
            // 5. By this stage, we return Client as a fallback; having exhausted all knowable reasons why we'd be on the Server.
            return ServerMain?.GetServerThreads().Any(p =>
                string.Equals(thread.Name, p.Name, StringComparison.InvariantCultureIgnoreCase)) ?? false
                ? EnumAppSide.Server
                : EnumAppSide.Client;
        }

        internal static Dictionary<int, EnumAppSide> ThreadSideCache { get; } = new();

        private static EnumAppSide CacheAppSide(EnumAppSide side, Thread thread)
        {
            // NB:  It's possible that this caching can lead to false positives; especially on Single-Player or LAN worlds.
            //      To limit this, I've made it so that thread pool threads don't get cached.
            if (thread.IsThreadPoolThread) return side;
            ThreadSideCache.AddOrUpdate(thread.ManagedThreadId, side);
            return side;
        }

        /// <summary>
        ///     Determines whether the current code block is running on the main thread. See remarks.
        /// </summary>
        /// <remarks>
        ///     Within a Single-Player game, the server will never run on the main application thread.
        ///     Single-Player servers are run as a background thread, within the client application.
        /// </remarks>
        /// <returns><c>true</c> if the code is currently running on the main application thread; otherwise <c>false</c>.</returns>
        public static bool IsOnMainThread()
        {
            var thread = Thread.CurrentThread;
            return thread.GetApartmentState() == ApartmentState.STA
                   && !thread.IsBackground
                   && !thread.IsThreadPoolThread
                   && thread.IsAlive;
        }

        //
        // Async
        //

        public static IAsyncActions Async => OneOf<IAsyncActions>(ClientAsync, ServerAsync);

        private static ClientSystemAsyncActions ClientAsync => Client.GetInternalClientSystem<ClientSystemAsyncActions>();

        private static ServerSystemAsyncActions ServerAsync => Server.GetInternalServerSystem<ServerSystemAsyncActions>();


        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public static void Dispose()
        {
            ThreadSideCache.Clear();
            Run(() => ClientAsync?.Dispose(),
                () => ServerAsync?.Dispose());
        }
    }
}
