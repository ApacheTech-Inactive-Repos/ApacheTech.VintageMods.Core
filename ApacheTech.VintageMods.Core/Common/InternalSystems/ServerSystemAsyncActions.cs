﻿using System;
using System.Collections.Concurrent;
using Vintagestory.Server;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Common.InternalSystems
{
    /// <summary>
    ///     An internal system that is injected into the game, on the server.
    /// </summary>
    /// <seealso cref="ServerSystem" />
    public class ServerSystemAsyncActions : ServerSystem, IAsyncActions
    {
        private readonly ServerMain _game;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ServerSystemAsyncActions" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public ServerSystemAsyncActions(ServerMain game) : base(game)
        {
            _game = game;
        }

        private ConcurrentQueue<Action> AsyncActions { get; set; } = new();

        private ConcurrentQueue<Action> MainThreadActions { get; set; } = new();

        /// <summary>
        ///     Called every tick, on the thread that this system resides on.
        /// </summary>
        //public override void OnSeparateThreadTick()
        public override void OnSeperateThreadTick(float dts)
        {
            ProcessActions(AsyncActions);
            ProcessMainThreadActions();
        }

        /// <summary>
        ///     Adds an action to the task list for the VintageMods shared thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public void EnqueueAsyncTask(Action action)
        {
            AsyncActions.Enqueue(action);
        }

        /// <summary>
        ///     Adds an action to the task list for the main game thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public void EnqueueMainThreadTask(Action action)
        {
            MainThreadActions.Enqueue(action);
        }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            if (!AsyncActions.IsEmpty)
                AsyncActions = new ConcurrentQueue<Action>();

            if (!MainThreadActions.IsEmpty)
                MainThreadActions = new ConcurrentQueue<Action>();

            base.Dispose();
        }

        private void ProcessMainThreadActions()
        {
            if (!MainThreadActions.IsEmpty)
                _game.EnqueueMainThreadTask(() => ProcessActions(MainThreadActions));
        }

        private static void ProcessActions(ConcurrentQueue<Action> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                var success = actions.TryDequeue(out var action);
                if (success) action.Invoke();
            }
        }
    }
}