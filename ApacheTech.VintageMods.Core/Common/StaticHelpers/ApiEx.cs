using System;
using System.Collections.Generic;
using System.Threading;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.InternalSystems;
using ApacheTech.VintageMods.Core.Extensions.Game;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

// AppSide Anywhere - Code by: Novocain1: https://github.com/Novocain1/MiscMods/blob/1.15/VSHUD/Utility/CheckAppSideAnywhere.cs

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo

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
        public static ICoreAPI Current => Side.IsClient() ? Client : Server;

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
        ///     Gets the current app-side.
        /// </summary>
        /// <value>The current app-side.</value>
        public static EnumAppSide Side
        {
            get
            {
                var appSide = FastSideLookup.TryGetValue(Thread.CurrentThread.ManagedThreadId, out var side)
                    ? side
                    : CacheCurrentThread();
                return appSide;
            }
        }

        private static readonly Dictionary<int, EnumAppSide> FastSideLookup = new();

        private static EnumAppSide CacheCurrentThread()
        {
            // Will this work in tasks?
            return FastSideLookup[Thread.CurrentThread.ManagedThreadId] =
                string.Equals(Thread.CurrentThread.Name, "SingleplayerServer", StringComparison.InvariantCultureIgnoreCase)
                    ? EnumAppSide.Server
                    : EnumAppSide.Client;
        }
        //
        // Async
        //

        public static IAsyncActions Async => OneOf<IAsyncActions>(ClientAsync, ServerAsync);

        private static ClientSystemAsyncActions ClientAsync => Client.GetVanillaClientSystem<ClientSystemAsyncActions>();

        private static ServerSystemAsyncActions ServerAsync => Server.GetVanillaServerSystem<ServerSystemAsyncActions>();


        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public static void Dispose()
        {
            FastSideLookup.Clear();
            Run(() => ClientAsync?.Dispose(),
                () => ServerAsync?.Dispose());
        }
    }
}
