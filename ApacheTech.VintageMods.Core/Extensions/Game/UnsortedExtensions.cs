using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.Common.Extensions.Harmony;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using Vintagestory.Server;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    // TODO: This is a mess.
    public static class UnsortedExtensions
    {

        public static ModSystem GetModSystem(this ModLoader modLoader, Type type)
        {
            return modLoader.Systems.FirstOrDefault(p => p.GetType() == type);
        }

        public static void SendMessage(this IServerPlayer player, string message)
        {
            player.SendMessage(GlobalConstants.CurrentChatGroup, message, EnumChatType.Notification);
        }

        public static void ShowChatMessage(this IPlayer player, string message)
        {
            var api = player.Entity.Api;
            switch (api.Side)
            {
                case EnumAppSide.Server:
                    ((IServerPlayer)player).SendMessage(message);
                    break;
                case EnumAppSide.Client:
                    ((IClientPlayer)player).ShowChatNotification(message);
                    break;
                case EnumAppSide.Universal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void RegisterDelayedCallback(this ICoreAPI api, Action<float> onDelayedCallbackTick, int millisecondInterval)
        {
            api.Event.EnqueueMainThreadTask(() =>
            {
                api.Event.RegisterCallback(onDelayedCallbackTick, millisecondInterval);
            }, "");
        }

        public static Vec2f ClientWindowSize(this ICoreClientAPI capi)
        {
            var platform = capi.AsClientMain().GetField<ClientPlatformWindows>("Platform");
            return new Vec2f(platform.window.Width, platform.window.Height);
        }

        public static object GetInternalClientSystem(this ICoreClientAPI api, string name)
        {
            var clientSystems = (api.World as ClientMain).GetField<ClientSystem[]>("clientSystems");
            return clientSystems.FirstOrDefault(p => p.Name == name);
        }

        public static T GetInternalClientSystem<T>(this ICoreClientAPI api) where T : ClientSystem
        {
            var clientSystems = (api.World as ClientMain).GetField<ClientSystem[]>("clientSystems");
            return clientSystems.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
        }



        public static T GetInternalServerSystem<T>(this ICoreServerAPI sapi) where T : ServerSystem
        {
            var systems = sapi.AsServerMain().GetField<ServerSystem[]>("Systems");
            return systems.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
        }

        public static void UnregisterCommand(this ICoreClientAPI capi, string cmd)
        {
            var eventManager = (capi.World as ClientMain).GetField<ClientEventManager>("eventManager");
            var chatCommands = eventManager.GetField<Dictionary<string, ChatCommand>>("chatCommands");
            if (chatCommands.ContainsKey(cmd)) chatCommands.Remove(cmd);
            eventManager.SetField("chatCommands", chatCommands);
        }

        public static void UnregisterInternalClientSystem<T>(this ICoreClientAPI capi) where T : ClientSystem
        {
            var clientMain = capi.World as ClientMain;
            var clientSystems = clientMain.GetField<ClientSystem[]>("clientSystems").ToList();
            for (var i = 0; i < clientSystems.Count; i++)
            {
                if (clientSystems[i].GetType() != typeof(T)) continue;
                clientSystems[i].Dispose(clientMain);
                clientSystems.Remove(clientSystems[i]);
                break;
            }

            clientMain.SetField("clientSystems", clientSystems.ToArray());
        }

        public static void UnregisterInternalClientSystem(this ICoreClientAPI capi, string name)
        {
            var clientMain = capi.World as ClientMain;
            var clientSystems = clientMain.GetField<ClientSystem[]>("clientSystems").ToList();
            for (var i = 0; i < clientSystems.Count; i++)
            {
                if (clientSystems[i].Name != name) continue;
                clientSystems[i].Dispose(clientMain);
                clientSystems.Remove(clientSystems[i]);
                break;
            }

            clientMain.SetField("clientSystems", clientSystems.ToArray());
        }

        /// <summary>
        ///     Thread-Safe.
        ///     Shows a client side only chat message in the current chat channel. Does not execute client commands.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="message">The message to show to the player.</param>
        public static void EnqueueShowChatMessage(this ICoreClientAPI api, string message)
        {
            (api.World as ClientMain)?.EnqueueShowChatMessage(message);
        }

        /// <summary>
        ///     Thread-Safe.
        ///     Shows a client side only chat message in the current chat channel. Does not execute client commands.
        /// </summary>
        /// <param name="game">The core game API this method was called from.</param>
        /// <param name="message">The message to show to the player.</param>
        public static void EnqueueShowChatMessage(this ClientMain game, string message)
        {
            game?.EnqueueMainThreadTask(() => game.ShowChatMessage(message), "");
        }

        public static TBlockEntity GetNearestBlockEntity<TBlockEntity>(this IWorldAccessor world, BlockPos pos,
            float horRange, float vertRange, System.Func<TBlockEntity, bool> predicate) where TBlockEntity : BlockEntity
        {
            TBlockEntity blockEntity = null;
            var minPos = pos.AddCopy(-horRange, -vertRange, -horRange);
            var maxPos = pos.AddCopy(horRange, vertRange, horRange);
            world.BlockAccessor.WalkBlocks(minPos, maxPos, (_, blockPos) =>
            {
                var entity = world.GetBlockAccessorPrefetch(false, false).GetBlockEntity(blockPos);
                if (entity == null) return;
                if (entity.GetType() == typeof(TBlockEntity) && predicate((TBlockEntity)entity))
                    blockEntity = (TBlockEntity)entity;
            }, true);
            return blockEntity;
        }

        public static TBlockEntity GetNearestBlockEntity<TBlockEntity>(this IWorldAccessor world, BlockPos pos,
            float horRange, float vertRange) where TBlockEntity : BlockEntity
        {
            return world.GetNearestBlockEntity<TBlockEntity>(pos, horRange, vertRange, _ => true);
        }

        public static TBlock GetNearestBlock<TBlock>(this IWorldAccessor world, BlockPos pos,
            float horRange, float vertRange, System.Func<TBlock, bool> predicate, out BlockPos blockPosOut) where TBlock : Block
        {
            TBlock blockEntity = null;
            BlockPos blockPosTemp = null;
            var found = false;
            var minPos = pos.AddCopy(-horRange, -vertRange, -horRange);
            var maxPos = pos.AddCopy(horRange, vertRange, horRange);
            world.BlockAccessor.WalkBlocks(minPos, maxPos, (block, blockPos) =>
            {
                if (found) return;
                if (block.GetType() != typeof(TBlock) || !predicate((TBlock)block)) return;
                blockEntity = (TBlock)block;
                blockPosTemp = blockPos.DeepClone();
                found = true;
            }, true);
            blockPosOut = blockPosTemp;
            return blockEntity;
        }

        public static TBlock GetNearestBlock<TBlock>(this IWorldAccessor world, BlockPos pos,
            float horRange, float vertRange, out BlockPos blockPosOut) where TBlock : Block
        {
            return world.GetNearestBlock<TBlock>(pos, horRange, vertRange, _ => true, out blockPosOut);
        }

        /// <summary>
        ///     Determines whether one <see cref="BlockPos"/> is within three-dimensional range of another <see cref="BlockPos"/>.
        /// </summary>
        /// <param name="sourceBlockPos">The position under test.</param>
        /// <param name="originBlockPos">The position used as the origin of the search.</param>
        /// <param name="horizontalRadius">The horizontal radius for the search.</param>
        /// <param name="verticalRadius">The vertical radius for the search.</param>
        /// <returns><c>true</c> if the source block position is within the specified range of the origin position, <c>false</c> otherwise.</returns>
        public static bool InRangeCubic(
            this BlockPos sourceBlockPos, 
            BlockPos originBlockPos, 
            int horizontalRadius = 10,
            int verticalRadius = 10)
        {
            if (!sourceBlockPos.InRangeHorizontally(originBlockPos.X, originBlockPos.Z, horizontalRadius)) return false;
            return sourceBlockPos.Y <= originBlockPos.Y + verticalRadius && sourceBlockPos.Y >= originBlockPos.Y - verticalRadius;
        }
    }
}
