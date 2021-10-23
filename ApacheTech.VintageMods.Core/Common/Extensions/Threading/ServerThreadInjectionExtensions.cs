using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ApacheTech.VintageMods.Core.Common.InternalSystems;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace ApacheTech.VintageMods.Core.Common.Extensions.Threading
{
    public static class ServerThreadInjectionExtensions
    {
        private static readonly Type ServerThread;

        static ServerThreadInjectionExtensions()
        {
            ServerThread = typeof(ServerMain).Assembly.GetClassType("ServerThread");
        }

        /// <summary>
        ///     Injects a ServerSystem into the game, which enables asynchronous tasks to be queued on the game's internal thread-pool.
        /// </summary>
        /// <param name="api">The core API implemented by the server. The main interface for accessing the server. Contains all sub-components, and some miscellaneous methods.</param>
        public static void EnableAsyncTasks(this ICoreServerAPI api)
        {
            api.Event.SaveGameLoaded += () =>
            {
                if (!api.IsServerSystemLoaded<ServerSystemAsyncActions>())
                    api.InjectServerThread("AsyncActions", new ServerSystemAsyncActions(api.World as ServerMain));
            };
        }

        /// <summary>
        ///     Determines whether a given ServerSystem is present within the game's registry.
        /// </summary>
        /// <param name="api">The core API implemented by the server. The main interface for accessing the server. Contains all sub-components, and some miscellaneous methods.</param>
        /// <typeparam name="TServerSystem">The type of the ServerSystem to find.</typeparam>
        /// <returns><c>true</c> if the ServerSystem is loaded; otherwise, <c>false</c>.</returns>
        public static bool IsServerSystemLoaded<TServerSystem>(this ICoreServerAPI api)
            where TServerSystem : ServerSystem
        {
            return api.World.GetVanillaSystems().Any(clientSystem => clientSystem.GetType() == typeof(TServerSystem));
        }

        public static List<Thread> GetServerThreads(this IServerWorldAccessor world)
        {
            return (world as ServerMain).GetField<List<Thread>>("Serverthreads");
        }

        public static Stack<ServerSystem> GetVanillaSystems(this IServerWorldAccessor world)
        {
            return new Stack<ServerSystem>((world as ServerMain).GetField<ServerSystem[]>("Systems"));
        }

        public static void InjectServerThread(this ICoreServerAPI sapi, string name, params ServerSystem[] systems)
        {
            sapi.World.InjectServerThread(name, systems);
        }

        public static void InjectServerThread(this IServerWorldAccessor world, string name, params ServerSystem[] systems)
        {
            var instance = CreateServerThread(world, name, systems);
            var serverThreads = world.GetServerThreads();
            var vanillaSystems = world.GetVanillaSystems();

            foreach (var system in systems) vanillaSystems.Push(system);

            (world as ServerMain).SetField("Systems", vanillaSystems.ToArray());

            var thread = new Thread(() => instance.CallMethod("Process")) {IsBackground = true, Name = name};
            serverThreads.Add(thread);
        }

        private static object CreateServerThread(IServerWorldAccessor world, string name, IEnumerable<ServerSystem> systems)
        {
            var instance = ServerThread.CreateInstance();
            instance.SetField("server", world as ServerMain);
            instance.SetField("threadName", name);
            instance.SetField("serversystems", systems.ToArray());
            instance.SetField("lastFramePassedTime", new Stopwatch());
            instance.SetField("totalPassedTime", new Stopwatch());
            instance.SetField("paused", false);
            instance.SetField("alive", true);
            return instance;
        }
    }
}