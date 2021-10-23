using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ApacheTech.VintageMods.Core.Common.InternalSystems;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

// ReSharper disable StringLiteralTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Common.Extensions.Threading
{
    public static class ClientThreadInjectionExtensions
    {
        private static readonly Type ClientThread;

        static ClientThreadInjectionExtensions()
        {
            ClientThread = typeof(ClientMain).Assembly.GetClassType("ClientThread");
        }

        /// <summary>
        ///     Injects a ClientSystem into the game, which enables asynchronous tasks to be queued on the game's internal thread-pool.
        /// </summary>
        /// <param name="api">The core API implemented by the client. The main interface for accessing the client. Contains all sub-components, and some miscellaneous methods.</param>
        public static void EnableAsyncTasks(this ICoreClientAPI api)
        {
            api.Event.LevelFinalize += () =>
            {
                if (!api.IsClientSystemLoaded<ClientSystemAsyncActions>())
                    api.InjectClientThread("AsyncActions", new ClientSystemAsyncActions(api.World as ClientMain));
            };
        }

        /// <summary>
        ///     Determines whether a given ClientSystem is present within the game's registry.
        /// </summary>
        /// <param name="api">The core API implemented by the client. The main interface for accessing the client. Contains all sub-components, and some miscellaneous methods.</param>
        /// <param name="name">The name of the ClientSystem to find.</param>
        /// <returns><c>true</c> if the ClientSystem is loaded; otherwise, <c>false</c>.</returns>
        public static bool IsClientSystemLoaded(this ICoreClientAPI api, string name)
        {
            return api.World.GetVanillaSystems().Any(clientSystem => clientSystem.Name == name);
        }

        /// <summary>
        ///     Determines whether a given ClientSystem is present within the game's registry.
        /// </summary>
        /// <param name="api">The core API implemented by the client. The main interface for accessing the client. Contains all sub-components, and some miscellaneous methods.</param>
        /// <typeparam name="TClientSystem">The type of the ClientSystem to find.</typeparam>
        /// <returns><c>true</c> if the ClientSystem is loaded; otherwise, <c>false</c>.</returns>
        public static bool IsClientSystemLoaded<TClientSystem>(this ICoreClientAPI api)
            where TClientSystem : ClientSystem
        {
            return api.World.GetVanillaSystems().Any(clientSystem => clientSystem.GetType() == typeof(TClientSystem));
        }

        public static List<Thread> GetClientThreads(this IClientWorldAccessor world)
        {
            return (world as ClientMain).GetField<List<Thread>>("clientThreads");
        }

        public static Stack<ClientSystem> GetVanillaSystems(this IClientWorldAccessor world)
        {
            return new Stack<ClientSystem>((world as ClientMain).GetField<ClientSystem[]>("clientSystems"));
        }

        public static void InjectClientThread(this ICoreClientAPI capi, string name, params ClientSystem[] systems)
        {
            capi.World.InjectClientThread(name, systems);
        }

        public static void InjectClientThread(this IClientWorldAccessor world, string name, params ClientSystem[] systems)
        {
            var instance = CreateClientThread(world, name, systems);
            var clientThreads = world.GetClientThreads();
            var vanillaSystems = world.GetVanillaSystems();

            foreach (var system in systems) vanillaSystems.Push(system);

            (world as ClientMain).SetField("clientSystems", vanillaSystems.ToArray());

            var thread = new Thread(() => instance.CallMethod("Process")) {IsBackground = true};
            thread.Start();
            thread.Name = name;
            clientThreads.Add(thread);
        }

        private static object CreateClientThread(IClientWorldAccessor world, string name, IEnumerable<ClientSystem> systems)
        {
            var instance = ClientThread.CreateInstance();
            instance.SetField("game", world as ClientMain);
            instance.SetField("threadName", name);
            instance.SetField("clientsystems", systems.ToArray());
            instance.SetField("lastFramePassedTime", new Stopwatch());
            instance.SetField("totalPassedTime", new Stopwatch());
            instance.SetField("paused", false);
            return instance;
        }
    }
}